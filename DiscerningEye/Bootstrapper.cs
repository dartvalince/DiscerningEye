using Prism.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using System.Windows;
using DiscerningEye.Views;

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
