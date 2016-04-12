/* ===================================================================
 License:
    DiscerningEye - FFXIV Gathering Companion App
    MainWindowViewModels.cs


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
using MahApps.Metro.Controls.Dialogs;
using Prism.Commands;
using Prism.Regions;
using Squirrel;
using System;
using System.Windows;

namespace DiscerningEye.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly IRegionManager _regionManager;

        private IUpdateManager updateManager;

        private bool _isGatheringDictionaryOpen;
        /// <summary>
        /// Gets or sets a boolean value representing if the Gathering Dicitonary flyout
        /// shoudl be open or closed
        /// </summary>
        /// <remarks>
        /// This is bound to the IsOpen property of the GatheringDictionary flyout on MainWindow.xaml
        /// </remarks>
        public bool IsGatheringDictionaryOpen
        {
            get { return this._isGatheringDictionaryOpen; }
            set { SetProperty(ref _isGatheringDictionaryOpen, value); }
        }

        private bool _isSettingsOpen;
        /// <summary>
        /// Gets or sets a boolean value representing if the Settings flyout should be open
        /// or closed
        /// </summary>
        /// <remarks>
        /// This is boud to the IsOpen property of the Settings flyout on MainWindow.xaml
        /// </remarks>
        public bool IsSettingsOpen
        {
            get { return _isSettingsOpen; }
            set { SetProperty(ref _isSettingsOpen, value); }
        }


        private string _windowTitle;
        /// <summary>
        /// Gets or sets the string representing the title to use for the window
        /// </summary>
        /// <remarks>
        /// This is bound to the Title property of the window on MainWindow.xaml
        /// </remarks>
        public string WindowTitle
        {
            get { return _windowTitle; }
            set { SetProperty(ref _windowTitle, value); }
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
        public DelegateCommand SettingsCommand { get; private set; }


        /// <summary>
        /// Gets or sets the SettingsSaveCommand
        /// </summary>
        /// <remarks>
        /// This is not bound to any controls on a view, instead it is called
        /// explicity when the Settings flyout on MainWindow.xaml is closed.
        /// </remarks>
        public DelegateCommand SettingsSaveCommand { get; private set; }

        /// <summary>
        /// Gets or sets the GitHubCommand
        /// </summary>
        /// <remarks>
        /// This is bound to a menu item on the notification icon context menu
        /// in MainWindow.xaml
        /// </remarks>
        public DelegateCommand GitHubCommand { get; private set; }

        /// <summary>
        /// Gets or sets the MinimalNotificationCommand
        /// </summary>
        /// <remarks>
        /// This is bound to a menu item on the notification icon context menu
        /// in MainWindow.xaml
        /// </remarks>
        public DelegateCommand MinimalNotificationCommand { get; private set; }

        /// <summary>
        /// Gets or sets the AllNotificationCommand
        /// </summary>
        /// <remarks>
        /// This is bound to a menu item on the notification icon context menu
        /// in MainWindow.xaml
        /// </remarks>
        public DelegateCommand AllNotificationsCommand { get; private set; }

        /// <summary>
        /// Gets or sets the OpenGatheringDicationaryCommand
        /// </summary>
        /// <remarks>
        /// This is bound to the window title button on MainWindow.xaml
        /// </remarks>
        public DelegateCommand OpenGatheringDictionaryCommand { get; private set; }

        /// <summary>
        /// Gets or sets the OpenSettingsCommand
        /// </summary>
        /// <remarks>
        /// This is bound to the window title button on MainWindow.xaml
        /// </remarks>
        public DelegateCommand OpenSettingsCommand { get; private set; }

        public DelegateCommand<string> NavigateCommand { get; set; }







        //================================================================
        //  Constructor
        //================================================================
        /// <summary>
        /// Creates a new instance of MainWindowViewModel
        /// </summary>
        public MainWindowViewModel(IRegionManager regionManager)
        {

            _regionManager = regionManager;

            //  Setup all of the delegate commands
            this.NavigateCommand = new DelegateCommand<string>(Navigate);

            this.GitHubCommand = new DelegateCommand(() =>
            {
                System.Diagnostics.Process.Start("https://github.com/dartvalince/DiscerningEye");
            }, () => true);

            this.MinimalNotificationCommand = new DelegateCommand(() =>
            {
                //Properties.Settings.Default.EnableAlarms = true;
                //Properties.Settings.Default.EnableBallonTip = true;
                //Properties.Settings.Default.EnableEarlyWarning = true;
                //Properties.Settings.Default.EnableNotificationTone = false;
                //Properties.Settings.Default.EnableTextToSpeech = false;
                UserSettingsRepository.Settings.EnableAlarms = true;
                UserSettingsRepository.Settings.EnableBallonTip = true;
                UserSettingsRepository.Settings.EnableEarlyWarning = true;
                UserSettingsRepository.Settings.EnableNotificationTone = false;
                UserSettingsRepository.Settings.EnableTextToSpeech = false;
            }, () => true);


            this.AllNotificationsCommand = new DelegateCommand(() =>
            {
                //Properties.Settings.Default.EnableAlarms = true;
                //Properties.Settings.Default.EnableBallonTip = true;
                //Properties.Settings.Default.EnableEarlyWarning = true;
                //Properties.Settings.Default.EnableNotificationTone = true;
                //Properties.Settings.Default.EnableTextToSpeech = true;
                UserSettingsRepository.Settings.EnableAlarms = true;
                UserSettingsRepository.Settings.EnableBallonTip = true;
                UserSettingsRepository.Settings.EnableEarlyWarning = true;
                UserSettingsRepository.Settings.EnableNotificationTone = true;
                UserSettingsRepository.Settings.EnableTextToSpeech = true;
            }, () => true);


            this.OpenGatheringDictionaryCommand = new DelegateCommand(() =>
            {
                this.IsGatheringDictionaryOpen = !this.IsGatheringDictionaryOpen;
            }, () => true);

            this.OpenSettingsCommand = new DelegateCommand(() =>
            {
                this.IsSettingsOpen = !this.IsSettingsOpen;
            }, () => true);

            //  Set the window title
            this.WindowTitle = string.Format("Discerning Eye (v{0})", typeof(MainWindowViewModel).Assembly.GetName().Version);

            //  Check for updates for the application
#if (!DEBUG)
            this.CheckForUpdate();
#endif
        }


        //=========================================================
        //  Interface Implementations
        //=========================================================
        #region IDisposeable Implementation

        protected override void OnDispose()
        {
            if (updateManager != null)
                updateManager.Dispose();
            updateManager = null;
        }

        #endregion IDisposeable Implementation






        //================================================================
        //  Functions
        //================================================================
        private void Navigate(string uri)
        {
            _regionManager.RequestNavigate("ContentRegion", uri);
        }


        /// <summary>
        /// Utilizing the Squirrel.Windows framework, check for updates on GitHub for the applicaiton
        /// and download/install them if needed.
        /// </summary>
        private async void CheckForUpdate()
        {
            try
            {
                using (var mgr = await UpdateManager.GitHubUpdateManager("https://github.com/dartvalince/DiscerningEye"))
                {
                    updateManager = mgr;
                    
                    
                    var release = await mgr.UpdateApp();
                    
                }
            }
            catch (Exception ex)
            {
                string message = ex.Message + Environment.NewLine;
                if (ex.InnerException != null)
                    message += ex.InnerException.Message;
                MessageBox.Show(message);
            }

            //if(Properties.Settings.Default.AssemblyVersion != typeof(MainWindowViewModel).Assembly.GetName().Version.ToString())
            if (UserSettingsRepository.Settings.AssemblyVersion != typeof(MainWindowViewModel).Assembly.GetName().Version.ToString())
            {
                //Properties.Settings.Default.AssemblyVersion = typeof(MainWindowViewModel).Assembly.GetName().Version.ToString();
                UserSettingsRepository.Settings.AssemblyVersion = typeof(MainWindowViewModel).Assembly.GetName().Version.ToString();
                MetroDialogSettings settings = new MetroDialogSettings();
                settings.AffirmativeButtonText = "View Changes On GitHub";
                settings.NegativeButtonText = "Close";
                settings.AnimateHide = true;
                settings.AnimateShow = true;
                settings.DefaultButtonFocus = MessageDialogResult.Affirmative;
                //var result = await Views.MainWindow.View.ShowMessageAsync("Application Updated", string.Format("This application has been udpated to version {0}", Properties.Settings.Default.AssemblyVersion), MessageDialogStyle.AffirmativeAndNegative, settings);
                var result = await Views.MainWindow.View.ShowMessageAsync("Application Updated", string.Format("This application has been udpated to version {0}", UserSettingsRepository.Settings.AssemblyVersion), MessageDialogStyle.AffirmativeAndNegative, settings);
                if (result == MessageDialogResult.Affirmative)
                    System.Diagnostics.Process.Start("https://github.com/dartvalince/DiscerningEye/releases/latest");

            }
        }
    }
}
