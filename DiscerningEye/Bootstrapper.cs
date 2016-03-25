using Prism.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using System.Windows;

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
    }
}
