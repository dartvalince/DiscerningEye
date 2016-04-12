// TODO:    CLEANUP UNUSED SETTINGS
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.IO;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Prism.Mvvm;

namespace DiscerningEye.Models
{
    [DataContract]
    public class UserSettingsModel :  BindableBase
    {
        

        private int _clockOffsetHours;
        /// <summary>
        /// Gets or sets the number of hours to offset the Eorzea Clock
        /// </summary>
        [DataMember (EmitDefaultValue =true)]
        public int ClockOffsetHours
        {
            get { return _clockOffsetHours; }
            set { SetProperty(ref _clockOffsetHours, value); }
        }


        private int _clockOffsetMinutes;
        /// <summary>
        /// Gets or sets the number of minutes to offest the Eorzea Clock
        /// </summary>
        [DataMember(EmitDefaultValue = true)]
        public int ClockOffsetMinutes
        {
            get { return _clockOffsetMinutes; }
            set { SetProperty(ref _clockOffsetMinutes, value); }
        }

        private int _clockOffsetSeconds;
        /// <summary>
        /// Gets or sets the number of seconds to offset the Eorzea Clock
        /// </summary>
        [DataMember(EmitDefaultValue = true)]
        public int ClockOffsetSeconds
        {
            get { return _clockOffsetSeconds; }
            set { SetProperty(ref _clockOffsetSeconds, value); }
        }

        private bool _enableAlarms;
        /// <summary>
        /// Gets or sets a boolean value represnting if alarms should be enabled
        /// </summary>
        [DataMember(EmitDefaultValue = true)]
        public bool EnableAlarms
        {
            get { return _enableAlarms; }
            set { SetProperty(ref _enableAlarms, value); }
        }

        private bool _enableEarlyWarning;
        /// <summary>
        /// Gets or sets a boolean value representing if early warnings should be enabled
        /// </summary>
        [DataMember(EmitDefaultValue = true)]
        public bool EnableEarlyWarning
        {
            get { return _enableEarlyWarning; }
            set { SetProperty(ref _enableEarlyWarning, value); }
        }

        private int _earlyWarningMinutes;
        /// <summary>
        /// Gets or sets the number of minutes before an alarm is triggered that an early warning should go out
        /// </summary>
        [DataMember(EmitDefaultValue = true)]
        public int EarlyWarningMinutes
        {
            get { return _earlyWarningMinutes; }
            set { SetProperty(ref _earlyWarningMinutes, value); }
        }

        private bool _enableBallonTips;
        /// <summary>
        /// Gets or sets the boolean value represnting if ballon tip notifications should be enabled
        /// </summary>
        [DataMember(EmitDefaultValue = true)]
        public bool EnableBallonTip
        {
            get { return _enableBallonTips; }
            set { SetProperty(ref _enableBallonTips, value); }
        }

        private bool _enableNotificationTone;
        /// <summary>
        /// Gets or sets a boolean value representing if notification tones should be enabled
        /// </summary>
        [DataMember(EmitDefaultValue = true)]
        public bool EnableNotificationTone
        {
            get { return _enableNotificationTone; }
            set { SetProperty(ref _enableNotificationTone, value); }
        }

        private bool _enableTextToSpeech;
        /// <summary>
        /// Gets or sets a boolean value representing if text to speech should be enabled on notifications
        /// </summary>
        [DataMember(EmitDefaultValue = true)]
        public bool EnableTextToSpeech
        {
            get { return _enableTextToSpeech; }
            set { SetProperty(ref _enableTextToSpeech, value); }
        }

        private string _notificationToneUri;
        /// <summary>
        /// Gets or sets the URI/File Path of the notification tone file
        /// </summary>
        [DataMember(EmitDefaultValue = true)]
        public string NotificationToneUri
        {
            get { return _notificationToneUri; }
            set { SetProperty(ref _notificationToneUri, value); }
        }

        private string _earlyWarningMessage;
        /// <summary>
        /// Gets or sets the string representing the template to use for early warning notification messages
        /// </summary>
        [DataMember(EmitDefaultValue = true)]
        public string EarlyWarningMessage
        {
            get { return _earlyWarningMessage; }
            set { SetProperty(ref _earlyWarningMessage, value); }
        }

        private string _itemAvailableMessage;
        /// <summary>
        /// gets or sets the string representing the template to use for item avaialble notification messages
        /// </summary>
        [DataMember(EmitDefaultValue = true)]
        public string ItemAvailableMessage
        {
            get { return _itemAvailableMessage; }
            set { SetProperty(ref _itemAvailableMessage, value); }

        }

        private string _language;
        /// <summary>
        /// Gets or sets the language to use for the UI
        /// </summary>
        [DataMember(EmitDefaultValue = true)]
        public string Language
        {
            get { return _language; }
            set { SetProperty(ref _language, value); }

        }

        private string _uiScale;
        /// <summary>
        /// Gets or sets the scale value of the UI
        /// </summary>
        [DataMember(EmitDefaultValue = true)]
        public string UIScale
        {
            get { return _uiScale; }
            set { SetProperty(ref _uiScale, value); }

        }

