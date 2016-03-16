/* ===================================================================
 License:
    DiscerningEye - FFXIV Gathering Dictionary and Alarm
    SettingsModel.cs


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

using System.Collections.Specialized;
using System.ComponentModel;

namespace DiscerningEye.Model
{
    public class SettingModel : INotifyPropertyChanged
    {
        //=========================================================
        //  Properties
        //=========================================================
        #region Clock Settings
        /// <summary>
        /// Gets or sets the number of hours to offset the clock
        /// </summary>
        public int ClockOffsetHours
        {
            get { return Properties.Settings.Default.ClockOffsetHours; }
            set
            {
                if (Properties.Settings.Default.ClockOffsetHours == value) return;
                Properties.Settings.Default.ClockOffsetHours = value;
            }
        }

        /// <summary>
        /// Gets or sets the number of minutes to offset the clock
        /// </summary>
        public int ClockOffsetMinutes
        {
            get { return Properties.Settings.Default.ClockOffsetMinutes; }
            set
            {
                if (Properties.Settings.Default.ClockOffsetMinutes == value) return;
                Properties.Settings.Default.ClockOffsetMinutes = value;
            }
        }

        /// <summary>
        /// Gets or sets the number of seconds to offset the clock
        /// </summary>
        public int ClockOffsetSeconds
        {
            get { return Properties.Settings.Default.ClockOffsetSeconds; }
            set
            {
                if (Properties.Settings.Default.ClockOffsetSeconds == value) return;
                Properties.Settings.Default.ClockOffsetSeconds = value;
            }
        }

        #endregion ClockSettings

        #region Alarm Settings

        /// <summary>
        /// Gets or sets if the alarms should be enabled
        /// </summary>
        public bool EnableAlarms
        {
            get { return Properties.Settings.Default.EnableAlarms; }
            set
            {
                if (Properties.Settings.Default.EnableAlarms == value) return;
                Properties.Settings.Default.EnableAlarms = value;
            }
        }

        /// <summary>
        /// Gets or sets if the early warning should be enabled
        /// </summary>
        public bool EnableEarlyWarning
        {
            get { return Properties.Settings.Default.EnableEarlyWarning; }
            set
            {
                if (Properties.Settings.Default.EnableEarlyWarning == value) return;
                Properties.Settings.Default.EnableEarlyWarning = value;
            }
        }

        /// <summary>
        /// Gets or sets the number of minutes for the early warning
        /// </summary>
        public int EarlyWarningMinutes
        {
            get { return Properties.Settings.Default.EarlyWarningMinutes; }
            set
            {
                if (Properties.Settings.Default.EarlyWarningMinutes == value) return;
                Properties.Settings.Default.EarlyWarningMinutes = value;
            }
        }

        #endregion Alarm Settings


        #region Notification Settings

        /// <summary>
        /// Gets or sets if ballon tips should be shown
        /// </summary>
        public bool EnableBallonTip
        {
            get { return Properties.Settings.Default.EnableBallonTip; }
            set
            {
                if (Properties.Settings.Default.EnableBallonTip == value) return;
                Properties.Settings.Default.EnableBallonTip = value;
            }
        }

        /// <summary>
        /// Gets or sets if notification tones should play
        /// </summary>
        public bool EnableNotificationTone
        {
            get { return Properties.Settings.Default.EnableNotificationTone; }
            set
            {
                if (Properties.Settings.Default.EnableNotificationTone == value) return;
                Properties.Settings.Default.EnableNotificationTone = value;
            }
        }

        /// <summary>
        /// Gets or sets if text to speech should be used
        /// </summary>
        public bool EnableTextToSpeech
        {
            get { return Properties.Settings.Default.EnableTextToSpeech; }
            set
            {
                if (Properties.Settings.Default.EnableTextToSpeech == value) return;
                Properties.Settings.Default.EnableTextToSpeech = value;
            }
        }

        #endregion Notification Settings

        #region Notification Tone Settings

        /// <summary>
        /// Gets or sets the path of the notificaiton tone file
        /// </summary>
        public string NotificationToneUri
        {
            get { return Properties.Settings.Default.NotificationToneUri; }
            set
            {
                if (Properties.Settings.Default.NotificationToneUri == value) return;
                Properties.Settings.Default.NotificationToneUri = value;
            }
        }

        #endregion Notification Tone Settings

        #region Text-to-speech Settings

        /// <summary>
        /// Gets or sets the template message used for early warnings
        /// </summary>
        public string EarlyWarningMessage
        {
            get { return Properties.Settings.Default.EarlyWarningMessage; }
            set
            {
                if (Properties.Settings.Default.EarlyWarningMessage == value) return;
                Properties.Settings.Default.EarlyWarningMessage = value;
            }
        }

        /// <summary>
        /// Gets or sets the template message used for when an item is available
        /// </summary>
        public string ItemAvailableMessage
        {
            get { return Properties.Settings.Default.ItemAvailableMessage; }
            set
            {
                if (Properties.Settings.Default.ItemAvailableMessage == value) return;
                Properties.Settings.Default.ItemAvailableMessage = value;
            }
        }

        #endregion Text-to-speech Settings

        #region Application Settings

        /// <summary>
        /// Gets or sets the language used for the user interface
        /// </summary>
        public string Language
        {
            get { return Properties.Settings.Default.Language; }
            set
            {
                if (Properties.Settings.Default.Language == value) return;
                Properties.Settings.Default.Language = value;
            }
        }

        /// <summary>
        /// Gets or sets the scale of the user inteface
        /// </summary>
        public string UIScale
        {
            get { return Properties.Settings.Default.UIScale; }
            set
            {
                if (Properties.Settings.Default.UIScale == value) return;
                Properties.Settings.Default.UIScale = value;
            }
        }

        /// <summary>
        /// Gets the list of UI Accent values
        /// </summary>
        public StringCollection UIAccentList
        {
            get { return Properties.Settings.Default.UIAccentList; }
        }


        /// <summary>
        /// Gets a list of the UI App Theme values
        /// </summary>
        public StringCollection UIAppThemeList
        {
            get { return Properties.Settings.Default.UIAppThemeList; }
        }

        /// <summary>
        /// Gets a list of the UI Scale values
        /// </summary>
        public StringCollection UIScaleList
        {
            get { return Properties.Settings.Default.UIScaleList; }
        }

        /// <summary>
        /// Gets or sets the theme used for the UI
        /// </summary>
        public string UIAppTheme
        {
            get { return Properties.Settings.Default.UIAppTheme; }
            set
            {
                if (Properties.Settings.Default.UIAppTheme == value) return;
                Properties.Settings.Default.UIAppTheme = value;
                OnPropertyChanged("UIAppTheme");
            }
        }

        /// <summary>
        /// Gets or sets the accent used for the UI
        /// </summary>
        public string UIAccent
        {
            get { return Properties.Settings.Default.UIAccent; }
            set
            {
                if (Properties.Settings.Default.UIAccent == value) return;
                Properties.Settings.Default.UIAccent = value;
                OnPropertyChanged("UIAccent");
            }
        }

        #endregion Application Settings

        /// <summary>
        /// Gets or sets a boolean value representing if the settings interface is open or closed
        /// </summary>
        public bool IsOpen { get; set; }


        //=========================================================
        //  Constructor
        //=========================================================
        /// <summary>
        /// Creates a new instance of the Setttings model
        /// </summary>
        public SettingModel()
        {
            this.IsOpen = false;
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
