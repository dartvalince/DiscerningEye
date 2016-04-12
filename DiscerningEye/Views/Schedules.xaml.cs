using DiscerningEye.Models.AlarmItem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DiscerningEye.Views
{
    /// <summary>
    /// Interaction logic for Schedules.xaml
    /// </summary>
    public partial class Schedules : UserControl
    {
        public static Schedules View;
        public Schedules()
        {
            InitializeComponent();
            Schedules.View = this;
        }


        private void ScheduleAlarmsViewSource_Filter(object sender, System.Windows.Data.FilterEventArgs e)
        {
            AlarmItem i = e.Item as AlarmItem;
            if (i != null)
            {
                e.Accepted = i.IsSet == true;
            }
        }
    }
}
