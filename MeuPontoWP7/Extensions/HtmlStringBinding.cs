using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using Microsoft.Phone.Controls;

namespace MeuPontoWP7.Extensions
{
    public class HtmlStringBinding : DependencyObject
    {
        public static readonly DependencyProperty HtmlStringProperty =
            DependencyProperty.RegisterAttached(
                "HtmlString",
                typeof(string),
                typeof(HtmlStringBinding),
                new PropertyMetadata(OnHtmlStringPropertyChanged));

        private static void OnHtmlStringPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //if (e.NewValue != e.OldValue)
            //{
            //    WebBrowser wb = (WebBrowser)d;
            //    wb.NavigateToString((string)e.NewValue);
            //}
        }

        public static void SetHtmlString(DependencyObject obj, string html)
        {
            obj.SetValue(HtmlStringProperty, html);
        }

        public static string GetHtmlString(DependencyObject obj)
        {
            return (string)obj.GetValue(HtmlStringProperty);
        }
    }
}