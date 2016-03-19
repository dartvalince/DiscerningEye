/* ===================================================================
 License:
    DiscerningEye - FFXIV Gathering Companion App
    AlarmSchedule.cs


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


using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DiscerningEye.Model
{
    public class AlarmSchedule
    {

        /// <summary>
        /// Gets or set the name of the alarm schedule
        /// </summary>
        [DataMember(EmitDefaultValue = true)]
        public string Name { get; set; }


        /// <summary>
        /// Gets or sets the List of AlarmItems
        /// </summary>
        [DataMember(EmitDefaultValue = true)]
        public List<AlarmItem> Alarms { get; set; }

        /// <summary>
        /// Gets or sets the file path string of the saved schedule
        /// </summary>
        [DataMember(EmitDefaultValue = true)]
        public string Path { get; set; }

        public AlarmSchedule()
        {

        }

        /// <summary>
        /// Creates a new instance of AlarmSchedule
        /// </summary>
        /// <param name="alarms"></param>
        public AlarmSchedule(List<Model.AlarmItem> alarms)
        {
            this.Alarms = alarms;
        }

       

    }
}
