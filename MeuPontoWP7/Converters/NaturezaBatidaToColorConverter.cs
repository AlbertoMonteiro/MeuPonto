using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using MeuPonto.Common;

namespace MeuPontoWP7.Converters
{
    public class NaturezaBatidaToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var naturezaBatida = (NaturezaBatida)value;

            return naturezaBatida == NaturezaBatida.Entrada
                       ? "#FF7FBA00"
                       : "#FFF25022";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (System.ComponentModel.DesignerProperties.GetIsInDesignMode(new BitmapImage()))
                return value;
            return value;
        }
    }
}