using System;
using System.Globalization;
using Xamarin.Forms;

namespace TestTask
{
    /// <summary>
    /// Конвертер, що приводить null і <see cref="bool"/> до <see cref="bool"/>.
    /// </summary>
    public class NullToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return false;
            else if (value is string && string.IsNullOrEmpty(value as string))
                return false;
            else if (value is bool && (bool)value == false)
                return false;
            else
                return true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
