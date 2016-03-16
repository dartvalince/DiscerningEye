/* ===================================================================
 License:
    DiscerningEye - FFXIV Gathering Dictionary and Alarm
    SettingsViewModel.cs


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



using DiscerningEye.Commands;
using DiscerningEye.Model;
using System.Diagnostics;
using System.Windows.Input;
using NAudio;
using NAudio.Wave;

namespace DiscerningEye.ViewModel
{
    public class SettingsViewModel : ViewModelBase
    {
        //=========================================================
        //  Private Fields
        //=========================================================
        private SettingModel _settingsModel;
        private string _testButtonText;

        private string _uiAccentSelectedValue;
        private string _uiAppThemeSelectedValue;
        private IWavePlayer _waveOutDevice;
        private AudioFileReader _audioFileReader;



        //=========================================================
        //  Properties
        //=========================================================
        /// <summary>
        /// Gets the Settings instance
        /// </summary>
        public SettingModel SettingsModel
        {
            get { return _settingsModel; }
            set
            {
                if (this._settingsModel == value)
                    return;
                this._settingsModel = value;
                OnPropertyChanged("SettingsModel");


            }
        }
        
        /// <summary>
        /// Gets or sets the selected UI Accent Value
        /// </summary>
        /// <remarks>
        /// This is bound to a Combobox.SelectedValue on the SettingsView
        /// </remarks>
        public string UIAccentSelectedValue
        {
            get { return this._uiAccentSelectedValue; }
            set
            {
                if (this._uiAccentSelectedValue == value) return;
                this._uiAccentSelectedValue = value;

            }
        }


        /// <summary>
        /// Gets or sets the selected UI theme
        /// </summary>
        /// <remarks>
        /// This is bound to a Combobox.SelectedValue on the SettingsView
        /// </remarks>
        public string UIAppThemeSelectedValue
        {
            get { return this._uiAppThemeSelectedValue; }
            set
            {
                if (this._uiAppThemeSelectedValue == value) return;
                this._uiAppThemeSelectedValue = value;
            }
        }

        public string TestButtonText
        {
            get { return this._testButtonText; }
            set
            {
                if (this._testButtonText == value) return;
                this._testButtonText = value;
                OnPropertyChanged("TestButtonText");
            }
        }

        //=========================================================
        //  Commands
        //=========================================================
        public ICommand SelectFileCommand
        {
            get;
            private set;
        }

        public ICommand TestNotificationCommand
        {
            get;
            private set;
        }



        //=========================================================
        //  Constructor
        //=========================================================
        /// <summary>
        /// Initilizes a new instance of the SettingViewModel class
        /// </summary>
        public SettingsViewModel()
        {
            _settingsModel = new SettingModel();
            this.TestButtonText = "Test Sound";
            this.UIAccentSelectedValue = Properties.Settings.Default.UIAccent;
            this.UIAppThemeSelectedValue = Properties.Settings.Default.UIAppTheme;
            this.SelectFileCommand = new SelectNotificationFileCommand(this);
            this.TestNotificationCommand = new TestNotificationSoundCommand(this);
            _waveOutDevice = new WaveOut();
            _waveOutDevice.PlaybackStopped += _waveOutDevice_PlaybackStopped;
        }


        //=========================================================
        //  Events
        //=========================================================
        private void _waveOutDevice_PlaybackStopped(object sender, StoppedEventArgs e)
        {
            if (_audioFileReader != null)
            {
                _audioFileReader.Dispose();
                _audioFileReader = null;
            }
            this.TestButtonText = "Test Sound";
        }






        //=========================================================
        //  Interface Implementations
        //=========================================================
        #region IDisposeable Implementation

        protected override void OnDispose()
        {
            
            if (_waveOutDevice != null)
            {
                _waveOutDevice.PlaybackStopped -= _waveOutDevice_PlaybackStopped;
                _waveOutDevice.Stop();
                
            }
            if (_audioFileReader != null)
            {
                _audioFileReader.Dispose();
                _audioFileReader = null;
            }
            if (_waveOutDevice != null)
            {
                _waveOutDevice.Dispose();
                _waveOutDevice = null;
            }


            _settingsModel = null;
            this.UIAccentSelectedValue = null;
            this.UIAppThemeSelectedValue = null;
            this.SelectFileCommand = null;
            this.TestNotificationCommand = null;
        }

        #endregion IDisposeable Implementation



        //=========================================================
        //  Methods
        //=========================================================
        public void TestNotificationSound()
        {
            if (_waveOutDevice.PlaybackState != PlaybackState.Playing)
            {

                _audioFileReader = new AudioFileReader(Properties.Settings.Default.NotificationToneUri);
                _waveOutDevice.Init(_audioFileReader);
                _waveOutDevice.Play();
                this.TestButtonText = "Cancel Test";
            }
            else
            {
                _waveOutDevice.Stop();
            }
        }
    }
}
