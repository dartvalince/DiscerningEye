using Prism.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using System.Windows;
using DiscerningEye.Views;
using DiscerningEye.DataAccess;

namespace DiscerningEye
{
    public class Bootstrapper : UnityBootstrapper
    {

        protected override DependencyObject CreateShell()
        {
            return Container.Resolve<DiscerningEye.Views.MainWindow>();
        }

        protected override void InitializeShell()
        {
            Application.Current.MainWindow.Show();
            Application.Current.MainWindow.StateChanged += MainWindow_StateChanged;
            ((MainWindow)(Application.Current.MainWindow)).notificationIcon.TrayMouseDoubleClick += NotificationIcon_TrayMouseDoubleClick;
        }

        private void NotificationIcon_TrayMouseDoubleClick(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow.WindowState == WindowState.Minimized)
            {
                Application.Current.MainWindow.Show();
                Application.Current.MainWindow.WindowState = WindowState.Normal;

            }
        }

        private void MainWindow_StateChanged(object sender, EventArgs e)
        {
            if(UserSettingsRepository.Settings != null)
            {
                if (Application.Current.MainWindow.WindowState == System.Windows.WindowState.Minimized && !UserSettingsRepository.Settings.MinimizeToTaskBar)
                    Application.Current.MainWindow.Hide();
            }
        }

        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();

            Container.RegisterType(typeof(object), typeof(Alarms), "Alarms");
            Container.RegisterType(typeof(object), typeof(Schedules), "Schedules");
            Container.RegisterType(typeof(object), typeof(GatheringDictionary), "GatheringDictionary");
            Container.RegisterType(typeof(object), typeof(Settings), "Settings");
            Container.RegisterType(typeof(object), typeof(About), "About");
        }
    }
}
