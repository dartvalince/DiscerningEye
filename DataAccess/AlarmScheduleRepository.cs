/* ===================================================================
 License:
    DiscerningEye - FFXIV Gathering Companion App
    AlarmScheduleRepository.cs


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
using System.Linq;
using System.Runtime.Serialization.Json;

namespace DiscerningEye.DataAccess
{
    public class AlarmScheduleRepository
    {
        //================================================================
        //  Fields
        //================================================================
        private readonly static string _schedulesPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "DiscerningEye\\Schedules");
        private readonly List<Model.AlarmSchedule> _alarmSchedules;



        //================================================================
        //  Constructor
        //================================================================
        /// <summary>
        /// Creates a new instance of AlarmItemRepository
        /// </summary>
        public AlarmScheduleRepository()
        {
            if (_alarmSchedules == null)
                _alarmSchedules = new List<Model.AlarmSchedule>();

            //  Check if schedule directory exists
            
            if (!Directory.Exists(AlarmScheduleRepository._schedulesPath))
                Directory.CreateDirectory(AlarmScheduleRepository._schedulesPath);

            // Load saved Schedules
            DirectoryInfo dirInfo = new DirectoryInfo(AlarmScheduleRepository._schedulesPath);
            FileInfo[] savedSchedules = dirInfo.GetFiles();

            if(savedSchedules.Count() > 0)
            {
                foreach(FileInfo schedule in savedSchedules)
                {
                    DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Model.AlarmSchedule));

                    Model.AlarmSchedule alarmSchedule;
                    using (FileStream fs = new FileStream(schedule.FullName, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        alarmSchedule = (Model.AlarmSchedule)ser.ReadObject(fs);
                    }
                    this._alarmSchedules.Add(alarmSchedule);
                }
            }

        }

        //================================================================
        //  Methods
        //================================================================
        /// <summary>
        /// Gets a shallow copy of the alarm schedule colleciton
        /// </summary>
        /// <returns></returns>
        public List<Model.AlarmSchedule> GetAlarmSchedules()
        {
            return new List<Model.AlarmSchedule>(this._alarmSchedules);
        }

        /// <summary>
        /// Provides a method for saving an alarm schedule
        /// </summary>
        /// <param name="schedule"></param>
        public static void Save(Model.AlarmSchedule schedule)
        {
            if (schedule == null) return;
            string schedulPath = Path.Combine(_schedulesPath, string.Format("{0}.json", schedule.Name));
            using (FileStream fs = new FileStream(schedulPath, FileMode.Create, FileAccess.Write, FileShare.Write))
            {
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Model.AlarmSchedule));

                ser.WriteObject(fs, schedule);
            }
        }

        /// <summary>
        /// Provides a method of deleting an alarm schedule
        /// </summary>
        /// <param name="schedule"></param>
        public static void Delete(Model.AlarmSchedule schedule)
        {
            if (schedule == null) return;
            string schedulePath = Path.Combine(_schedulesPath, string.Format("{0}.json", schedule.Name));
            File.Delete(schedulePath);
        }







    }
}
