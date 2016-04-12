using DiscerningEye.AlarmController;
using DiscerningEye.DataAccess;
using DiscerningEye.Events;
using DiscerningEye.Models.AlarmItem;
using DiscerningEye.Views;
using MahApps.Metro.Controls.Dialogs;
using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace DiscerningEye.ViewModels
{
    public class SchedulesViewModel : ViewModelBase
    {
        //=========================================================
        //  Properties (some with backing private fields)
        //=========================================================
        /// <summary>
        /// Gets or Sets the Boolean value used to determine if a schedule can be adjusted
        /// </summary>
        /// <remarks>
        /// This is used by the various ICommands for the CanExecute for the buttons on the ScheculedView that deal
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


        private Models.AlarmSchedule _selectedSchedule;
        /// <summary>
        /// Gets or sets the current selected Models.AlarmSchedule
        /// </summary>
        /// <remarks>
        /// This is bound to the SelectedValue property of the combobox on the AlarmView
        /// </remarks>
        public Models.AlarmSchedule SelectedSchedule
        {
            get { return this._selectedSchedule; }
            set { SetProperty(ref _selectedSchedule, value); }
        }


        //=========================================================
        //  ICommands
        //=========================================================
        /// <summary>
        /// Gets or sets the DelegateCommand used when an alarm item is checked
        /// </summary>
        public DelegateCommand<AlarmItem> AlarmItemChecked { get; private set; }


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
        /// Gets or sets the DelegateCommand used to delete the current selected schedule
        /// </summary>
        public DelegateCommand DeleteCurrentScheduleCommand { get; private set; }

        /// <summary>
        /// Gets or sets the DelegateCommand used to remove all alarms from the schedule alarms view
        /// </summary>
        public DelegateCommand RemoveAllAlarmsCommand { get; private set; }



        //=========================================================
        //  Constructor
        //=========================================================
        public SchedulesViewModel()
        {
            this.AlarmItemChecked = new DelegateCommand<AlarmItem>(this.OnAlarmItemChecked);
            this.CreateNewScheduleCommand = new DelegateCommand(this.CreateNewSchedule, () => true);
            this.LoadScheduleCommand = new DelegateCommand(this.LoadSchedule, () => this.CanAdjustSelectedSchedule).ObservesProperty(() => this.SelectedSchedule);
            this.UpdateScheduleCommand = new DelegateCommand(this.UpdateCurrentSchedule, () => this.CanAdjustSelectedSchedule).ObservesProperty(() => this.SelectedSchedule);
            this.DeleteCurrentScheduleCommand = new DelegateCommand(this.DeleteCurrentSchedule, () => this.CanAdjustSelectedSchedule).ObservesProperty(() => this.SelectedSchedule);
            this.RemoveAllAlarmsCommand = new DelegateCommand(this.RemoveAllAlarms, () => true);
        }





        //=========================================================
        //  Events
        //=========================================================
        /// <summary>
        /// Occurs when an alarm item on either the Miner or Botany data grid is checked
        /// </summary>
        /// <param name="item"></param>
        private void OnAlarmItemChecked(AlarmItem alarmItem)
        {
            AlarmController.Controller.Master.SetAlarmItem(alarmItem.Name, alarmItem.StartTime);
            CollectionViewSource scheduledAlarmsViewSource = ((CollectionViewSource)Schedules.View.FindResource("ScheduledAlarmsViewSource"));
            if (scheduledAlarmsViewSource.View != null) scheduledAlarmsViewSource.View.Refresh();
        }


        //=========================================================
        //  Methods
        //=========================================================
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

            var list = Controller.Master.AlarmScheduleCollection.Where(ap => ap.Name == scheduleName);

            if (list.Count() > 0)
            {
                settings.AffirmativeButtonText = "Ok";
                await MainWindow.View.ShowMessageAsync("Create Schedule Error", "Cannot create a sachedule with the same name as an existing one", MessageDialogStyle.Affirmative, settings);

            }
            else
            {

                if (scheduleName == null) return;
                Models.AlarmSchedule schedule = new Models.AlarmSchedule(Controller.Master.AlarmItemCollection.ToList());

                if (scheduleName.Length > 0)
                {
                    schedule.Name = scheduleName;
                    AlarmScheduleRepository.Save(schedule);
                }

                Controller.Master.AlarmScheduleCollection.Add(schedule);

                settings.AffirmativeButtonText = "Ok";
                await MainWindow.View.ShowMessageAsync("Schedule Created", string.Format("Schedule has been created and saved for {0}", scheduleName), MessageDialogStyle.Affirmative, settings);
                Controller.Master.AlarmScheduleCollection = new ObservableCollection<Models.AlarmSchedule>((new AlarmScheduleRepository()).GetAlarmSchedules());

                this.SelectedSchedule = Controller.Master.AlarmScheduleCollection.Where(p => p.Name == schedule.Name).First();
            }




        }


        /// <summary>
        /// Creates a new schedule based on an existing schedule
        /// </summary>
        public void CreateNewSchedule(Models.AlarmSchedule schedule)
        {
            var list = Controller.Master.AlarmScheduleCollection.Where(ap => ap.Name == schedule.Name);

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

                Controller.Master.AlarmScheduleCollection = new ObservableCollection<Models.AlarmSchedule>((new AlarmScheduleRepository()).GetAlarmSchedules());

                this.SelectedSchedule = Controller.Master.AlarmScheduleCollection.Where(p => p.Name == schedule.Name).First();
            }




        }



        /// <summary>
        /// Updates the current selected schedule with the current selected alarms
        /// </summary>
        public async void UpdateCurrentSchedule()
        {
            if (this.SelectedSchedule == null) return;

            string scheduleName = this.SelectedSchedule.Name;
            Controller.Master.AlarmScheduleCollection.Where(p => p.Name == this.SelectedSchedule.Name).First().Alarms = new List<AlarmItem>(Controller.Master.AlarmItemCollection.ToList());

            AlarmScheduleRepository.Save(this.SelectedSchedule);


            Controller.Master.AlarmScheduleCollection = new ObservableCollection<Models.AlarmSchedule>((new AlarmScheduleRepository()).GetAlarmSchedules());

            this.SelectedSchedule = Controller.Master.AlarmScheduleCollection.Where(s => s.Name == scheduleName).First();

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

            Controller.Master.AlarmScheduleCollection.Remove(Controller.Master.AlarmScheduleCollection.Where(schedule => schedule.Name == this.SelectedSchedule.Name).First());

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
        /// Loads the Selecte Schedule
        /// </summary>
        public async void LoadSchedule()
        {
            //  First Remove all alarms
            this.RemoveAllAlarms();


            foreach (AlarmItem scheduleAlarm in this.SelectedSchedule.Alarms)
            {
                //foreach(Models.AlarmItem collectionAlarm in this.AlarmItemCollection)
                foreach (AlarmItem collectionAlarm in Controller.Master.AlarmItemCollection)
                {
                    if (scheduleAlarm.Name == collectionAlarm.Name && scheduleAlarm.StartTime == collectionAlarm.StartTime && scheduleAlarm.IsSet)
                    {
                        collectionAlarm.EarlyWarningIssued = false;
                        collectionAlarm.Armed = true;
                        OnAlarmItemChecked(collectionAlarm);
                            //collectionAlarm.IsSet = scheduleAlarm.IsSet;
                    }
                }
            }

            //this.RefreshScheduleView();


            MetroDialogSettings settings = new MetroDialogSettings();
            settings.AffirmativeButtonText = "Ok";
            settings.AnimateHide = true;
            settings.AnimateShow = true;
            settings.DefaultButtonFocus = MessageDialogResult.Affirmative;
            await MainWindow.View.ShowMessageAsync("Schedule Loaded ", string.Format("Schedule \"{0}\" has been loaded.", this.SelectedSchedule.Name), MessageDialogStyle.Affirmative, settings);
        }


        /// <summary>
        /// Removes all alarms from the schedule
        /// </summary>
        public void RemoveAllAlarms()
        {
            foreach (AlarmItem alarmItem in Controller.Master.AlarmItemCollection)
            {
                if (alarmItem.IsSet)
                    this.OnAlarmItemChecked(alarmItem);
            }
            //this.RefreshScheduleView();
        }
    }
}
