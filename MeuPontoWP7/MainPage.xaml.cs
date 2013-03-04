using System;
using MeuPontoWP7.ViewModel;
using Microsoft.Phone.Controls;

namespace MeuPontoWP7
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
    }
}