using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using MeuPonto.Common;
using MeuPonto.Common.Models;
using MeuPonto.Common.Repositorios;
using System.Linq;
using MeuPontoWP7.Extensions;

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

                var groups = batidas.ToKeyGroup(item => item.Horario.Date.ToString("dd/MM/yyyy"));

                Batidas = new ObservableCollection<KeyGroup<Batida>>(groups);
            }
            else
            {
                _cacheContext = repositorio.CacheContext;
                // Code runs in Blend --> create design time data.
                Batidas = new ObservableCollection<KeyGroup<Batida>>();

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
                var groups = batidas.ToKeyGroup(item => item.Horario.Date.ToString("dd/MM/yyyy"));

                Batidas.Clear();
                groups.ForEach(Batidas.Add);
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

        public ObservableCollection<KeyGroup<Batida>> Batidas { get; set; }
    }
}