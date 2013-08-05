using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Navigation;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MeuPonto.Common.Repositorios;
using System.Linq;

namespace MeuPontoWP7.ViewModel
{
    public class ImportarBatidasViewModel : ViewModelBase
    {
        private readonly IContextProvider repositorio;
        private string url;
        private string htmlString;
        Regex regex = new Regex(@"Trabalho de (\d{2}:\d{2}) a (\d{2}:\d{2})");

        public ImportarBatidasViewModel(IContextProvider repositorio)
        {
            this.repositorio = repositorio;
            if (IsInDesignMode)
            {

            }
            else
            {
                Url = @"http://10.1.254.31:8089";
                OnIr = new RelayCommand(Ir);
                OnNavegado = new RelayCommand<NavigationEventArgs>(Navegado);
            }
        }

        private void Navegado(NavigationEventArgs obj)
        {
            var htmlDocument = new HtmlAgilityPack.HtmlDocument();
            if (HtmlString != null)
            {
                htmlDocument.LoadHtml(HtmlString);

                var documentNode = htmlDocument.DocumentNode;
                var datas = documentNode.SelectNodes(@"//*[@id=""table-custom-2""]/tbody/tr/td[2]/a");
                if (datas != null)
                    foreach (var node in datas)
                    {
                        var cultureInfo = new CultureInfo("pt-BR");
                        var data = DateTime.Parse(node.InnerText, cultureInfo.DateTimeFormat);
                        var idPopup = node.Attributes["href"].Value.Remove(0, 1);

                        var horarios = documentNode.SelectNodes(string.Format(@"//*[@id=""{0}""]//tbody/tr", idPopup));
                        foreach (var horario in horarios)
                        {
                            var match = regex.Match(horario.InnerText);
                            if (match.Success)
                            {
                                var value = match.Groups.Cast<Group>().LastOrDefault().Value;
                                MessageBox.Show(data.ToShortDateString() + value);
                            }
                        }
                    }
            }
        }

        private void Ir()
        {
            //Url = new Uri(Url, UriKind.RelativeOrAbsolute);
        }

        public string Url
        {
            get { return url; }
            set
            {
                url = value;
                RaisePropertyChanged("Url");
                RaisePropertyChanged("Uri");
            }
        }

        public Uri Uri
        {
            get { return new Uri(Url, UriKind.RelativeOrAbsolute); }
        }

        public string HtmlString
        {
            get { return htmlString; }
            set
            {
                htmlString = value;
                RaisePropertyChanged("HtmlString");
            }
        }

        public RelayCommand OnIr { get; set; }

        public RelayCommand<NavigationEventArgs> OnNavegado { get; set; }
    }
}