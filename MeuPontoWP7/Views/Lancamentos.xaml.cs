using System;
using MeuPontoWP7.ViewModel;
using Microsoft.Phone.Controls;

namespace MeuPontoWP7.Views
{
    public partial class Lancamentos : PhoneApplicationPage
    {
        public Lancamentos()
        {
            InitializeComponent();
        }

        private void ApplicationBarIconButton_Click_1(object sender, EventArgs e)
        {
            var lancamentoViewModel = (LancamentoViewModel) DataContext;

            lancamentoViewModel.AdicionarBatida.Execute(null);
        }
    }
}