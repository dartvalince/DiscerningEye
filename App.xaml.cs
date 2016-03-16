using DiscerningEye.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using MahApps.Metro.Controls;
using MahApps.Metro;

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

            MainWindow window = new DiscerningEye.MainWindow();
            var viewModel = new MainWindowViewModel();
            
            window.DataContext = viewModel;
            window.Show();

            DiscerningEye.Properties.Settings.Default.PropertyChanged += Default_PropertyChanged;

            //  Apply startup theme from users saved settings
            ChangeApplicationTheme();

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
