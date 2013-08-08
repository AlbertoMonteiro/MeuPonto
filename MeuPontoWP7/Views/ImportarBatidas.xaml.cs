using System;
using System.Windows;
using MeuPontoWP7.ViewModel;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

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
            importarBatidasViewModel.PropertyChanged += (o, args) =>
            {
                var button = this.ApplicationBar.Buttons[0] as ApplicationBarIconButton;
                button.IsEnabled = (importarBatidasViewModel.ImportarBatidasState == ImportarBatidasState.Importando);
            };
        }

        private void OnImportar(object sender, EventArgs e)
        {
            importarBatidasViewModel.Importar();
        }
    }
}