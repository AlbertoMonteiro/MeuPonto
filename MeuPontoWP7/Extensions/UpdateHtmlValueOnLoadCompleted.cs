using System.Windows.Interactivity;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;

namespace MeuPontoWP7.Extensions
{
    public class UpdateHtmlValueOnLoadCompleted : Behavior<WebBrowser>
    {
        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.LoadCompleted += AssociatedObjectLoadCompleted;
        }

        private void AssociatedObjectLoadCompleted(object sender, NavigationEventArgs e)
        {
            HtmlStringBinding.SetHtmlString(AssociatedObject, AssociatedObject.SaveToString());
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            this.AssociatedObject.LoadCompleted -= AssociatedObjectLoadCompleted;
        }
    }
}