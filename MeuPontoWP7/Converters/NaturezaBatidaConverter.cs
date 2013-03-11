using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using MeuPonto.Common;

namespace MeuPontoWP7.Converters
{
    public class NaturezaBatidaToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
			var v = (NaturezaBatida)value;
            
            var imageSource = v == NaturezaBatida.Entrada
                ? new Uri("/Images/Entrada.png", UriKind.RelativeOrAbsolute)
                : new Uri("/Images/Saida.png", UriKind.RelativeOrAbsolute);
            
            return new BitmapImage(imageSource);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (System.ComponentModel.DesignerProperties.GetIsInDesignMode(new BitmapImage())) 
                return value;
            return value;
        }
    }

    public class NaturezaBatidaToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var naturezaBatida = (NaturezaBatida)value;

            return naturezaBatida == NaturezaBatida.Entrada
                ? "#FF3EBB55"
                : "#FFF54949";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (System.ComponentModel.DesignerProperties.GetIsInDesignMode(new BitmapImage()))
                return value;
            return value;
        }
    }
}