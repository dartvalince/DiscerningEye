/* ===================================================================
 License:
    DiscerningEye - FFXIV Gathering Companion App
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



using DiscerningEye.Model;
using MahApps.Metro.Controls.Dialogs;
using NAudio.Wave;
using System.Windows.Input;

namespace DiscerningEye.ViewModel
{
    public class SettingsViewModel : ViewModelBase
    {
        //=========================================================
        //  Private Fields
        //=========================================================
        private string _testButtonText;
        private string _uiAccentSelectedValue;
        private string _uiAppThemeSelectedValue;
        private IWavePlayer _waveOutDevice;
        private AudioFileReader _audioFileReader;



        //=========================================================
        //  Properties
        //=========================================================
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
            this.TestButtonText = "Test Sound";
            this.UIAccentSelectedValue = Properties.Settings.Default.UIAccent;
            this.UIAppThemeSelectedValue = Properties.Settings.Default.UIAppTheme;
            this.SelectFileCommand = new Commands.SettingsViewModelCommands.SelectNotificationFileCommand(this);
            this.TestNotificationCommand = new Commands.SettingsViewModelCommands.TestNotificationSoundCommand(this);
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


            this.UIAccentSelectedValue = null;
            this.UIAppThemeSelectedValue = null;
            this.SelectFileCommand = null;
            this.TestNotificationCommand = null;
        }

        #endregion IDisposeable Implementation



        //=========================================================
        //  Methods
        //=========================================================
        public async void TestNotificationSound()
        {
            if (string.IsNullOrWhiteSpace(Properties.Settings.Default.NotificationToneUri))
            {
                MetroDialogSettings settings = new MetroDialogSettings();
                settings.AffirmativeButtonText = "Ok";
                settings.AnimateHide = true;
                settings.AnimateShow = true;
                settings.DefaultButtonFocus = MessageDialogResult.Affirmative;
                await MainWindow.View.ShowMessageAsync("Notification Path ", string.Format("Notification file path cannot be empty to test sound"), MessageDialogStyle.Affirmative, settings);
                return;
            }
            
            if (_waveOutDevice.PlaybackState != PlaybackState.Playing)
            {

                _audioFileReader = new AudioFileReader(Properties.Settings.Default.NotificationToneUri);
                _audioFileReader.Volume = (float)Properties.Settings.Default.NotificationToneVolume / 100.0f;
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
