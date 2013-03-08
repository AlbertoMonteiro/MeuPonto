using System;
using MeuPontoWP7.ViewModel;
using Microsoft.Phone.Controls;

namespace MeuPontoWP7.Views
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        private void ApplicationBarIconButton_OnClick(object sender, EventArgs e)
        {
            ((MainViewModel) DataContext).AdicionarBatida.Execute(null);
        }

        private void ApplicationBarMenuItem_Click_1(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/Lancamentos.xaml", UriKind.RelativeOrAbsolute));
        }
    }
}