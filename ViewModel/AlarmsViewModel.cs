/* ===================================================================
 License:
    DiscerningEye - FFXIV Gathering Dictionary and Alarm
    AlarmsViewModel.cs


    Copyright(C) 2015 - 2016  Christopher Whitley

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.If not, see<http://www.gnu.org/licenses/> .
  =================================================================== */


using DiscerningEye.DataAccess;
using DiscerningEye.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Speech;
using System.Speech.Synthesis;
using NAudio;
using NAudio.Wave;
using System.IO;
using MahApps.Metro.Controls.Dialogs;
using System.Threading.Tasks;
using System.Linq;
using System.Windows.Input;

namespace DiscerningEye.ViewModel
{
    public class AlarmsViewModel : ViewModelBase
    {
        //=========================================================
        //  Private Fields
        //=========================================================
        private AlarmItemRepository _alarmItemRepository;
        private ObservableCollection<Model.AlarmItem> _alarmItemCollection;
        private ObservableCollection<Model.AlarmProfile> _alarmProfileCollection;
        private Model.AlarmProfile _selectedProfile;
        private string _searchText;
        private Utilities.ClockController _eorzeaClock;
        private System.Timers.Timer _updateTimer;
        private bool _isLoaded;
        private IWavePlayer _waveOutDevice;
        private AudioFileReader _audioFileReader;
        private SpeechSynthesizer synth;

        //=========================================================
        //  Properties
        //=========================================================
        /// <summary>
        /// Gets or sets the observablecollection of Model.AlarmItem
        /// </summary>
        /// <remarks>
        /// This is bound to the ItemSource property of the datagrid on the AlarmView
        /// </remarks>
        public ObservableCollection<Model.AlarmItem> AlarmItemCollection
        {
            get {
                
                return this._alarmItemCollection; }
            set
            {
                if (this._alarmItemCollection == value) return;
                this._alarmItemCollection = value;
                OnPropertyChanged("AlarmItemCollection");
                


            }
        }

        public ObservableCollection<Model.AlarmProfile> AlarmProfileCollection
        {
            get { return this._alarmProfileCollection; }
            set
            {
                if (this._alarmProfileCollection == value) return;
                this._alarmProfileCollection = value;
                OnPropertyChanged("AlarmProfileCollection");
            }
        }

        public Model.AlarmProfile SelectedProfile
        {
            get { return this._selectedProfile; }
            set
            {
                if (this._selectedProfile == value) return;
                this._selectedProfile = value;
                OnPropertyChanged("SelectedProfile");
            }
        }





        /// <summary>
        /// Gets or sets the serach text
        /// </summary>
        /// <remarks>
        /// This is bound to the Text property of the search textbox on the AlarmView
        /// </remarks>
        public string SearchText
        {
            get { return this._searchText; }
            set
            {
                if (this._searchText == value) return;
                this._searchText = value;
                OnPropertyChanged("SearchText");
                if (value == "") this.SearchAlarmsCommand.Execute(null);
            }
        }

        /// <summary>
        /// Gets or sets the ClockController used to get the eorzeatime
        /// </summary>
        /// <remarks>
        /// This is bound within the MainView
        /// </remarks>
        public Utilities.ClockController EorzeaClock
        {
            get { return this._eorzeaClock; }
            set
            {
                if (this._eorzeaClock == value) return;
                this._eorzeaClock = value;
                OnPropertyChanged("EorzeaClock");
            }
        }

        /// <summary>
        /// Gets or set the System.Timers.Timer
        /// </summary>
        /// <remarks>
        /// The elapsed event of this timer is used to calculate
        /// all things related to the alarms
        /// </remarks>
        public System.Timers.Timer UpdateTimer
        {
            get { return this._updateTimer; }
            set
            {
                if (this._updateTimer == value) return;
                this._updateTimer = value;
            }
        }


        public bool CanAdjustSelectedProfile
        {
            get
            {
                if (this.SelectedProfile == null) return false;
                else return true;
            }
        }



