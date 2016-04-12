using DiscerningEye.Events;
using DiscerningEye.Models.AlarmItem;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscerningEye.ViewModels
{
    public class SchedulesViewModel : ViewModelBase
    {

        private System.Timers.Timer _updatTimer;

        private ObservableCollection<AlarmItem> _scheduledAlarms;
        public ObservableCollection<AlarmItem> ScheduledAlarms
        {
            get { return _scheduledAlarms; }
            set { SetProperty(ref _scheduledAlarms, value); }
        }

        public SchedulesViewModel(IEventAggregator eventAggregator)
        {
            eventAggregator.GetEvent<AddAlarmEvent>().Subscribe(AddAlarm);
            this._updatTimer = new System.Timers.Timer();
            this._updatTimer.Interval = 1000;
            this._updatTimer.Elapsed += _updatTimer_Elapsed;
            this._updatTimer.Start();
        }

        private void _updatTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            this._updatTimer.Stop();

            //  Calculate the next spawn time of the scheduled alarms




            this._updatTimer.Start();
        }

        private void AddAlarm(AlarmItem alarmItem)
        {
            if (!this.ScheduledAlarms.Any(a => a.Name == alarmItem.Name && a.StartTime == alarmItem.StartTime))
                this.ScheduledAlarms.Add(alarmItem);
        }


        private void RemovedAlarm(AlarmItem alarmItem)
        {
            if(this.ScheduledAlarms.Any(a => a.Name == alarmItem.Name && a.StartTime == alarmItem.StartTime))
            {
                this.ScheduledAlarms.Remove(this.ScheduledAlarms.First(a => a.Name == alarmItem.Name && a.StartTime == alarmItem.StartTime));
            }
        }








    }
}
