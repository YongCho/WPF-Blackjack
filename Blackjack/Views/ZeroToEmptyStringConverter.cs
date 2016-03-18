using System;
using System.Globalization;
using System.Windows.Data;

namespace BlackJack.Views
{
    // Converts 0 to an empty string.
    public class ZeroToEmptyStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((int)value == 0)
            {
                return string.Empty;
            }
            else
            {
                return System.Convert.ToString(value);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