        public ICommand CreateNewProfileCommand
        {
            get;
            private set;
        }

        public ICommand LoadProfileCommand
        {
            get;
            private set;
        }

        public ICommand UpdateProfileCommand
        {
            get;
            private set;
        }


        public ICommand SearchAlarmsCommand
        {
            get;
            private set;
        }

        public ICommand DeleteCurrentProfileCommand
        {
            get;
            private set;
        }

        public ICommand RefreshSchedulViewCommand
        {
            get;
            private set;
        }


        public ICommand RemoveAllAlarmsCommand
        {
            get;
            private set;
        }

        //=========================================================
        //  Constructor
        //=========================================================
        /// <summary>
        /// Creates a new instance of AlarmsViewModel
        /// </summary>
        public AlarmsViewModel()
        {
            AlarmsView.View.Loaded += View_Loaded;
            this.CreateNewProfileCommand = new Commands.AlarmViewModelCommands.CreateAlarmProfileComand(this);
            this.LoadProfileCommand = new Commands.AlarmViewModelCommands.LoadAlarmProfileCommand(this);
            this.UpdateProfileCommand = new Commands.AlarmViewModelCommands.UpdateAlarmProfileCommand(this);
            this.SearchAlarmsCommand = new Commands.AlarmViewModelCommands.SearchAlarmsCommand(this);
            this.DeleteCurrentProfileCommand = new Commands.AlarmViewModelCommands.DeleteAlarmProfileCommand(this);
            this.RefreshSchedulViewCommand = new Commands.AlarmViewModelCommands.RefreshScheduleViewCommand(this);
            this.RemoveAllAlarmsCommand = new Commands.AlarmViewModelCommands.RemoveAllAlarmsCommand(this);
            this._isLoaded = false;

        }

        /// <summary>
        /// Called when the AlarmView user control is loaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void View_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            if (this._isLoaded) return;

            //  Initilize the alarm item repository
            if (_alarmItemRepository == null)
                _alarmItemRepository = new AlarmItemRepository();

            this.SearchText = "";
            
            //  Initilize the AlarmItemCollection
            this.AlarmItemCollection = new ObservableCollection<Model.AlarmItem>(_alarmItemRepository.GetAlarmItems());


            //  Initilize the Alarm Profiles
            this.AlarmProfileCollection = new ObservableCollection<Model.AlarmProfile>((new AlarmProfileRepository()).GetAlarmProfiles());
            ((CollectionViewSource)AlarmsView.View.FindResource("AlarmViewSource")).Filter += AlarmsViewModel_Filter;
            ((CollectionViewSource)AlarmsView.View.FindResource("SetAlarmsViewSource")).Filter += SetAlarmsViewSource_Filter;









            // Initilize the update timer
            this.UpdateTimer = new System.Timers.Timer();
            this.UpdateTimer.Interval = 100;
            this.UpdateTimer.AutoReset = false;
            this.UpdateTimer.Elapsed += UpdateTimer_Elapsed;
            this.UpdateTimer.Start();

            _waveOutDevice = new WaveOut();
            _waveOutDevice.PlaybackStopped += _waveOutDevice_PlaybackStopped;
            this.EorzeaClock = new Utilities.ClockController();
            synth = new SpeechSynthesizer();


            this._isLoaded = true;
        }
        private void AlarmsViewModel_Filter(object sender, FilterEventArgs e)
        {
            Model.AlarmItem i = e.Item as Model.AlarmItem;
            if (i != null)
            {
                if (this.SearchText == null) this.SearchText = "";
                e.Accepted = i.Name.ToLower().Contains(this.SearchText.ToLower());
            }
        }

        private void SetAlarmsViewSource_Filter(object sender, FilterEventArgs e)
        {
            Model.AlarmItem i = e.Item as Model.AlarmItem;
            if (i != null)
            {
                if (this.SearchText == null) this.SearchText = "";
                e.Accepted = i.IsSet == true;
            }
        }






