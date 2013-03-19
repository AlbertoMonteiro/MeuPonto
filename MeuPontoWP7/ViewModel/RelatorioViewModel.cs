using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using MeuPonto.Common;
using MeuPonto.Common.Models;
using MeuPonto.Common.Repositorios;
using System.Linq;

namespace MeuPontoWP7.ViewModel
{
    public class RelatorioViewModel : ViewModelBase
    {
        private CacheContext _cacheContext;
        private DateTime? _de;
        private DateTime? _ate;

        public RelatorioViewModel(IContextProvider repositorio)
        {
            if (IsInDesignMode)
            {
                // Code runs in Blend --> create design time data.
                var batidas = new List<Batida>();
                batidas.Add(new Batida { Horario = new DateTime(2012, 1, 1, 08, 0, 0), NaturezaBatida = NaturezaBatida.Entrada });
                batidas.Add(new Batida { Horario = new DateTime(2012, 1, 1, 12, 0, 0), NaturezaBatida = NaturezaBatida.Saida });
                batidas.Add(new Batida { Horario = new DateTime(2012, 1, 1, 13, 0, 0), NaturezaBatida = NaturezaBatida.Entrada });
                batidas.Add(new Batida { Horario = new DateTime(2012, 1, 1, 18, 0, 0), NaturezaBatida = NaturezaBatida.Saida });

                batidas.Add(new Batida { Horario = new DateTime(2012, 2, 1, 08, 0, 0), NaturezaBatida = NaturezaBatida.Entrada });
                batidas.Add(new Batida { Horario = new DateTime(2012, 2, 1, 12, 0, 0), NaturezaBatida = NaturezaBatida.Saida });
                batidas.Add(new Batida { Horario = new DateTime(2012, 2, 1, 13, 0, 0), NaturezaBatida = NaturezaBatida.Entrada });
                batidas.Add(new Batida { Horario = new DateTime(2012, 2, 1, 17, 0, 0), NaturezaBatida = NaturezaBatida.Saida });

                var batidasAgrupadas = batidas.
                    GroupBy(x => x.Horario.Date.ToString("dd/MM/yyyy")).
                    Select(x => new Group<Batida>(x.Key.ToString(), x));

                Batidas = new ObservableCollection<Group<Batida>>(batidasAgrupadas);
            }
            else
            {
                _cacheContext = repositorio.CacheContext;
                // Code runs in Blend --> create design time data.
                Batidas = new ObservableCollection<Group<Batida>>();

                PropertyChanged += (sender, args) =>
                {
                    if (args.PropertyName == "De" || args.PropertyName == "Ate")
                        AtualizaBatidas();
                };
            }
        }

        private void AtualizaBatidas()
        {
            if (De.HasValue && Ate.HasValue)
            {
                var batidas = _cacheContext.Batidas.Where(batida => De.Value.Date >= batida.Horario.Date && batida.Horario.Date <= Ate.Value.Date).ToList();
                var agrupamentos = batidas.GroupBy(bat => bat.Horario.Date)
                                          .Select(batidasAgrupadas => new Group<Batida>(batidasAgrupadas.Key.ToString("dd/MM/yyyy"), batidasAgrupadas))
                                          .ToList();
                Batidas.Clear();
                agrupamentos.ForEach(Batidas.Add);
            }
        }

        public DateTime? De
        {
            get { return _de; }
            set
            {
                _de = value;
                RaisePropertyChanged("De");
            }
        }

        public DateTime? Ate
        {
            get { return _ate; }
            set
            {
                _ate = value;
                RaisePropertyChanged("Ate");
            }
        }

        public ObservableCollection<Group<Batida>> Batidas { get; set; }
    }
}