using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using MeuPontoWP7.ViewModel;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace MeuPontoWP7.Views
{
    public partial class ImportarBatidas : PhoneApplicationPage
    {
        public ImportarBatidas()
        {
            InitializeComponent();
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            var importarBatidasViewModel = (ImportarBatidasViewModel) DataContext;
            importarBatidasViewModel.PropertyChanged += (o, args) =>
            {
                if (args.PropertyName == "Url")
                {
                    ImportWebBrowser.Navigate(new Uri(importarBatidasViewModel.Url, UriKind.RelativeOrAbsolute));
                }
            };
        }
    }
}