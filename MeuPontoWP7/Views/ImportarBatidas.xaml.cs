using System;
using System.Windows;
using MeuPontoWP7.ViewModel;
using Microsoft.Phone.Controls;

namespace MeuPontoWP7.Views
{
    public partial class ImportarBatidas : PhoneApplicationPage
    {
        private ImportarBatidasViewModel importarBatidasViewModel;

        public ImportarBatidas()
        {
            InitializeComponent();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            importarBatidasViewModel = (ImportarBatidasViewModel) DataContext;
            AppBarIconBtnImportar.IsEnabled = (importarBatidasViewModel.ImportarBatidasState == ImportarBatidasState.Importando);
            importarBatidasViewModel.PropertyChanged += (o, args) =>
            {
                AppBarIconBtnImportar.IsEnabled = (importarBatidasViewModel.ImportarBatidasState == ImportarBatidasState.Importando);
            };
        }

        private void OnImportar(object sender, EventArgs e)
        {
            importarBatidasViewModel.Importar();
        }
    }
}