        private System.Collections.Specialized.StringCollection _uiScaleList;
        /// <summary>
        /// Gets or sets the string collection representing the various Ui Scale values the user can choose from
        /// </summary>
        [DataMember(EmitDefaultValue = true)]
        public System.Collections.Specialized.StringCollection UIScaleList
        {
            get { return _uiScaleList; }
            set { SetProperty(ref _uiScaleList, value); }

        }

        private bool _isSettingsOpen;
        /// <summary>
        /// Gets or sets a boolean value representing if the settings flyout is open
        /// </summary>
        [DataMember(EmitDefaultValue = true)]
        public bool IsSettingsOpen
        {
            get { return _isSettingsOpen; }
            set { SetProperty(ref _isSettingsOpen, value); }

        }

        private System.Collections.Specialized.StringCollection _uiAccentList;
        /// <summary>
        /// Gets or sets the string collection representing the various UI accent values the user can choose from
        /// </summary>
        [DataMember(EmitDefaultValue = true)]
        public System.Collections.Specialized.StringCollection UIAccentList
        {
            get { return _uiAccentList; }
            set { SetProperty(ref _uiAccentList, value); }

        }

        private System.Collections.Specialized.StringCollection _uiAppThemeList;
        /// <summary>
        /// Gets or sets the string collection representing the various UI App theme values the user can choose from
        /// </summary>
        [DataMember(EmitDefaultValue = true)]
        public System.Collections.Specialized.StringCollection UIAppThemeList
        {
            get { return _uiAppThemeList; }
            set { SetProperty(ref _uiAppThemeList, value); }

        }

        private string _uiAccent;
        /// <summary>
        /// Gets or sets the string value representing the UI accent value the user has choosen
        /// </summary>
        [DataMember(EmitDefaultValue = true)]
        public string UIAccent
        {
            get { return _uiAccent; }
            set { SetProperty(ref _uiAccent, value); }

        }

        private string _uiAppTheme;
        /// <summary>
        /// Gets or sets the string value representing the UI App theme value the user has choosen
        /// </summary>
        [DataMember(EmitDefaultValue = true)]
        public string UIAppTheme
        {
            get { return _uiAppTheme; }
            set { SetProperty(ref _uiAppTheme, value); }

        }

        private System.Collections.Specialized.StringCollection _groupingPropertyList;
        /// <summary>
        /// Gets or sets the string collection rperesenting the various group properties the use can choose from
        /// </summary>
        [DataMember(EmitDefaultValue = true)]
        public System.Collections.Specialized.StringCollection GroupingPropertyList
        {
            get { return _groupingPropertyList; }
            set { SetProperty(ref _groupingPropertyList, value); }

        }

        private bool _doNotDisturb;
        /// <summary>
        /// Gets or sets the boolean value representing if Do Not Disturb is enabled
        /// </summary>
        [DataMember(EmitDefaultValue = true)]
        public bool DoNotDisturb
        {
            get { return _doNotDisturb; }
            set { SetProperty(ref _doNotDisturb, value); }

        }

        private int _notificationToneVolume;
        /// <summary>
        /// Gets or sets the value representing the loudness of the notifiction tone
        /// </summary>
        [DataMember(EmitDefaultValue = true)]
        public int NotificationToneVolume
        {
            get { return _notificationToneVolume; }
            set { SetProperty(ref _notificationToneVolume, value); }

        }

        private int _textToSpeechVolume;
        /// <summary>
        /// Gets or sets the value representing the loudness of the text to speech
        /// </summary>
        [DataMember(EmitDefaultValue = true)]
        public int TextToSpeechVolume
        {
            get { return _textToSpeechVolume; }
            set { SetProperty(ref _textToSpeechVolume, value); }

        }

        private string _releaseUri;
        /// <summary>
        /// Gets or sets the URI to use for checking release updates
        /// </summary>
        [DataMember(EmitDefaultValue = true)]
        public string ReleaseUri
        {
            get { return _releaseUri; }
            set { SetProperty(ref _releaseUri, value); }

        }

        private bool _useInverseOnFlyout;
        /// <summary>
        /// Gets or sets the boolean value representing if flyout controls should use the INverse theme property
        /// </summary>
        [DataMember(EmitDefaultValue = true)]
        public bool UseInverseOnFlyout
        {
            get { return _useInverseOnFlyout; }
            set { SetProperty(ref _useInverseOnFlyout, value); }

        }

        private int _earlyWarningSeconds;
        /// <summary>
        /// Gets or sets a value representing the number of seconds before an alarm is triggered that an early warning should be triggered
        /// </summary>
        [DataMember(EmitDefaultValue = true)]
        public int EarlyWarningSeconds
        {
            get { return _earlyWarningSeconds; }
            set { SetProperty(ref _earlyWarningSeconds, value); }

        }

        private string _assemblyVersion;
        /// <summary>
        /// Gets or sets the assmebly version string used to check for updates for the application
        /// </summary>
        [DataMember(EmitDefaultValue = true)]
        public string AssemblyVersion
        {
            get { return _assemblyVersion; }
            set { SetProperty(ref _assemblyVersion, value); }

        }

