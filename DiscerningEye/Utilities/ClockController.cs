/* ===================================================================
 License:
    DiscerningEye - FFXIV Gathering Companion App
    ClockController.cs


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
using System;
using System.ComponentModel;
using System.Timers;

namespace DiscerningEye.Utilities
{
    public class ClockController : INotifyPropertyChanged, IDisposable
    {
        //================================================================
        //  Fields
        //================================================================
        /// <summary>
        /// Static providing the multipler used to convert earth time to eorzea time
        /// </summary>
        public static double EORZEA_MULTIPLIER = 3600D / 175D;

        private Timer updateTimer;
        private DateTime _currentEorzeaTime;
        private DateTime _currentTimeLocal;
        private DateTime _currentTimeUTC;


        //================================================================
        //  Properties
        //================================================================
        /// <summary>
        /// Gets or sets the DateTime object representing the current eorzean time
        /// </summary>
        public DateTime CurrentTimeEorzea
        {
            get { return this._currentEorzeaTime; }
            set
            {
                if (this._currentEorzeaTime == value) return;
                this._currentEorzeaTime = value;
                OnPropertyChanged("CurrentTimeEorzea");
            }
        }

        /// <summary>
        /// Gets or sets the DateTime object representing the current local time on the users computer
        /// </summary>
        public DateTime CurrentTimeLocal
        {
            get { return this._currentTimeLocal; }
            set
            {
                if (this._currentTimeLocal == value) return;
                this._currentTimeLocal = value;
                OnPropertyChanged("CurrentTimeLocal");
            }
        }

        /// <summary>
        /// Gets or sets the DateTime object representing the current local time as UTC on the users computer
        /// </summary>
        public DateTime CurrentTImeUTC
        {
            get { return this._currentTimeUTC; }
            set
            {
                if (this._currentTimeUTC == value) return;
                this._currentTimeUTC = value;
                OnPropertyChanged("CurrentTImeUTC");
            }
        }

        /// <summary>
        /// Gets the number of minutes to offest the eorzean time by
        /// </summary>
        /// <remarks>
        /// This is used due to the imperfect calulcations made to convert earth to eorzea time.  
        /// </remarks>
        public TimeSpan Offset
        {
            get
            {
                //return new TimeSpan(Properties.Settings.Default.ClockOffsetHours, Properties.Settings.Default.ClockOffsetMinutes, Properties.Settings.Default.ClockOffsetSeconds);
                return new TimeSpan(UserSettingsRepository.Settings.ClockOffsetHours, UserSettingsRepository.Settings.ClockOffsetMinutes, UserSettingsRepository.Settings.ClockOffsetSeconds);
            }
        }



        //================================================================
        //  Constructor
        //================================================================
        /// <summary>
        /// Creates a new instance of the ClockController
        /// </summary>
        public ClockController()
        {
            //  Initilize
            updateTimer = new Timer();
            updateTimer.Interval = 1;
            updateTimer.Elapsed += UpdateTimer_Elapsed;
            updateTimer.Start();

        }




        //================================================================
        //  Interface Implementations
        //================================================================
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


        #region IDisposible Implementation

        public void Dispose()
        {
            this.OnDispose();
        }

        protected virtual void OnDispose()
        {
            this.updateTimer.Elapsed -= UpdateTimer_Elapsed;
            this.updateTimer.Stop();
            this.updateTimer = null;

        }

        #endregion IDisposible Implementation




        //================================================================
        //  Events
        //================================================================
        private void UpdateTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.CurrentTimeLocal = DateTime.Now;
            this.CurrentTImeUTC = DateTime.UtcNow;
            this.CurrentTimeEorzea = CalculateEorzeaTime();
        }



        //================================================================
        //  Functions
        //================================================================
        /// <summary>
        /// Calculates the current eorzea time
        /// </summary>
        /// <returns>
        /// DateTime representing the current eorzea time
        /// </returns>
        private DateTime CalculateEorzeaTime()
        {
            //  Calculate how many ticks have elapsed since Epoch Time
            long epochTicks = this.CurrentTImeUTC.Ticks - (new DateTime(1970, 1, 1).Ticks);

            //  Multiply those ticks by the Eorzea Multipler (approx 20.5)
            long eorzeaTicks = (long)Math.Round(epochTicks * EORZEA_MULTIPLIER);

            //return new DateTime(eorzeaTicks).Add(offset);
            return new DateTime(eorzeaTicks).Add(this.Offset);
        }

        /// <summary>
        /// Generate a timespan representing the Current Eorzea Time
        /// </summary>
        /// <returns></returns>
        public TimeSpan GetEorzeaTimeSpan()
        {
            return new TimeSpan(this.CurrentTimeEorzea.Hour, this.CurrentTimeEorzea.Minute, this.CurrentTimeEorzea.Second);
        }
    }
}
