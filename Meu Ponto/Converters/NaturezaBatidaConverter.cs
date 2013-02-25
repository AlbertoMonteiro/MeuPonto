using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using Meu_Ponto.ViewModel;

namespace Meu_Ponto.Converters
{
    public class NaturezaBatidaConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var v = (NaturezaBatida)value;

            var imageSource = v == NaturezaBatida.Entrada
                                  ? new Uri("/Imagens/Entrada.png", UriKind.RelativeOrAbsolute)
                                  : new Uri("/Imagens/Saida.png", UriKind.RelativeOrAbsolute);

            return new BitmapImage(imageSource);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}