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
    /// Interaction logic for GatheringItemListView.xaml
    /// </summary>
    public partial class GatheringItemListView : UserControl
    {
        public GatheringItemListView()
        {
            InitializeComponent();
            //this.DataContext = new ViewModel.GatheringItemViewModel(this.mapCanvas);
            this.DataContext = new ViewModel.GatheringItemViewModel();
        }
        const double scalerate = 1.1;
        private void mapCanvas_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            //if(e.Delta > 0)
            //{
            //    canvaseScaleTransform.ScaleX *= scalerate;
            //    canvaseScaleTransform.ScaleY *= scalerate;
            //}
            //else
            //{
            //    canvaseScaleTransform.ScaleX /= scalerate;
            //    canvaseScaleTransform.ScaleY /= scalerate;
            //}

        }
    }
}

