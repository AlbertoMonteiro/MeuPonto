using System;
using System.Globalization;
using System.Windows.Data;

namespace MeuPonto.Common.Converters
{
    public class TimespanToNullableDatetime : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null ? new DateTime(((TimeSpan) value).Ticks) : new DateTime?();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var dateTime = value as DateTime?;
            return dateTime.HasValue ? TimeSpan.FromTicks(dateTime.Value.Ticks) : TimeSpan.Zero;
        }
    }
}
