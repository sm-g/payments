using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Windows.Data;
using System.Windows;
using PaymentsWpfApplication.Properties;

namespace PaymentsWpfApplication.Converters
{
    public class SexToStringConverter : IValueConverter
    {
        public virtual object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((byte)value)
            {
                case 9:
                    return Resources.ControlSexNotAppl;
                case 1:
                    return Resources.ControlSexMan;
                case 2:
                    return Resources.ControlSexWoman;
                default:
                    return Resources.ControlSexUnknown;
            }
        }

        public virtual object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
