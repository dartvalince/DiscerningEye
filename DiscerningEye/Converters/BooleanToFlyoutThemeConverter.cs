using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace DiscerningEye.Converters
{
    public class BooleanToFlyoutThemeConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value)
                return MahApps.Metro.Controls.FlyoutTheme.Inverse;
            else
                return MahApps.Metro.Controls.FlyoutTheme.Adapt;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();

        }



    }
}