        //=========================================================
        //  Events
        //=========================================================
        /// <summary>
        /// AlarmInfo struct used to temporaraly hold data about an alarm item
        /// during the UpdateTimer_Elapsed event
        /// </summary>
        struct AlarmInfo
        {
            public bool IsSet;
            public string Name;
            public string Region;
            public string Zone;
            public TimeSpan StartTime;
            public TimeSpan NextSpawn;
            public bool Armed;
            public bool EarlyWarningIssued;
        }

        private void UpdateTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            

            //  Get the current eorzea time span
            TimeSpan currentEorzeaTimeSpan = this.EorzeaClock.GetEorzeaTimeSpan();

            List<string> earlyWarningMessages = new List<string>();
            List<string> itemAvailableMessages = new List<string>();
            StringBuilder notificationMessage = new StringBuilder();

            //  Go through each of the alarm items
            foreach (Model.AlarmItem alarmItem in this.AlarmItemCollection)
            {    

                //  Get info about the alarm item
                AlarmInfo alarmInfo = new AlarmInfo();
                alarmInfo.IsSet = alarmItem.IsSet;
                alarmInfo.Name = alarmItem.Name;
                alarmInfo.Region = alarmItem.Region;
                alarmInfo.Zone = alarmItem.Zone;
                TimeSpan.TryParse(alarmItem.StartTime, out alarmInfo.StartTime);
                alarmInfo.Armed = alarmItem.Armed;
                alarmInfo.EarlyWarningIssued = alarmItem.EarlyWarningIssued;
                ///TimeSpan.TryParse(alarmItem.NextSpawn, out alarmInfo.NextSpawn);
                alarmInfo.NextSpawn = alarmItem.NextSpawn;

                //  Because we use the pretrigger of the alarms, we need a way to rearm the alarm
                //  without it triggering over and over until the hour has passed.  So we will
                //  check to see if the current eorzea time is > than the alarm time and if so
                //  we just arm it
                #region CheckIfReArm
                if (currentEorzeaTimeSpan > alarmInfo.StartTime)
                {
                    //  becuase we use 24 hour time, zero hour alarms will rearm on any hour
                    //  meaning they will continuously rearm immiedialy after alarming unless
                    //  we perform a hacky check
                    if ((alarmInfo.StartTime.Hours == 0 && currentEorzeaTimeSpan.Hours < 2) || alarmInfo.StartTime.Hours > 0)
                    {
                        alarmInfo.Armed = true;
                        alarmInfo.EarlyWarningIssued = false;
                    }
                }
                #endregion CheckIfReArm



                #region CalculateTimeTillSpawn
                if (alarmInfo.IsSet)
                {
                    //  Get the time difference between the alarm time and eorzea time
                    TimeSpan timeDiff;
                    TimeSpan nextEorzeaSpawn;
                    if (alarmInfo.StartTime.Equals(new TimeSpan(0, 0, 0)))
                    {
                        timeDiff = (new TimeSpan(24, 0, 0)).Subtract(currentEorzeaTimeSpan);
                    }
                    else
                    {
                        timeDiff = alarmInfo.StartTime.Subtract(currentEorzeaTimeSpan);
                    }



                    if (alarmInfo.StartTime > currentEorzeaTimeSpan)
                    {
                        //alarmInfo.TimeTillSpawnEorzea = alarmInfo.StartTime.Subtract(currentEorzeaTimeSpan);
                        nextEorzeaSpawn = alarmInfo.StartTime.Subtract(currentEorzeaTimeSpan);
                    }
                    else
                    {
                        //alarm.TimeTillSpawnEorzea = ((TimeSpan)new TimeSpan(23, 59, 59)).Subtract(currentEorzeaTimeSpan.Subtract(alarm.StartTime));
                        nextEorzeaSpawn = ((TimeSpan)new TimeSpan(23, 59, 59)).Subtract(currentEorzeaTimeSpan.Subtract(alarmInfo.StartTime));
                    }
                    long earthTicks = nextEorzeaSpawn.Ticks / (long)Utilities.ClockController.EORZEA_MULTIPLIER;
                    alarmInfo.NextSpawn = new TimeSpan(earthTicks);

                    //  Check if the NexSpawn time is 0h 0m and 0s.  If so, and fullZeroExists is false
                    //  set fullZeroExists to true
                    //if (alarmInfo.NextSpawn == new TimeSpan(0, 0, 0) && fullZeroExists == false)
                    //{
                    //    fullZeroExists = true;
                    //}
                }
                
                #endregion CalculateTimeTillSpawn




                //  Check if we should issue an early warning
                if(Properties.Settings.Default.EnableEarlyWarning && alarmInfo.IsSet)
                {
                    //  Check if the alarm should issue an early warning
                    if(alarmInfo.NextSpawn.TotalMinutes <= Properties.Settings.Default.EarlyWarningMinutes && !alarmInfo.EarlyWarningIssued)
                    {
                        alarmInfo.EarlyWarningIssued = true;

                        string earlyWarningMessage = Properties.Settings.Default.EarlyWarningMessage;
                        earlyWarningMessage = System.Text.RegularExpressions.Regex.Replace(earlyWarningMessage, "{item}", alarmInfo.Name, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                        earlyWarningMessage = System.Text.RegularExpressions.Regex.Replace(earlyWarningMessage, "{region}", alarmInfo.Region, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                        earlyWarningMessage = System.Text.RegularExpressions.Regex.Replace(earlyWarningMessage, "{zone}", alarmInfo.Zone, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                        string timeMessage = "";
                        //  Check hours
                        if (alarmInfo.NextSpawn.Hours != 0)
                        {
                            if (alarmInfo.NextSpawn.Hours == 1)
                                timeMessage += "1 hour";
                            else
                                timeMessage += string.Format("{0} hours", alarmInfo.NextSpawn.Hours);
                        }

                        if (alarmInfo.NextSpawn.Minutes != 0)
                        {
                            if (alarmInfo.NextSpawn.Minutes == 1)
                                timeMessage += " 1 minute";
                            else
                                timeMessage += string.Format(" {0} minutes", alarmInfo.NextSpawn.Minutes);
                        }

                        if (alarmInfo.NextSpawn.Seconds != 0)
                        {
                            if (alarmInfo.NextSpawn.Seconds == 1)
                                timeMessage += " 1 second";
                            else
                                timeMessage += string.Format(" {0} seconds", alarmInfo.NextSpawn.Seconds);
                        }

                        
                        earlyWarningMessage = System.Text.RegularExpressions.Regex.Replace(earlyWarningMessage, "{time}", timeMessage, System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                        earlyWarningMessages.Add(earlyWarningMessage);



                    }
                }




                //  Check if alarm should be triggered
                if(alarmInfo.NextSpawn <= new TimeSpan(0,0,2) && alarmInfo.Armed && alarmInfo.IsSet)
                {
                    alarmInfo.Armed = false;

                    string spawnMessage = Properties.Settings.Default.ItemAvailableMessage;
                    spawnMessage = System.Text.RegularExpressions.Regex.Replace(spawnMessage, "{item}", alarmInfo.Name, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                    spawnMessage = System.Text.RegularExpressions.Regex.Replace(spawnMessage, "{region}", alarmInfo.Region, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                    spawnMessage = System.Text.RegularExpressions.Regex.Replace(spawnMessage, "{zone}", alarmInfo.Zone, System.Text.RegularExpressions.RegexOptions.IgnoreCase);


                    itemAvailableMessages.Add(spawnMessage);

                }






                //  Push the alarmInfo back into the alarmItem
                alarmItem.Armed = alarmInfo.Armed;
                alarmItem.EarlyWarningIssued = alarmInfo.EarlyWarningIssued;
                alarmItem.NextSpawn = alarmInfo.NextSpawn;





            }


            //  Check if there are any notifications to push
            if (earlyWarningMessages.Count > 0)
            {
                notificationMessage.AppendLine("Early Warnings:");
                foreach(string message in earlyWarningMessages)
                {
                    notificationMessage.AppendLine(message);
                }
            }

            if(itemAvailableMessages.Count > 0)
            {
                notificationMessage.AppendLine("Items Available:");
                foreach (string message in itemAvailableMessages)
                {
                    notificationMessage.AppendLine(message);
                }
            }


            if (notificationMessage.Length > 0)
            {
                if(Properties.Settings.Default.EnableBallonTip)
                    MainWindow.View.notificationIcon.ShowBalloonTip("Discerning Eye: Notificaitons", notificationMessage.ToString(), Hardcodet.Wpf.TaskbarNotification.BalloonIcon.Info);

                if (Properties.Settings.Default.EnableTextToSpeech)
                {
                    if (synth.State == SynthesizerState.Ready)
                        synth.SpeakAsync(notificationMessage.ToString());
                }

                if(Properties.Settings.Default.EnableNotificationTone && File.Exists(Properties.Settings.Default.NotificationToneUri))
                {
                    if (_waveOutDevice.PlaybackState != PlaybackState.Playing)
                    {

                        _audioFileReader = new AudioFileReader(Properties.Settings.Default.NotificationToneUri);
                        _waveOutDevice.Init(_audioFileReader);
                        _waveOutDevice.Play();
                    }
                }
                    
                    
            }

            this.UpdateTimer.Start();
        }

        private void _waveOutDevice_PlaybackStopped(object sender, StoppedEventArgs e)
        {

        }


        //=========================================================
        //  Interface Implementations
        //=========================================================
        #region IDisposeable Implementation

        protected override void OnDispose()
        {
            this.AlarmItemCollection.Clear();
            if (this.synth != null)
            {
                this.synth.SpeakAsyncCancelAll();
                this.synth = null;
            }

            if (_waveOutDevice != null)
            {
                _waveOutDevice.PlaybackStopped -= _waveOutDevice_PlaybackStopped;
                _waveOutDevice.Stop();
            }
            if (_audioFileReader != null)
            {
                _audioFileReader.Dispose();
                _audioFileReader = null;
            }
            if (_waveOutDevice != null)
            {
                _waveOutDevice.Dispose();
                _waveOutDevice = null;
            }
        }

        #endregion IDisposeable Implementation



        //=========================================================
        //  Methods
        //=========================================================
        public void SearchAlarms()
        {
            ((CollectionViewSource)AlarmsView.View.FindResource("AlarmViewSource")).Filter -= AlarmsViewModel_Filter;
            ((CollectionViewSource)AlarmsView.View.FindResource("AlarmViewSource")).Filter += AlarmsViewModel_Filter;
        }

        public async void LoadProfile()
        {
            foreach(Model.AlarmItem profileAlarm in this.SelectedProfile.Alarms)
            {
                foreach(Model.AlarmItem collectionAlarm in this.AlarmItemCollection)
                {
                    if (profileAlarm.Name == collectionAlarm.Name && profileAlarm.StartTime == collectionAlarm.StartTime)
                    {
                        collectionAlarm.EarlyWarningIssued = false;
                        collectionAlarm.Armed = true;
                        collectionAlarm.IsSet = profileAlarm.IsSet;
                    }
                }
            }

            this.RefreshScheduleView();


            MetroDialogSettings settings = new MetroDialogSettings();
            settings.AffirmativeButtonText = "Ok";
            settings.AnimateHide = true;
            settings.AnimateShow = true;
            settings.DefaultButtonFocus = MessageDialogResult.Affirmative;
            await MainWindow.View.ShowMessageAsync("Schedule Loaded ", string.Format("Schedule \"{0}\" has been loaded.", this.SelectedProfile.Name), MessageDialogStyle.Affirmative, settings);
        }

        public async void CreateNewProfile()
        {
            //  Ensure a profile with the same name doens't already exist
            
            
            MetroDialogSettings settings = new MetroDialogSettings();
            settings.AffirmativeButtonText = "Create";
            settings.AnimateHide = true;
            settings.AnimateShow = true;
            settings.DefaultButtonFocus = MessageDialogResult.Affirmative;
            settings.NegativeButtonText = "Cancel";
            string profileName = await MainWindow.View.ShowInputAsync("Create New Schedule", "Enter the name you would like to use for the new schedule", settings);

            var list = this.AlarmProfileCollection.Where(ap => ap.Name == profileName);

            if (list.Count() > 0)
            {
                settings.AffirmativeButtonText = "Ok";
                await MainWindow.View.ShowMessageAsync("Create Schedule Error", "Cannot create a profile with the same name as an existing one", MessageDialogStyle.Affirmative, settings);

            }
            else
            {

                if (profileName == null) return;
                Model.AlarmProfile profile = new Model.AlarmProfile(this.AlarmItemCollection.ToList());

                if (profileName.Length > 0)
                {
                    profile.Name = profileName;
                    AlarmProfileRepository.Save(profile);
                }

                this.AlarmProfileCollection.Add(profile);
                settings.AffirmativeButtonText = "Ok";
                await MainWindow.View.ShowMessageAsync("Schedule Created", string.Format("Profile has been created and saved for {0}", profileName), MessageDialogStyle.Affirmative, settings);
                this.AlarmProfileCollection = new ObservableCollection<Model.AlarmProfile>((new AlarmProfileRepository()).GetAlarmProfiles());
                this.SelectedProfile = this.AlarmProfileCollection.Where(p => p.Name == profile.Name).First();
            }

            


        }

        public async void UpdateCurrentProfile()
        {
            if (this.SelectedProfile == null) return;





            string profilename = this.SelectedProfile.Name;
            Model.AlarmProfile profile = new Model.AlarmProfile(this.AlarmItemCollection.ToList());
            profile.Name = profilename;

            this.AlarmProfileCollection.Remove(this.AlarmProfileCollection.Where(p => p.Name == this.SelectedProfile.Name).First());
            this.AlarmProfileCollection.Add(profile);
            this.SelectedProfile = null;
            this.SelectedProfile = profile;
            AlarmProfileRepository.Save(this.SelectedProfile);

            MetroDialogSettings settings = new MetroDialogSettings();
            settings.AffirmativeButtonText = "Ok";
            settings.AnimateHide = true;
            settings.AnimateShow = true;
            settings.DefaultButtonFocus = MessageDialogResult.Affirmative;
            await MainWindow.View.ShowMessageAsync("Schedule Updated ", string.Format("Schedule \"{0}\" has been updated.", profile.Name), MessageDialogStyle.Affirmative, settings);

        }


        public async void DeleteCurrentProfile()
        {
            string profileName = this.SelectedProfile.Name;
            AlarmProfileRepository.Delete(this.SelectedProfile);
            
            this.AlarmProfileCollection.Remove(this.AlarmProfileCollection.Where(profile => profile.Name == this.SelectedProfile.Name).First());
            this.SelectedProfile = null;
            this.RemoveAllAlarms();


            MetroDialogSettings settings = new MetroDialogSettings();
            settings.AffirmativeButtonText = "Ok";
            settings.AnimateHide = true;
            settings.AnimateShow = true;
            settings.DefaultButtonFocus = MessageDialogResult.Affirmative;
            await MainWindow.View.ShowMessageAsync("Schedule Deleted ", string.Format("Schedule \"{0}\" has been deleted.", profileName), MessageDialogStyle.Affirmative, settings);

        }

        public void RefreshScheduleView()
        {
            ((CollectionViewSource)AlarmsView.View.FindResource("SetAlarmsViewSource")).View.Refresh();
        }

        public void RemoveAllAlarms()
        {
            foreach(Model.AlarmItem alarmItem in this.AlarmItemCollection)
            {
                alarmItem.IsSet = false;
            }
            this.RefreshScheduleView();
        }
    }
}
