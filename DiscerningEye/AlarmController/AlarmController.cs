using DiscerningEye.DataAccess;
using DiscerningEye.Models;
using DiscerningEye.Models.AlarmItem;
using DiscerningEye.Utilities;
using DiscerningEye.Views;
using MahApps.Metro.Controls.Dialogs;
using NAudio.Wave;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Data;

namespace DiscerningEye.AlarmController
{
    public class Controller : INotifyPropertyChanged, IDisposable
    {

        private AudioFileReader _audioFileReader;
        private SpeechSynthesizer _synth;
        private IWavePlayer _waveOutDevice;

        public static Controller Master;

        private Timer _updateTimer;


        private ObservableCollection<AlarmItem> _alarmCollection;
        public ObservableCollection<AlarmItem> AlarmItemCollection
        {
            get { return this._alarmCollection; }
            set
            {
                if (this._alarmCollection == value) return;
                this._alarmCollection = value;
                NotifyPropertyChanged();
            }
        }

        private ObservableCollection<AlarmSchedule> _alarmScheduleCollection;
        public ObservableCollection<AlarmSchedule> AlarmScheduleCollection
        {
            get { return _alarmScheduleCollection; }
            set
            {
                if (this._alarmScheduleCollection == value) return;
                _alarmScheduleCollection = value;
                NotifyPropertyChanged();
            }
        }

        private ObservableCollection<AlarmItem> _upComingAlarms;
        public ObservableCollection<AlarmItem> UpComingAlarms
        {
            get { return this._upComingAlarms; }
            set
            {
                if (this._upComingAlarms == value) return;
                this._upComingAlarms = value;
                NotifyPropertyChanged();
            }
        }



        private ClockController _eorzeaClock;
        public ClockController EorzeaClock
        {
            get { return _eorzeaClock; }
            set
            {
                if (this._eorzeaClock == value) return;
                _eorzeaClock = value;
                NotifyPropertyChanged();
            }
        }




        public Controller()
        {
            this._updateTimer = new Timer();
            this._updateTimer.Interval = 100;
            this._updateTimer.AutoReset = false;
            this._updateTimer.Elapsed += _updateTimer_Elapsed;


            //  Initilize the AlarmItemCollection
            this.AlarmItemCollection = new ObservableCollection<AlarmItem>(new AlarmItemRepository().GetAlarmItems());
            this.AlarmScheduleCollection = new ObservableCollection<Models.AlarmSchedule>((new AlarmScheduleRepository()).GetAlarmSchedules());

            //  Initilize the Eorzea Clock
            this.EorzeaClock = new ClockController();


            //  Initilize the sound player
            _waveOutDevice = new WaveOut();

            //  Initilize the text-to-speech object
            _synth = new SpeechSynthesizer();

            Controller.Master = this;
            


            this._updateTimer.Start();
        }



        private void _updateTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            //  Get the current eorzea time span
            TimeSpan currentEorzeaTimeSpan = this.EorzeaClock.GetEorzeaTimeSpan();

            List<string> earlyWarningMessages = new List<string>();
            List<string> itemAvailableMessages = new List<string>();
            StringBuilder notificationMessage = new StringBuilder();

