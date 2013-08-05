using System;
using MeuPontoWP7.ViewModel;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Scheduler;

namespace MeuPontoWP7.Views
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            Loaded += (sender, args) =>
            {
                var periodic = new PeriodicTask("Temp");
                periodic.Description = "A";

                if (ScheduledActionService.Find("Temp") != null)
                    ScheduledActionService.Remove("Temp");
                ScheduledActionService.Add(periodic);

                ScheduledActionService.LaunchForTest("Temp", TimeSpan.FromSeconds(30));
            };
        }

        private void ApplicationBarIconButton_OnClick(object sender, EventArgs e)
        {
            ((MainViewModel) DataContext).AdicionarBatida.Execute(null);
        }

        private void ApplicationBarMenuItemClick1(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/Lancamentos.xaml", UriKind.RelativeOrAbsolute));
        }

        private void ApplicationBarIconButtonClick1(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/Sobre.xaml", UriKind.RelativeOrAbsolute));
        }

        private void ApplicationBarIconButtonClick2(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/Relatorio.xaml", UriKind.RelativeOrAbsolute));
        }

        private void ApplicationBarMenuItemClick2(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/ImportarBatidas.xaml", UriKind.RelativeOrAbsolute));            
        }
    }
}