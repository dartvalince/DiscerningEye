/* ===================================================================
 License:
    DiscerningEye - FFXIV Gathering Companion App
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
using DiscerningEye.Views;
using MahApps.Metro.Controls.Dialogs;
using NAudio.Wave;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Windows.Data;
using System.Windows.Input;

namespace DiscerningEye.ViewModels
{
    public class AlarmsViewModel : ViewModelBase
    {
        private AlarmItemRepository _alarmItemRepository;
        private AudioFileReader _audioFileReader;
        private SpeechSynthesizer _synth;
        private System.Timers.Timer _updateTimer;
        private IWavePlayer _waveOutDevice;

        private ObservableCollection<Model.AlarmItem> _alarmItemCollection;
        /// <summary>
        /// Gets or sets the observablecollection of Model.AlarmItem
        /// </summary>
        /// <remarks>
        /// This is bound to the ItemSource property of the datagrid on the AlarmView
        /// </remarks>
        public ObservableCollection<Model.AlarmItem> AlarmItemCollection
        {
            get { return this._alarmItemCollection; }
            set
            {
                SetProperty(ref _alarmItemCollection, value);
                //if (this._alarmItemCollection == value) return;
                //this._alarmItemCollection = value;
                //OnPropertyChanged("AlarmItemCollection");
            }
        }


        private ObservableCollection<Model.AlarmSchedule> _alarmScheduleCollection;
        /// <summary>
        /// Gets or sets the observablecollection of Model.AlarmSchedul
        /// </summary>
        /// <remarks>
        /// This is bound ot the ItemSource property of the combobox on the AlarmView
        /// </remarks>
        public ObservableCollection<Model.AlarmSchedule> AlarmScheduleCollection
        {
            get { return this._alarmScheduleCollection; }
            set
            {
                SetProperty(ref _alarmScheduleCollection, value);
                //if (this._alarmScheduleCollection == value) return;
                //this._alarmScheduleCollection = value;
                //OnPropertyChanged("AlarmScheduleCollection");
            }
        }


        /// <summary>
        /// Gets or Sets the Boolean value used to determine if a schedule can be adjusted
        /// </summary>
        /// <remarks>
        /// This is used by the various ICommands for the CanExecute for the buttons on the AlarmView that deal
        /// with schedule loading, creating, updating and deleting
        /// </remarks>
        public bool CanAdjustSelectedSchedule
        {
            get
            {
                if (this.SelectedSchedule == null) return false;
                else return true;
            }
        }

        private Utilities.ClockController _eorzeaClock;
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
                SetProperty(ref _eorzeaClock, value);
                //if (this._eorzeaClock == value) return;
                //this._eorzeaClock = value;
                //OnPropertyChanged("EorzeaClock");
            }
        }


        private string _searchText;
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
                SetProperty(ref _searchText, value);
                //if (this._searchText == value) return;
                //this._searchText = value;
                //OnPropertyChanged("SearchText");
                if (value == "") this.SearchAlarmsCommand.Execute();
            }
        }

        private Model.AlarmSchedule _selectedSchedule;
        /// <summary>
        /// Gets or sets the current selected Model.AlarmSchedule
        /// </summary>
        /// <remarks>
        /// This is bound to the SelectedValue property of the combobox on the AlarmView
        /// </remarks>
        public Model.AlarmSchedule SelectedSchedule
        {
            get { return this._selectedSchedule; }
            set
            {
                SetProperty(ref _selectedSchedule, value);
                //if (this._selectedSchedule == value) return;
                //this._selectedSchedule = value;
                //OnPropertyChanged("SelectedSchedule");
            }
        }


        

        //=========================================================
        //  ICommands
        //=========================================================
        /// <summary>
        /// Gets or sets the DelegateCommand used to create a new schedule
        /// </summary>
        public DelegateCommand CreateNewScheduleCommand { get; private set; }


        /// <summary>
        /// Gets or sets the DelegateCommand used to load the selected schedule
        /// </summary>
        public DelegateCommand LoadScheduleCommand { get; private set; }

        /// <summary>
        /// Gets or sets the DelegateCommand used to update the selected schedule
        /// </summary>
        public DelegateCommand UpdateScheduleCommand { get; private set; }


        /// <summary>
        /// Gets or sets the DelegateCommand used to search the alarms
        /// </summary>
        public DelegateCommand SearchAlarmsCommand { get; private set; }


        /// <summary>
        /// Gets or sets the DelegateCommand used to delete the current selected schedule
        /// </summary>
        public DelegateCommand DeleteCurrentScheduleCommand { get; private set; }


        /// <summary>
        /// Gets or sets the DelegateCommand to use to refresh the scheduled alarms view
        /// </summary>
        public DelegateCommand RefreshSchedulViewCommand { get; private set; }

        /// <summary>
        /// Gets or sets the DelegateCommand used to remove all alarms from the schedule alarms view
        /// </summary>
        public DelegateCommand RemoveAllAlarmsCommand { get; private set; }


        //=========================================================
        //  Constructor
        //=========================================================
        /// <summary>
        /// Creates a new instance of AlarmsViewModel
        /// </summary>
        public AlarmsViewModel()
        {
            //  Setup Commands
            this.CreateNewScheduleCommand = new DelegateCommand(this.CreateNewSchedule, () => true);
            this.LoadScheduleCommand = new DelegateCommand(this.LoadSchedule, () => this.CanAdjustSelectedSchedule).ObservesProperty(() => this.SelectedSchedule);
            this.UpdateScheduleCommand = new DelegateCommand(this.UpdateCurrentSchedule, () => this.CanAdjustSelectedSchedule).ObservesProperty(() => this.SelectedSchedule);
            this.DeleteCurrentScheduleCommand = new DelegateCommand(this.DeleteCurrentSchedule, () => this.CanAdjustSelectedSchedule).ObservesProperty(() => this.SelectedSchedule);

            this.SearchAlarmsCommand = new DelegateCommand(this.SearchAlarms, () => !string.IsNullOrEmpty(this.SearchText) && !string.IsNullOrWhiteSpace(this.SearchText)).ObservesProperty(() => this.SearchText);
            this.RefreshSchedulViewCommand = new DelegateCommand(this.RefreshScheduleView, () => true);
            this.RemoveAllAlarmsCommand = new DelegateCommand(this.RemoveAllAlarms, () => true);


            //  Initilize the alarm item repository
            if (_alarmItemRepository == null)
                _alarmItemRepository = new AlarmItemRepository();

            //this.SearchText = "";

            //  Initilize the AlarmItemCollection
            this.AlarmItemCollection = new ObservableCollection<Model.AlarmItem>(_alarmItemRepository.GetAlarmItems());

            //((CollectionViewSource)AlarmsView.View.FindResource("AlarmViewSource")).Filter += AlarmViewSource_Filter;
            //((CollectionViewSource)AlarmsView.View.FindResource("SetAlarmsViewSource")).Filter += SetAlarmsViewSource_Filter;


            //  Initilize the Alarm Schedules Collection
            this.AlarmScheduleCollection = new ObservableCollection<Model.AlarmSchedule>((new AlarmScheduleRepository()).GetAlarmSchedules());


            //  Initilize the Eorzea Clock
            this.EorzeaClock = new Utilities.ClockController();

            // Initilize the update timer
            this._updateTimer = new System.Timers.Timer();
            this._updateTimer.Interval = 100;
            this._updateTimer.AutoReset = false;
            this._updateTimer.Elapsed += UpdateTimer_Elapsed;
            this._updateTimer.Start();


            //  Initilize the sound player
            _waveOutDevice = new WaveOut();

            //  Initilize the text-to-speech object
            _synth = new SpeechSynthesizer();

        }


        //=========================================================
        //  Events
        //=========================================================
        /// <summary>
        /// Filter event used for the AlarmViewSource
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AlarmViewSource_Filter(object sender, FilterEventArgs e)
        {
            Model.AlarmItem i = e.Item as Model.AlarmItem;
            if (i != null)
            {
                if (this.SearchText == null) this.SearchText = "";
                e.Accepted = i.Name.ToLower().Contains(this.SearchText.ToLower());
            }
        }

        /// <summary>
        /// Filter event used for the SetAlarmsViewSource
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SetAlarmsViewSource_Filter(object sender, FilterEventArgs e)
        {
            Model.AlarmItem i = e.Item as Model.AlarmItem;
            if (i != null)
            {
                if (this.SearchText == null) this.SearchText = "";
                e.Accepted = i.IsSet == true;
            }
        }

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

        /// <summary>
        /// Elapsed event for the update timer. Main logic of view model occurs here
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

            if (Properties.Settings.Default.DoNotDisturb == false)
            {
                //  Check if there are any notifications to push
                if (earlyWarningMessages.Count > 0)
                {
                    notificationMessage.AppendLine("Early Warnings:");
                    foreach (string message in earlyWarningMessages)
                    {
                        notificationMessage.AppendLine(message);
                    }
                }

                if (itemAvailableMessages.Count > 0)
                {
                    notificationMessage.AppendLine("Items Available:");
                    foreach (string message in itemAvailableMessages)
                    {
                        notificationMessage.AppendLine(message);
                    }
                }


                if (notificationMessage.Length > 0)
                {
                    if (Properties.Settings.Default.EnableBallonTip)
                        MainWindow.View.notificationIcon.ShowBalloonTip("Discerning Eye: Notificaitons", notificationMessage.ToString(), Hardcodet.Wpf.TaskbarNotification.BalloonIcon.Info);

                    if (Properties.Settings.Default.EnableTextToSpeech)
                    {
                        if (_synth.State == SynthesizerState.Ready)
                        {
                            _synth.Volume = Properties.Settings.Default.TextToSpeechVolume;
                            _synth.SpeakAsync(notificationMessage.ToString());
                        }
                    }

                    if (Properties.Settings.Default.EnableNotificationTone && File.Exists(Properties.Settings.Default.NotificationToneUri))
                    {
                        if (_waveOutDevice.PlaybackState != PlaybackState.Playing)
                        {

                            _audioFileReader = new AudioFileReader(Properties.Settings.Default.NotificationToneUri);
                            _audioFileReader.Volume = (float)Properties.Settings.Default.NotificationToneVolume / 100.0f;
                            _waveOutDevice.Init(_audioFileReader);
                            _waveOutDevice.Play();
                        }
                    }
                }

            }
            this._updateTimer.Start();
        }


        //=========================================================
        //  Interface Implementations
        //=========================================================
        #region IDisposeable Implementation

        protected override void OnDispose()
        {
            this.AlarmItemCollection.Clear();
            this.AlarmScheduleCollection.Clear();

            ((CollectionViewSource)AlarmsView.View.FindResource("AlarmViewSource")).Filter -= AlarmViewSource_Filter;
            ((CollectionViewSource)AlarmsView.View.FindResource("SetAlarmsViewSource")).Filter -= SetAlarmsViewSource_Filter;

            this._updateTimer.Stop();
            this._updateTimer.Elapsed -= UpdateTimer_Elapsed;
            this._updateTimer.Dispose();

            if (this._synth != null)
            {
                this._synth.SpeakAsyncCancelAll();
                this._synth = null;
            }

            if (_waveOutDevice != null)
            {
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
        /// <summary>
        /// Refreshes the AlarmsViewSource which updates the filter
        /// </summary>
        public void SearchAlarms()
        {
            CollectionViewSource alarmsViewSource = ((CollectionViewSource)AlarmsView.View.FindResource("AlarmViewSource"));
            if (alarmsViewSource.View != null) alarmsViewSource.View.Refresh();
        }

        /// <summary>
        /// Loads the Selecte Schedule
        /// </summary>
        public async void LoadSchedule()
        {
            foreach(Model.AlarmItem scheduleAlarm in this.SelectedSchedule.Alarms)
            {
                foreach(Model.AlarmItem collectionAlarm in this.AlarmItemCollection)
                {
                    if (scheduleAlarm.Name == collectionAlarm.Name && scheduleAlarm.StartTime == collectionAlarm.StartTime)
                    {
                        collectionAlarm.EarlyWarningIssued = false;
                        collectionAlarm.Armed = true;
                        collectionAlarm.IsSet = scheduleAlarm.IsSet;
                    }
                }
            }

            this.RefreshScheduleView();


            MetroDialogSettings settings = new MetroDialogSettings();
            settings.AffirmativeButtonText = "Ok";
            settings.AnimateHide = true;
            settings.AnimateShow = true;
            settings.DefaultButtonFocus = MessageDialogResult.Affirmative;
            await MainWindow.View.ShowMessageAsync("Schedule Loaded ", string.Format("Schedule \"{0}\" has been loaded.", this.SelectedSchedule.Name), MessageDialogStyle.Affirmative, settings);
        }

        /// <summary>
        /// Creates a new schedule based on the current selected alarms
        /// </summary>
        public async void CreateNewSchedule()
        {
            MetroDialogSettings settings = new MetroDialogSettings();
            settings.AffirmativeButtonText = "Create";
            settings.AnimateHide = true;
            settings.AnimateShow = true;
            settings.DefaultButtonFocus = MessageDialogResult.Affirmative;
            settings.NegativeButtonText = "Cancel";
            string scheduleName = await MainWindow.View.ShowInputAsync("Create New Schedule", "Enter the name you would like to use for the new schedule", settings);

            var list = this.AlarmScheduleCollection.Where(ap => ap.Name == scheduleName);

            if (list.Count() > 0)
            {
                settings.AffirmativeButtonText = "Ok";
                await MainWindow.View.ShowMessageAsync("Create Schedule Error", "Cannot create a sachedule with the same name as an existing one", MessageDialogStyle.Affirmative, settings);

            }
            else
            {

                if (scheduleName == null) return;
                Model.AlarmSchedule schedule = new Model.AlarmSchedule(this.AlarmItemCollection.ToList());

                if (scheduleName.Length > 0)
                {
                    schedule.Name = scheduleName;
                    AlarmScheduleRepository.Save(schedule);
                }

                this.AlarmScheduleCollection.Add(schedule);
                settings.AffirmativeButtonText = "Ok";
                await MainWindow.View.ShowMessageAsync("Schedule Created", string.Format("Schedule has been created and saved for {0}", scheduleName), MessageDialogStyle.Affirmative, settings);
                this.AlarmScheduleCollection = new ObservableCollection<Model.AlarmSchedule>((new AlarmScheduleRepository()).GetAlarmSchedules());
                this.SelectedSchedule = this.AlarmScheduleCollection.Where(p => p.Name == schedule.Name).First();
            }

            


        }



        /// <summary>
        /// Creates a new schedule based on an existing schedule
        /// </summary>
        public void CreateNewSchedule(Model.AlarmSchedule schedule)
        {
            var list = this.AlarmScheduleCollection.Where(ap => ap.Name == schedule.Name);

            if (list.Count() > 0)
            {
                return;
            }
            else
            {

                if (schedule.Name == null) return;

                if (schedule.Name.Length > 0)
                {
                    AlarmScheduleRepository.Save(schedule);
                }

                this.AlarmScheduleCollection = new ObservableCollection<Model.AlarmSchedule>((new AlarmScheduleRepository()).GetAlarmSchedules());
                this.SelectedSchedule = this.AlarmScheduleCollection.Where(p => p.Name == schedule.Name).First();
            }




        }

        /// <summary>
        /// Updates the current selected schedule with the current selected alarms
        /// </summary>
        public async void UpdateCurrentSchedule()
        {
            if (this.SelectedSchedule == null) return;

            string scheduleName = this.SelectedSchedule.Name;
            this.AlarmScheduleCollection.Where(p => p.Name == this.SelectedSchedule.Name).First().Alarms = new List<Model.AlarmItem>(this.AlarmItemCollection.ToList());
            AlarmScheduleRepository.Save(this.SelectedSchedule);
            this.AlarmScheduleCollection = new ObservableCollection<Model.AlarmSchedule>((new AlarmScheduleRepository()).GetAlarmSchedules());
            this.SelectedSchedule = this.AlarmScheduleCollection.Where(s => s.Name == scheduleName).First();

            MetroDialogSettings settings = new MetroDialogSettings();
            settings.AffirmativeButtonText = "Ok";
            settings.AnimateHide = true;
            settings.AnimateShow = true;
            settings.DefaultButtonFocus = MessageDialogResult.Affirmative;
            await MainWindow.View.ShowMessageAsync("Schedule Updated ", string.Format("Schedule \"{0}\" has been updated.", this.SelectedSchedule.Name), MessageDialogStyle.Affirmative, settings);

        }


        /// <summary>
        /// Deletes the current selected schedule
        /// </summary>
        public async void DeleteCurrentSchedule()
        {
            string scheduleName = this.SelectedSchedule.Name;
            AlarmScheduleRepository.Delete(this.SelectedSchedule);
            
            this.AlarmScheduleCollection.Remove(this.AlarmScheduleCollection.Where(schedule => schedule.Name == this.SelectedSchedule.Name).First());
            this.SelectedSchedule = null;
            this.RemoveAllAlarms();


            MetroDialogSettings settings = new MetroDialogSettings();
            settings.AffirmativeButtonText = "Ok";
            settings.AnimateHide = true;
            settings.AnimateShow = true;
            settings.DefaultButtonFocus = MessageDialogResult.Affirmative;
            await MainWindow.View.ShowMessageAsync("Schedule Deleted ", string.Format("Schedule \"{0}\" has been deleted.", scheduleName), MessageDialogStyle.Affirmative, settings);

        }

        /// <summary>
        /// Refreshes the SetAlarmsViewSource View
        /// </summary>
        public void RefreshScheduleView()
        {
            ((CollectionViewSource)AlarmsView.View.FindResource("SetAlarmsViewSource")).View.Refresh();
        }


        /// <summary>
        /// Sets all alarms IsSet property to false
        /// </summary>
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