            //  Go through each of the alarm items
            foreach (AlarmItem alarmItem in this.AlarmItemCollection)
            {
                //  Because we use the pretrigger of the alarms, we need a way to rearm the alarm
                //  without it triggering over and over until the hour has passed.  So we will
                //  check to see if the current eorzea time is > than the alarm time and if so
                //  we just arm it
                #region CheckIfReArm
                TimeSpan startTime;
                TimeSpan.TryParse(alarmItem.StartTime, out startTime);
                if (currentEorzeaTimeSpan > startTime)
                {
                    //  becuase we use 24 hour time, zero hour alarms will rearm on any hour
                    //  meaning they will continuously rearm immiedialy after alarming unless
                    //  we perform a hacky check
                    if ((startTime.Hours == 0 && currentEorzeaTimeSpan.Hours < 2) || startTime.Hours > 0)
                    {
                        alarmItem.Armed = true;
                        alarmItem.EarlyWarningIssued = false;
                    }
                }
                #endregion CheckIfReArm



                #region CalculateTimeTillSpawn
                if (alarmItem.IsSet || !alarmItem.IsSet)
                {
                    //  Get the time difference between the alarm time and eorzea time
                    TimeSpan timeDiff;
                    TimeSpan nextEorzeaSpawn;
                    if (startTime.Equals(new TimeSpan(0, 0, 0)))
                    {
                        timeDiff = (new TimeSpan(24, 0, 0)).Subtract(currentEorzeaTimeSpan);
                    }
                    else
                    {
                        timeDiff = startTime.Subtract(currentEorzeaTimeSpan);
                    }


                    if (startTime > currentEorzeaTimeSpan)
                    {                      
                        nextEorzeaSpawn = startTime.Subtract(currentEorzeaTimeSpan);
                    }
                    else
                    {
                        nextEorzeaSpawn = ((TimeSpan)new TimeSpan(23, 59, 59)).Subtract(currentEorzeaTimeSpan.Subtract(startTime));
                    }
                    long earthTicks = nextEorzeaSpawn.Ticks / (long)ClockController.EORZEA_MULTIPLIER;
                    alarmItem.NextSpawn = new TimeSpan(earthTicks);

                }

                #endregion CalculateTimeTillSpawn


                #region Available Message and Coming Up Colors
                //  Setup message
                alarmItem.AvaliableMessage = string.Format("{0:00}:{1:00}:{2:00}", alarmItem.NextSpawn.Hours, alarmItem.NextSpawn.Minutes, alarmItem.NextSpawn.Seconds);

                //  Setup Color
                if(alarmItem.NextSpawn >= new TimeSpan(0,5,0))
                {
                    alarmItem.MessageForegroundColor = System.Windows.Media.Brushes.Transparent;
                }
                else if (alarmItem.NextSpawn >= new TimeSpan(0,1,0) && alarmItem.NextSpawn < new TimeSpan(0,5,0))
                {
                    alarmItem.MessageForegroundColor = System.Windows.Media.Brushes.DarkGoldenrod;
                }
                else
                {
                    alarmItem.MessageForegroundColor = System.Windows.Media.Brushes.DarkSeaGreen;
                }

                #endregion Available Message and Coming Up Colors





                //  Check if we should issue an early warning
                //if (Properties.Settings.Default.EnableEarlyWarning && alarmItem.IsSet)
                if (UserSettingsRepository.Settings.EnableEarlyWarning && alarmItem.IsSet)
                {
                    //  Check if the alarm should issue an early warning
                    //if (alarmItem.NextSpawn.TotalMinutes <= new TimeSpan(0, Properties.Settings.Default.EarlyWarningMinutes, Properties.Settings.Default.EarlyWarningSeconds).TotalMinutes && !alarmItem.EarlyWarningIssued)
                    if (alarmItem.NextSpawn.TotalMinutes <= new TimeSpan(0, UserSettingsRepository.Settings.EarlyWarningMinutes, UserSettingsRepository.Settings.EarlyWarningSeconds).TotalMinutes && !alarmItem.EarlyWarningIssued)
                    {
                        alarmItem.EarlyWarningIssued = true;

                        //string earlyWarningMessage = Properties.Settings.Default.EarlyWarningMessage;
                        string earlyWarningMessage = UserSettingsRepository.Settings.EarlyWarningMessage;
                        earlyWarningMessage = System.Text.RegularExpressions.Regex.Replace(earlyWarningMessage, "{item}", alarmItem.Name, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                        earlyWarningMessage = System.Text.RegularExpressions.Regex.Replace(earlyWarningMessage, "{region}", alarmItem.Region, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                        earlyWarningMessage = System.Text.RegularExpressions.Regex.Replace(earlyWarningMessage, "{zone}", alarmItem.Zone, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                        string timeMessage = "";

                        if (alarmItem.NextSpawn.Hours != 0)
                        {
                            if (alarmItem.NextSpawn.Hours == 1)
                                timeMessage += "1 hour";
                            else
                                timeMessage += string.Format("{0} hours", alarmItem.NextSpawn.Hours);
                        }

                        if (alarmItem.NextSpawn.Minutes != 0)
                        {
                            if (alarmItem.NextSpawn.Minutes == 1)
                                timeMessage += " 1 minute";
                            else
                                timeMessage += string.Format(" {0} minutes", alarmItem.NextSpawn.Minutes);
                        }

                        if (alarmItem.NextSpawn.Seconds != 0)
                        {
                            if (alarmItem.NextSpawn.Seconds == 1)
                                timeMessage += " 1 second";
                            else
                                timeMessage += string.Format(" {0} seconds", alarmItem.NextSpawn.Seconds);
                        }


                        earlyWarningMessage = System.Text.RegularExpressions.Regex.Replace(earlyWarningMessage, "{time}", timeMessage, System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                        earlyWarningMessages.Add(earlyWarningMessage);



                    }
                }




                //  Check if alarm should be triggered
                if (alarmItem.NextSpawn <= new TimeSpan(0, 0, 2) && alarmItem.Armed && alarmItem.IsSet)
                {
                    alarmItem.Armed = false;

                    //string spawnMessage = Properties.Settings.Default.ItemAvailableMessage;
                    string spawnMessage = UserSettingsRepository.Settings.ItemAvailableMessage;
                    spawnMessage = System.Text.RegularExpressions.Regex.Replace(spawnMessage, "{item}", alarmItem.Name, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                    spawnMessage = System.Text.RegularExpressions.Regex.Replace(spawnMessage, "{region}", alarmItem.Region, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                    spawnMessage = System.Text.RegularExpressions.Regex.Replace(spawnMessage, "{zone}", alarmItem.Zone, System.Text.RegularExpressions.RegexOptions.IgnoreCase);


                    itemAvailableMessages.Add(spawnMessage);

                }
            }

            //if (Properties.Settings.Default.DoNotDisturb == false)
            if (UserSettingsRepository.Settings.DoNotDisturb == false)
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
                    //if (Properties.Settings.Default.EnableBallonTip)
                    if (UserSettingsRepository.Settings.EnableBallonTip)
                        MainWindow.View.notificationIcon.ShowBalloonTip("Discerning Eye: Notificaitons", notificationMessage.ToString(), Hardcodet.Wpf.TaskbarNotification.BalloonIcon.Info);

                    //if (Properties.Settings.Default.EnableTextToSpeech)
                    if (UserSettingsRepository.Settings.EnableTextToSpeech)
                    {
                        if (_synth.State == SynthesizerState.Ready)
                        {
                            //_synth.Volume = Properties.Settings.Default.TextToSpeechVolume;
                            _synth.Volume = UserSettingsRepository.Settings.TextToSpeechVolume;
                            _synth.SpeakAsync(notificationMessage.ToString());
                        }
                    }

                    //if (Properties.Settings.Default.EnableNotificationTone && File.Exists(Properties.Settings.Default.NotificationToneUri))
                    if (UserSettingsRepository.Settings.EnableNotificationTone && File.Exists(UserSettingsRepository.Settings.NotificationToneUri))
                    {
                        if (_waveOutDevice.PlaybackState != PlaybackState.Playing)
                        {

                            //_audioFileReader = new AudioFileReader(Properties.Settings.Default.NotificationToneUri);
                            _audioFileReader = new AudioFileReader(UserSettingsRepository.Settings.NotificationToneUri);
                            //_audioFileReader.Volume = (float)Properties.Settings.Default.NotificationToneVolume / 100.0f;
                            _audioFileReader.Volume = (float)UserSettingsRepository.Settings.NotificationToneVolume / 100.0f;
                            _waveOutDevice.Init(_audioFileReader);
                            _waveOutDevice.Play();
                        }
                    }
                }

            }
            this._updateTimer.Start();
        }

        #region INotifyPropertyChanged Implementation
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion INotifyPropertyChanged Implementation

        #region IDisposible Implementation

        public void Dispose()
        {
            this.OnDispose();
        }

        protected virtual void OnDispose()
        {
            if(this._updateTimer != null)
            {
                this._updateTimer.Stop();
                this._updateTimer.Elapsed -= _updateTimer_Elapsed;
                this._updateTimer.Dispose();
                this._updateTimer = null;

            }
        }

        #endregion IDisposible Implementation



        public void SetAlarmItem(string name, string startTime)
        {
            var alarmItem = this.AlarmItemCollection.Where(ai => ai.Name == name && ai.StartTime == startTime).First();
            alarmItem.IsSet = !alarmItem.IsSet;
            if(Schedules.View != null)
                ((CollectionViewSource)Schedules.View.FindResource("ScheduledAlarmsViewSource")).View.Refresh();
        }







    }
}
