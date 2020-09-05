using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Windows.Data;
using System.Windows;

namespace PaymentsWpfApplication.Converters
{
    public class LogicalNORConverter : IMultiValueConverter
    {
        public virtual object Convert(object[] value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value[0] is bool && value[1] is bool && value.Length == 2)
                return !((bool)value[0] || (bool)value[1]);
            throw new ArgumentException();
        }

        public virtual object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
