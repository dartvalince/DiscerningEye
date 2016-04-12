/* ===================================================================
 License:
    DiscerningEye - FFXIV Gathering Companion App
    AlarmItem.cs


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


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace DiscerningEye.Models.AlarmItem
{
    [DataContract]
    public class AlarmItem : INotifyPropertyChanged
    {
        //=======================================================
        //  private fields
        //=======================================================
        private TimeSpan _nextSpawn;
        private bool _isSet;

        //=======================================================
        //  Properties
        //=======================================================
        /// <summary>
        /// Gets or sets value to determing is alarm is set
        /// </summary>
        [DataMember(EmitDefaultValue = true)]
        public bool IsSet
        {
            get { return this._isSet; }
            set
            {
                if (this._isSet == value) return;
                this._isSet = value;
                if (value) this.Armed = true;
                OnPropertyChanged("IsSet");
            }
        }

        /// <summary>
        /// Get or set value to determine if alarm is armed
        /// </summary>
        [DataMember(EmitDefaultValue = true)]
        public bool Armed { get; set; }

        /// <summary>
        /// Gets or sets value to determine if alarm has issued an early warning
        /// </summary>
        [DataMember(EmitDefaultValue = true)]
        public bool EarlyWarningIssued { get; set; }

        /// <summary>
        /// Gets or sets the icon url of the item
        /// </summary>
        [DataMember(EmitDefaultValue = true)]
        public string Icon { get; set; }

        /// <summary>
        /// Gets of sets the name of the item
        /// </summary>
        [DataMember(EmitDefaultValue = true)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the region the item is located
        /// </summary>
        [DataMember(EmitDefaultValue = true)]
        public string Region { get; set; }

        /// <summary>
        /// Gets or sets the zone the item is located
        /// </summary>
        [DataMember(EmitDefaultValue = true)]
        public string Zone { get; set; }

        /// <summary>
        /// Gets or sets the slot the item is located in in the node
        /// </summary>
        [DataMember(EmitDefaultValue = true)]
        public string Slot { get; set; }

        /// <summary>
        /// Gets or sets the start time for the item
        /// </summary>
        [DataMember(EmitDefaultValue = true)]
        public string StartTime { get; set; }

        /// <summary>
        /// Gets or sets the end time of the item
        /// </summary>
        [DataMember(EmitDefaultValue = true)]
        public string EndTime { get; set; }


        /// <summary>
        /// Gets or sets the type of node the item is located in
        /// </summary>
        [DataMember(EmitDefaultValue = true)]
        public string NodeType { get; set; }

        /// <summary>
        /// Gets or sets the value for if this is a folklore node
        /// </summary>
        [DataMember(EmitDefaultValue = true)]
        public string Folklore { get; set; }

        /// <summary>
        /// Gets or sets the level needed to view the node the item is in
        /// </summary>
        [DataMember(EmitDefaultValue = true)]
        public string Level { get; set; }

        /// <summary>
        /// Gets or sets the star level of the item
        /// </summary>
        [DataMember(EmitDefaultValue = true)]
        public string Stars { get; set; }

        private string _job;
        /// <summary>
        /// Gets or sets the job used to gather the item
        /// </summary>
        [DataMember(EmitDefaultValue = true)]
        public string Job
        {
            get { return this._job; }
            set
            {
                if (this._job == value) return;
                this._job = value;
                OnPropertyChanged("Job");
            }
        }

        /// <summary>
        /// Gets or sets the minimum gathering needed to gather the item
        /// </summary>
        [DataMember(EmitDefaultValue = true)]
        public string GatheringNeeded { get; set; }


        /// <summary>
        /// Gets or sets the minimum perception needed to HQ the item
        /// </summary>
        [DataMember (EmitDefaultValue = true)]
        public string PerceptionNeeded { get; set; }

        /// <summary>
        /// Gets or sets the minimum collect value needed to get scrips
        /// </summary>
        [DataMember (EmitDefaultValue = true)]
        public string CollectMinimum { get; set; }

        /// <summary>
        /// Gets or sets the medium collect value needed to get scrips
        /// </summary>
        [DataMember(EmitDefaultValue = true)]
        public string CollectMedium { get; set; }


        /// <summary>
        /// Gets or sets the maximum collect value needed to get scrips
        /// </summary>
        [DataMember(EmitDefaultValue = true)]
        public string CollectMaximum { get; set; }

        /// <summary>
        /// Gets or sets the minimum blue scrip reward
        /// </summary>
        [DataMember (EmitDefaultValue = true)]
        public string BlueMinimum { get; set; }

        /// <summary>
        /// Gets or sets the medium blue scrip reward
        /// </summary>
        [DataMember(EmitDefaultValue = true)]
        public string BlueMedium { get; set; }
        /// <summary>
        /// Gets or sets the maxium blue scrip reward
        /// </summary>
        [DataMember(EmitDefaultValue = true)]
        public string BlueMaximum { get; set; }

        /// <summary>
        /// Gets or sets the minimum red scrip reward
        /// </summary>
        [DataMember(EmitDefaultValue = true)]
        public string RedMinimum { get; set; }

        /// <summary>
        /// Gets or sets the medium red scrip reward
        /// </summary>
        [DataMember(EmitDefaultValue = true)]
        public string RedMedium { get; set; }

        /// <summary>
        /// Gets or sets the maxiumum red scrip reward
        /// </summary>
        [DataMember(EmitDefaultValue = true)]
        public string RedMaximum { get; set; }

        [DataMember(EmitDefaultValue = true)]
        public XIVDBSharp.ItemRoot XIVDBItemRoot { get; set; }

        [DataMember(EmitDefaultValue = true)]
        public string Id { get; set; }

        [DataMember(EmitDefaultValue = true)]
        public string LocationX { get; set; }

        [DataMember(EmitDefaultValue = true)]
        public string LocationY { get; set; }

        [DataMember(EmitDefaultValue = true)]
        public string LocationZ { get; set; }


        /// <summary>
        /// Gets or sets the next time the item will spawn
        /// </summary>
        public TimeSpan NextSpawn
        {
            get { return this._nextSpawn; }
            set
            {
                if (this._nextSpawn == value) return;
                this._nextSpawn = value;
                OnPropertyChanged("NextSpawn");
            }
        }


        private string _avaliableMessage;

        public string AvaliableMessage
        {
            get { return _avaliableMessage; }
            set
            {
                if (this._avaliableMessage == value) return;
                this._avaliableMessage = value;
                OnPropertyChanged("AvaliableMessage");
            }
        }

        private System.Windows.Media.Brush _messageForegroundColor;
        public System.Windows.Media.Brush MessageForegroundColor
        {
            get { return _messageForegroundColor; }
            set
            {
                if (this._messageForegroundColor == value) return;
                this._messageForegroundColor = value;
                OnPropertyChanged("MessageForegroundColor");
            }
        }

        private bool _highlightMessageText;

        public bool HighlightMessageText
        {
            get { return _highlightMessageText; }
            set
            {
                if (this._highlightMessageText == value) return;
                this._highlightMessageText = value;
                OnPropertyChanged("HighlightMessageText");
            }
        }



        public List<Requirements> Requirment { get; set; }
        public List<Locations> Location { get; set; }

        



        //=======================================================
        //  Constructor
        //=======================================================
        /// <summary>
        /// Initilzies a new instance of the AlarmItem class
        /// </summary>
        public AlarmItem()
        {

        }


        //=========================================================
        //  Interface Implementations
        //=========================================================
        #region INotifyPropertyChanged Implementation

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null)
            {
                var e = new PropertyChangedEventArgs(propertyName);
                handler(this, e);
            }
        }

        #endregion INotifyPropertyChanged Implementation





    }
}
