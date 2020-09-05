using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Windows.Data;
using System.Windows;
using System.ComponentModel;

namespace PaymentsWpfApplication.Converters
{

    [ValueConversion(typeof(string), typeof(string))]
    public class StringTrimmingConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (value ?? "").ToString().Trim();
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }
}
