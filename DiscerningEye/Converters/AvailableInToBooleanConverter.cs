using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace DiscerningEye.Converters
{
    public class AvailableInToBooleanConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            TimeSpan time;
            TimeSpan.TryParse((string)value, out time);
            if (time > new TimeSpan(0, 5, 0))
                return "Normal";
            else if (time <= new TimeSpan(0, 5, 0) && time > new TimeSpan(0, 1, 0))
                return "Yellow";
            else
                return "Green";

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();

        }



    }
}
