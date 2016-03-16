/* ===================================================================
 License:
    DiscerningEye - FFXIV Gathering Dictionary and Alarm
    AlarmItemRepository.cs


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
using System.IO;
using System.Runtime.Serialization.Json;
using System.Windows;

namespace DiscerningEye.DataAccess
{
    public class AlarmItemRepository
    {
        //================================================================
        //  Fields
        //================================================================
        readonly List<Model.AlarmItem> _alarmItems;



        //================================================================
        //  Constructor
        //================================================================
        /// <summary>
        /// Creates a new instance of AlarmItemRepository
        /// </summary>
        public AlarmItemRepository()
        {
            if (_alarmItems == null)
                _alarmItems = new List<Model.AlarmItem>();

            //  Read from the json file
            using (Stream stream = Application.GetResourceStream(new Uri("pack://application:,,,/DiscerningEye;component/Resources/alarmdata.json")).Stream)
            {
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(List<Model.AlarmItem>));
                _alarmItems = (List<Model.AlarmItem>)ser.ReadObject(stream);
            }
        }


        //================================================================
        //  Methods
        //================================================================
        /// <summary>
        /// Returns a shallow copy fo the alrm items list
        /// </summary>
        /// <returns></returns>
        public List<Model.AlarmItem> GetAlarmItems()
        {
            return new List<Model.AlarmItem>(_alarmItems);
        }
    }
}
