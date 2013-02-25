using System;
using System.Globalization;
using System.Windows.Data;

namespace Meu_Ponto.Converters
{
    public class TimespanToNullableDatetime : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var timeSpan = (TimeSpan)value;

            var dateTime = new DateTime(timeSpan.Ticks);
            return dateTime; 
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var dateTime = value as DateTime?;
            TimeSpan timeSpan = dateTime.HasValue ? TimeSpan.FromTicks(dateTime.Value.Ticks) : TimeSpan.Zero;
            return timeSpan;
        }
    }
}
