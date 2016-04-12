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
        //=========================================================
        //  Properties (some with backing private fields)
        //=========================================================
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

        private AlarmItem _selectedAlarmItem;
        /// <summary>
        /// Gets or sets the ALarmItem objec that is currently selected on either the Miner or the Botany data grid
        /// </summary>
        public AlarmItem SelectedAlarmItem
        {
            get { return _selectedAlarmItem; }
            set
            {
                SetProperty(ref _selectedAlarmItem, value);
            }
        }

        private CollectionView _botanistView;
        /// <summary>
        /// Gets or sets the CollectionView used for the view on the Botany data grid
        /// </summary>
        public CollectionView BotanistView
        {
            get { return _botanistView; }
            set { SetProperty(ref _botanistView, value); }
        }

        private CollectionView _minerView;
        /// <summary>
        /// Gets or sets the Collectionview used for the view on the Miner data grid
        /// </summary>
        public CollectionView MinerView
        {
            get { return _minerView; }
            set { SetProperty(ref _minerView, value); }
        }


        //=========================================================
        //  ICommands
        //=========================================================
        /// <summary>
        /// Gets or sets the DelegateCommand used to search the alarms
        /// </summary>
        public DelegateCommand SearchAlarmsCommand { get; private set; }

        /// <summary>
        /// GEts or sets the DelegateCommand used when an alarm item is checked
        /// </summary>
        public DelegateCommand<AlarmItem> AlarmItemChecked { get; set; }


        //=========================================================
        //  Constructor
        //=========================================================
        /// <summary>
        /// Creates a new instance of AlarmsViewModel
        /// </summary>
        public AlarmsViewModel()
        {
            this.SearchAlarmsCommand = new DelegateCommand(this.SearchAlarms, () => !string.IsNullOrEmpty(this.SearchText) && !string.IsNullOrWhiteSpace(this.SearchText)).ObservesProperty(() => this.SearchText);
            this.AlarmItemChecked = new DelegateCommand<AlarmItem>(this.OnAlarmItemChecked);
            
            //  Setup the views based on the collection from the AlarmController
            if(AlarmController.Controller.Master.AlarmItemCollection != null)
            {

                this.BotanistView = (CollectionView)new CollectionViewSource { Source = AlarmController.Controller.Master.AlarmItemCollection }.View;
                this.MinerView = (CollectionView)new CollectionViewSource { Source = AlarmController.Controller.Master.AlarmItemCollection }.View;
                this.BotanistView.Filter = OnBotanistViewFilter;                
                this.MinerView.Filter = OnMinerViewFilter;
            }
        }








        //=========================================================
        //  Events
        //=========================================================
        /// <summary>
        /// Occurs when an alarm item on either the Miner or Botany data grid is checked
        /// </summary>
        /// <param name="item"></param>
        private void OnAlarmItemChecked(AlarmItem item)
        {
            AlarmController.Controller.Master.SetAlarmItem(item.Name, item.StartTime);
        }

        /// <summary>
        /// Occurs when the BotanistView is refreshed. Filters the objects to be botany only
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
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


        /// <summary>
        /// Occurs when the MinerView is refreshed. Filters the objects to be miner only
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
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
        }

    }
}
