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

namespace DiscerningEye.ViewModel
{
    public class AlarmsViewModel : ViewModelBase
    {
        //=========================================================
        //  Private Fields
        //=========================================================
        private AlarmItemRepository _alarmItemRepository;
        private ObservableCollection<Model.AlarmItem> _alarmItemCollection;
        private ICollectionView _minerViewSource;
        private ICollectionView _botanyViewSource;
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
            get { return this._alarmItemCollection; }
            set
            {
                if (this._alarmItemCollection == value) return;
                this._alarmItemCollection = value;
                OnPropertyChanged("AlarmItemCollection");
            }
        }

        //public CollectionViewSource MinerViewSource
        //{
        //    get { return this._minerViewSource; }
        //    set
        //    {
        //        if (this._minerViewSource == value) return;
        //        this._minerViewSource = value;
        //        OnPropertyChanged("MinerViewSource");
        //    }
        //}

        public ICollectionView MinerViewSource
        {
            get { return this._minerViewSource; }
            set
            {
                if (this._minerViewSource == value) return;
                this._minerViewSource = value;
                OnPropertyChanged("MinerViewSource");
            }
        }

        public ICollectionView BotanyViewSource
        {
            get { return this._botanyViewSource; }
            set
            {
                if (this._botanyViewSource == value) return;
                this._botanyViewSource = value;
                OnPropertyChanged("BotanyViewSource");
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
                this.FilterView(value);
                OnPropertyChanged("SearchText");
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
       

        //=========================================================
        //  Constructor
        //=========================================================
        /// <summary>
        /// Creates a new instance of AlarmsViewModel
        /// </summary>
        public AlarmsViewModel()
        {
            AlarmsView.View.Loaded += View_Loaded;
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


            //  Initilize the AlarmItemCollection
            this.AlarmItemCollection = new ObservableCollection<Model.AlarmItem>(_alarmItemRepository.GetAlarmItems());

            MinerViewSource = CollectionViewSource.GetDefaultView(this.AlarmItemCollection);
            MinerViewSource.Filter = new Predicate<object>(x =>
                                                               ((Model.AlarmItem)x).Job.ToLower().Contains("min"));


            BotanyViewSource = CollectionViewSource.GetDefaultView(this.AlarmItemCollection);
            BotanyViewSource.Filter = new Predicate<object>(x =>
                                                               ((Model.AlarmItem)x).Job.ToLower().Contains("bot"));




            //((CollectionViewSource)AlarmsView.View.FindResource("MinerViewSource")).Source = this.AlarmItemCollection;
            //((CollectionViewSource)AlarmsView.View.FindResource("MinerViewSource")).Filter += MinerViewModel_Filter;

            //((CollectionViewSource)AlarmsView.View.FindResource("BotanyViewSource")).Source = this.AlarmItemCollection;
            //((CollectionViewSource)AlarmsView.View.FindResource("BotanyViewSource")).Filter += BotanyViewModel_Filter;






            //////ICollectionView collectionView = CollectionViewSource.GetDefaultView(this.AlarmItemCollection);
            //////collectionView.SortDescriptions.Add(new SortDescription("NextSpawn.Hours", ListSortDirection.Ascending));
            //////collectionView.SortDescriptions.Add(new SortDescription("NextSpawn.Minutes", ListSortDirection.Ascending));
            //////collectionView.SortDescriptions.Add(new SortDescription("NextSpawn.Seconds", ListSortDirection.Ascending));
            ////////collectionView.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
            //////var view = (ICollectionViewLiveShaping)CollectionViewSource.GetDefaultView(this.AlarmItemCollection);
            //////view.IsLiveSorting = true;


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

        private void BotanyViewModel_Filter(object sender, FilterEventArgs e)
        {
            Model.AlarmItem i = e.Item as Model.AlarmItem;
            if (i != null)
            {
                if (this.SearchText == null) this.SearchText = "";
                e.Accepted = i.Job.ToLower().Contains("bot") && i.Name.ToLower().Contains(this.SearchText);
            }
        }

        private void MinerViewModel_Filter(object sender, FilterEventArgs e)
        {
            Model.AlarmItem i = e.Item as Model.AlarmItem;
            if(i != null)
            {
                e.Accepted = i.Job.ToLower().Contains("min");
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
                long earthTicks =nextEorzeaSpawn.Ticks / (long)Utilities.ClockController.EORZEA_MULTIPLIER;
                alarmInfo.NextSpawn = new TimeSpan(earthTicks);

                //  Check if the NexSpawn time is 0h 0m and 0s.  If so, and fullZeroExists is false
                //  set fullZeroExists to true
                //if (alarmInfo.NextSpawn == new TimeSpan(0, 0, 0) && fullZeroExists == false)
                //{
                //    fullZeroExists = true;
                //}
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

                        //MainWindow.View.notificationIcon.ShowBalloonTip("Discerning Eye: Early Warning", earlyWarningMessage, Hardcodet.Wpf.TaskbarNotification.BalloonIcon.Info);



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
                //alarmItem.NextSpawn = alarmInfo.NextSpawn.ToString(@"d\huh\:h\h\:m\m\:s\s", System.Globalization.CultureInfo.InvariantCulture);
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

                if(Properties.Settings.Default.EnableNotificationTone)
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
        private void FilterView(string filterText)
        {
            //if (view.CanFilter)
            //{
            //    view.Filter = item =>
            //    {
            //        Model.AlarmItem vItem = item as Model.AlarmItem;
            //        if (vItem == null) return false;
            //        return vItem.Name.ToLower().Contains(this.SearchText.ToLower());
            //    };
            //}
        }

        public void CreateNewProfile()
        {

        }

        public void UpdateCurrentProfile()
        {

        }

        public void DeleteCurrentProfile()
        {

        }
    }
}
