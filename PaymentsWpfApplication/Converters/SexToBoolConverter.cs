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
    public class SexToBoolConverter : IValueConverter
    {
        public virtual object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string param = (string)parameter;
            switch ((byte)value)
            {
                case 9:
                    return param == "not";
                case 1:
                    return param == "male";
                case 2:
                    return param == "female";
                default:
                    return param == "unk";
            }
        }

        public virtual object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string param = (string)parameter;
            bool isChecked = (bool)value;
            if (isChecked)
                switch (param)
                {
                    case "not":
                        return 9;
                    case "male":
                        return 1;
                    case "female":
                        return 2;
                    default:
                        return 0;
                }
            return 0;
        }
    }
}
