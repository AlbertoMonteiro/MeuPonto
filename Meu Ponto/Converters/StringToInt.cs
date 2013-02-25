using System;
using System.Globalization;
using System.Windows.Data;

namespace Meu_Ponto.Converters
{
    public class StringToInt : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var str = value.ToString();

            var val = int.Parse(str);
            return val; 
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var val = int.Parse((string) value);
            return val.ToString();
        }
    }
}