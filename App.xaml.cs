/* ===================================================================
 License:
    DiscerningEye - FFXIV Gathering Companion App
    App.xaml.cs


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


using DiscerningEye.ViewModel;
using MahApps.Metro;
using Squirrel;
using System.Threading.Tasks;
using System.Windows;

namespace DiscerningEye
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);


            Task.Run(async () =>
            {
                using (var mgr = UpdateManager.GitHubUpdateManager("https://github.com/Dartvalince/DiscerningEye"))
                {
                    await mgr.Result.UpdateApp();
                }
            });

            MainWindow window = new DiscerningEye.MainWindow();
            var viewModel = new MainWindowViewModel();
            
            window.DataContext = viewModel;
            window.Show();
            DiscerningEye.App.Current.Exit += Current_Exit;

            DiscerningEye.Properties.Settings.Default.PropertyChanged += Default_PropertyChanged;

            //  Apply startup theme from users saved settings
            ChangeApplicationTheme();

        }

        private void Current_Exit(object sender, ExitEventArgs e)
        {
            DiscerningEye.Properties.Settings.Default.Save();
        }

        private void Default_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "UIAccent" || e.PropertyName == "UIAppTheme")
            {
                ChangeApplicationTheme();
            }

        }

        private void ChangeApplicationTheme()
        {
            var theme = ThemeManager.DetectAppStyle(Application.Current);
            ThemeManager.ChangeAppStyle(Application.Current,
                             ThemeManager.GetAccent(DiscerningEye.Properties.Settings.Default.UIAccent),
                             ThemeManager.GetAppTheme(DiscerningEye.Properties.Settings.Default.UIAppTheme));
        }
    }
}