        private bool _useNightMode;
        /// <summary>
        /// Gets or sets the boolean value representing if the BaseDark "night mode" theme should be used
        /// </summary>
        [DataMember (EmitDefaultValue =true)]
        public bool UseNightMode
        {
            get { return _useNightMode; }
            set { SetProperty(ref _useNightMode, value); }
        }


        private bool _shouldShowCircles;
        /// <summary>
        /// Gets or sets the boolean value representing if the circle indicators should be shown on data grids for upcoming alarms
        /// </summary>
        [DataMember(EmitDefaultValue = true)]
        public bool ShouldShowCircles
        {
            get { return _shouldShowCircles; }
            set { SetProperty(ref _shouldShowCircles, value); }
        }

        private bool _shouldHighlightRows;
        /// <summary>
        /// Gets or sets the boolean value representing if data rows should be highlighted for upcoming alarms
        /// </summary>
        [DataMember(EmitDefaultValue = true)]
        public bool ShouldHighlightRows
        {
            get { return _shouldHighlightRows; }
            set { SetProperty(ref _shouldHighlightRows, value); }
        }

        private bool _minimizeToTaskBar;
        /// <summary>
        /// Gets or sets a boolean value representing if the application should minimize to the taskbar
        /// </summary>
        public bool MinimizeToTaskBar
        {
            get { return _minimizeToTaskBar; }
            set { SetProperty(ref _minimizeToTaskBar, value); }
        }

        private bool _hideSystemTrayIcon;
        /// <summary>
        /// Gets or sets a boolean value representing if the system tray icon should be hidden
        /// </summary>
        public bool HideSystemTrayIcon
        {
            get { return _hideSystemTrayIcon; }
            set { SetProperty(ref _hideSystemTrayIcon, value); }
        }







        //  FOr Json.Net
        protected UserSettingsModel()
        {

        }


        /// <summary>
        /// Creates a new instance of UserSettingsModel
        /// </summary>
        public UserSettingsModel(bool loadDefaults)
        {
            if (loadDefaults)
            {
                this.ClockOffsetHours = 0;
                this.ClockOffsetMinutes = 0;
                this.ClockOffsetSeconds = 0;
                this.EnableAlarms = true;
                this.EnableEarlyWarning = true;
                this.EarlyWarningMinutes = 2;
                this.EnableBallonTip = true;
                this.EnableNotificationTone = true;
                this.EnableTextToSpeech = true;
                this.NotificationToneUri = "";
                this.EarlyWarningMessage = "{item} will be avaialble in {time}";
                this.ItemAvailableMessage = "{item} is avaialbe to be gathered in {zone}";
                this.Language = "English";
                this.UseNightMode = false;
                this.UIScale = "1.0";
                this.UIScaleList = new System.Collections.Specialized.StringCollection();
                this.UIScaleList.AddRange(
                    new string[]
                    {
                    "0.5",
                    "0.6",
                    "0.7",
                    "0.8",
                    "0.9",
                    "1.0",
                    "1.1",
                    "1.2",
                    "1.3",
                    "1.4",
                    "1.5"
                    });
                this.IsSettingsOpen = true;
                this.UIAccentList = new System.Collections.Specialized.StringCollection();
                this.UIAccentList.AddRange(
                    new string[]
                    {
                    "Amber",
                    "Blue",
                    "Brown",
                    "Cobalt",
                    "Crimson",
                    "Cyan",
                    "Emerald",
                    "Green",
                    "Indigo",
                    "Lime",
                    "Magenta",
                    "Mauve",
                    "Olive",
                    "Orange",
                    "Pink",
                    "Purple",
                    "Red",
                    "Sienna",
                    "Steel",
                    "Taupe",
                    "Teal",
                    "Violet",
                    "Yellow"
                    });
                this.UIAppThemeList = new System.Collections.Specialized.StringCollection();
                this.UIAppThemeList.AddRange(
                    new string[]
                    {
                    "BaseDark",
                    "BaseLight"
                    });
                this.UIAccent = "Orange";
                this.UIAppTheme = "BaseLight";
                this.GroupingPropertyList = new System.Collections.Specialized.StringCollection();
                this.GroupingPropertyList.AddRange(
                    new string[]
                    {
                    "Alarm Checked",
                    "Is Folklore",
                    "Job",
                    "Level",
                    "Node Type",
                    "Region",
                    "Stars",
                    "Start Time",
                    "Zone"
                    });
                this.DoNotDisturb = false;
                this.NotificationToneVolume = 100;
                this.TextToSpeechVolume = 100;
                this.ReleaseUri = "https://github.com/Dartvalince/DiscerningEye";
                this.UseInverseOnFlyout = false;
                this.EarlyWarningSeconds = 0;
                this.AssemblyVersion = "0.1.2.12";
                this.ShouldShowCircles = true;
                this.ShouldHighlightRows = true;
                this.MinimizeToTaskBar = true;
            }


        }
    }
}
