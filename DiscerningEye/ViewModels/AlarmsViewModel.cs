/* ===================================================================
 License:
    DiscerningEye - FFXIV Gathering Companion App
    AlarmsViewModels.cs


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
using DiscerningEye.Events;
using DiscerningEye.Views;
using MahApps.Metro.Controls.Dialogs;
using NAudio.Wave;
using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Windows.Data;
using DiscerningEye.AlarmController;
using DiscerningEye.Models;
using DiscerningEye.Models.AlarmItem;

namespace DiscerningEye.ViewModels
{
    public class AlarmsViewModel : ViewModelBase
    {
        private IEventAggregator _eventAggregator;


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
                if (value == "") this.SearchAlarmsCommand.Execute();
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

        private bool _graphicViewVisible;
        public bool GraphicViewVisible
        {
            get { return _graphicViewVisible; }
            set { SetProperty(ref _graphicViewVisible, value); }
        }

        private bool _listViewVisible;
        public bool ListViewVisible
        {
            get { return _listViewVisible; }
            set { SetProperty(ref _listViewVisible, value); }
        }


        private AlarmItem _selectedAlarmItem;
        public AlarmItem SelectedAlarmItem
        {
            get { return _selectedAlarmItem; }
            set
            {
                SetProperty(ref _selectedAlarmItem, value);
                //this.SetMap();
            }
        }




        private CollectionView _botanistView;
        public CollectionView BotanistView
        {
            get { return _botanistView; }
            set { SetProperty(ref _botanistView, value); }
        }

        private CollectionView _minerView;
        public CollectionView MinerView
        {
            get { return _minerView; }
            set { SetProperty(ref _minerView, value); }
        }

        private CollectionView _upComingView;
        public CollectionView UpComingView
        {
            get { return _upComingView; }
            set { SetProperty(ref _upComingView, value); }
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
        //public DelegateCommand AlarmItemChecked { get; private set; }

        /// <summary>
        /// Gets or sets the DelegateCommand used to remove all alarms from the schedule alarms view
        /// </summary>
        public DelegateCommand RemoveAllAlarmsCommand { get; private set; }

        //public DelegateCommand ShowGraphicViewCommand { get; private set; }
        //public DelegateCommand ShowListViewCommand { get; private set; }

        //public DelegateCommand GroupBoxDoubleClick { get; private set; }

        public DelegateCommand<AlarmItem> AlarmItemChecked { get; set; }


        //=========================================================
        //  Constructor
        //=========================================================
        /// <summary>
        /// Creates a new instance of AlarmsViewModel
        /// </summary>
        public AlarmsViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            //  Setup Commands
            this.CreateNewScheduleCommand = new DelegateCommand(this.CreateNewSchedule, () => true);






            this.LoadScheduleCommand = new DelegateCommand(this.LoadSchedule, () => this.CanAdjustSelectedSchedule).ObservesProperty(() => this.SelectedSchedule);
            this.UpdateScheduleCommand = new DelegateCommand(this.UpdateCurrentSchedule, () => this.CanAdjustSelectedSchedule).ObservesProperty(() => this.SelectedSchedule);
            this.DeleteCurrentScheduleCommand = new DelegateCommand(this.DeleteCurrentSchedule, () => this.CanAdjustSelectedSchedule).ObservesProperty(() => this.SelectedSchedule);

            this.SearchAlarmsCommand = new DelegateCommand(this.SearchAlarms, () => !string.IsNullOrEmpty(this.SearchText) && !string.IsNullOrWhiteSpace(this.SearchText)).ObservesProperty(() => this.SearchText);
            this.AlarmItemChecked = new DelegateCommand<AlarmItem>(this.OnAlarmItemChecked);
            //this.AlarmItemChecked = new DelegateCommand(() =>
            //{
            //    _eventAggregator.GetEvent<AddAlarmEvent>().Publish(this.SelectedAlarmItem);
            //    this.RefreshScheduleView();

            //},() => true);
            this.RemoveAllAlarmsCommand = new DelegateCommand(this.RemoveAllAlarms, () => true);

            //this.ShowGraphicViewCommand = new DelegateCommand(() => 
            //{
            //    this.GraphicViewVisible = true;
            //    this.ListViewVisible = false;
            //}, () => true);
            //this.ShowListViewCommand = new DelegateCommand(() => 
            //{
            //    this.ListViewVisible = true;
            //    this.GraphicViewVisible = false;
            //}, () => true);



            //this.GroupBoxDoubleClick = new DelegateCommand(() =>
            //{
            //    this.SelectedAlarmItem.IsSet = !this.SelectedAlarmItem.IsSet;
            //    this.AlarmItemChecked.Execute();
            //}, () => true);


            //  Initilize which view type we use by default
            this.GraphicViewVisible = true;
            this.ListViewVisible = false;

            AlarmController.Controller.Master.PropertyChanged += Master_PropertyChanged;
            if(AlarmController.Controller.Master.AlarmItemCollection != null)
            {
                //this.BotanistView = (CollectionView)CollectionViewSource.GetDefaultView(AlarmController.Controller.Master.AlarmItemCollection);
                //this.MinerView = (CollectionView)CollectionViewSource.GetDefaultView(AlarmController.Controller.Master.AlarmItemCollection);
                this.BotanistView = (CollectionView)new CollectionViewSource { Source = AlarmController.Controller.Master.AlarmItemCollection }.View;
                this.MinerView = (CollectionView)new CollectionViewSource { Source = AlarmController.Controller.Master.AlarmItemCollection }.View;
                this.BotanistView.Filter = OnBotanistViewFilter;                
                this.MinerView.Filter = OnMinerViewFilter;


            }



        }


        private void OnAlarmItemChecked(AlarmItem item)
        {
            AlarmController.Controller.Master.SetAlarmItem(item.Name, item.StartTime);
        }

        private bool OnBotanistViewFilter(object item)
        {
            var alarmItem = (AlarmItem)item;

            if (!string.IsNullOrEmpty(this.SearchText))
            {
                return alarmItem.Job == "Botanist" && alarmItem.Name.ToLower().Contains(this.SearchText.ToLower());
            }
            else
            {
                return alarmItem.Job == "Botanist";
            }
        }


        private bool OnMinerViewFilter(object item)
        {
            var alarmItem = (AlarmItem)item;

            if (!string.IsNullOrEmpty(this.SearchText))
            {
                return alarmItem.Job == "Miner" && alarmItem.Name.ToLower().Contains(this.SearchText.ToLower());
            }
            else
            {
                return alarmItem.Job == "Miner";
            }
        }

        private void Master_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {

        }


        //=========================================================
        //  Events
        //=========================================================



        //=========================================================
        //  Interface Implementations
        //=========================================================
        #region IDisposeable Implementation

        protected override void OnDispose()
        {
 
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
            this.BotanistView.Refresh();
            this.MinerView.Refresh();
            ////CollectionViewSource alarmsViewSource = ((CollectionViewSource)Alarms.View.FindResource("BotanistViewSource"));
            ////if (alarmsViewSource.View != null) alarmsViewSource.View.Refresh();
        }

        /// <summary>
        /// Loads the Selecte Schedule
        /// </summary>
        public async void LoadSchedule()
        {
            foreach(AlarmItem scheduleAlarm in this.SelectedSchedule.Alarms)
            {
                //foreach(Models.AlarmItem collectionAlarm in this.AlarmItemCollection)
                foreach (AlarmItem collectionAlarm in Controller.Master.AlarmItemCollection)
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
        /// Refreshes the SetAlarmsViewSource View
        /// </summary>
        public void RefreshScheduleView()
        {
            //((CollectionViewSource)AlarmsView.View.FindResource("SetAlarmsViewSource")).View.Refresh();
        }


        /// <summary>
        /// Sets all alarms IsSet property to false
        /// </summary>
        public void RemoveAllAlarms()
        {            
            foreach (AlarmItem alarmItem in Controller.Master.AlarmItemCollection)
            {
                alarmItem.IsSet = false;
            }
            this.RefreshScheduleView();
        }
    }
}
