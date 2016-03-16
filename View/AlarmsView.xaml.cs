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


namespace DiscerningEye.View
{
    /// <summary>
    /// Interaction logic for AlarmsView.xaml
    /// </summary>
    public partial class AlarmsView : UserControl
    {
        public static AlarmsView View;
        public AlarmsView()
        {
            InitializeComponent();
            View = this;
            this.DataContext = new ViewModel.AlarmsViewModel();
            
        }
    }
}
