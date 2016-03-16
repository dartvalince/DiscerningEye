/* ===================================================================
 License:
    DiscerningEye - FFXIV Gathering Dictionary and Alarm
    MainWindowViewModel.cs


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
using DiscerningEye.DataAccess;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace DiscerningEye.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {

        //================================================================
        //  Fields
        //================================================================
        //readonly GatheringItemRepository _gatheringItemRepository;
        public static MainWindowViewModel ViewModel;




        ObservableCollection<ViewModelBase> _viewModels;
        private bool _isSettingsOpen;


        //================================================================
        //  Properties
        //================================================================
        /// <summary>
        /// Gets or sets boolean value representing if the Settings flyout is open
        /// </summary>
        /// <remarks>
        /// This is bound to the IsOpen property of the settings flyout on MainWindow.xaml
        /// </remarks>
        public bool IsSettingsOpen
        {
            get { return this._isSettingsOpen; }
            set
            {
                if (this._isSettingsOpen == value) return;
                this._isSettingsOpen = value;
                if (value == false) this.SettingsSaveCommand.Execute(null);
                OnPropertyChanged("IsSettingsOpen");
            }
        }


        public ObservableCollection<ViewModelBase> ViewModels
        {
            get
            {
                if (_viewModels == null)
                    _viewModels = new ObservableCollection<ViewModelBase>();
                return _viewModels;
            }
        }








        //================================================================
        //  Commands
        //================================================================
        /// <summary>
        /// Gets or sets the SettingsCommand
        /// </summary>
        /// <remarks>
        /// This is bound to the Command of the Settings button on MainWindow.xaml
        /// </remarks>
        public ICommand SettingsCommand
        {
            get;
            private set;
        }


        /// <summary>
        /// Gets or sets the SettingsSaveCommand
        /// </summary>
        /// <remarks>
        /// This is not bound to any controls on a view, instead it is called
        /// explicity when the Settings flyout on MainWindow.xaml is closed.
        /// </remarks>
        public ICommand SettingsSaveCommand
        {
            get;
            private set;
        }


        public ICommand GitHubCommand
        {
            get;
            private set;
        }








        //================================================================
        //  Constructor
        //================================================================
        /// <summary>
        /// Creates a new instance of MainWindowViewModel
        /// </summary>
        public MainWindowViewModel()
        {

            SettingsCommand = new OpenSettingsCommand(this);
            SettingsSaveCommand = new SaveSettingsCommand(this);
            GitHubCommand = new LaunchGithubPageCommand(this);
            MainWindowViewModel.ViewModel = this;
        }


        //================================================================
        //  Functions
        //================================================================

        /// <summary>
        /// Opens the settings flyout on MainWindow.xaml
        /// </summary>
        public void OpenSettings()
        {
            IsSettingsOpen = true;
        }

        /// <summary>
        /// Saves the applicaitons settings
        /// </summary>
        public void SaveSettings()
        {
            Properties.Settings.Default.Save();
        }

        public void LaunchGithubPage()
        {
            System.Diagnostics.Process.Start("https://github.com/dartvalince");
        }


    }
}
