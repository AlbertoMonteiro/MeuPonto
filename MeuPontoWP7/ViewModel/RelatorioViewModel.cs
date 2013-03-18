using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using MeuPonto.Common;
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
                var batidas = new List<BatidaViewModel>();
                batidas.Add(new BatidaViewModel { Horario = new DateTime(2012, 1, 1, 8, 0, 0), Natureza = NaturezaBatida.Entrada });
                batidas.Add(new BatidaViewModel { Horario = new DateTime(2012, 1, 1, 12, 0, 0), Natureza = NaturezaBatida.Saida });
                batidas.Add(new BatidaViewModel { Horario = new DateTime(2012, 1, 1, 13, 0, 0), Natureza = NaturezaBatida.Entrada });
                batidas.Add(new BatidaViewModel { Horario = new DateTime(2012, 1, 1, 18, 0, 0), Natureza = NaturezaBatida.Saida });
                
                batidas.Add(new BatidaViewModel { Horario = new DateTime(2012, 2, 1, 8, 0, 0), Natureza = NaturezaBatida.Entrada });
                batidas.Add(new BatidaViewModel { Horario = new DateTime(2012, 2, 1, 12, 0, 0), Natureza = NaturezaBatida.Saida });
                batidas.Add(new BatidaViewModel { Horario = new DateTime(2012, 2, 1, 13, 0, 0), Natureza = NaturezaBatida.Entrada });
                batidas.Add(new BatidaViewModel { Horario = new DateTime(2012, 2, 1, 17, 0, 0), Natureza = NaturezaBatida.Saida });

                Batidas = new ObservableCollection<Group<BatidaViewModel>>(batidas.GroupBy(x => x.Horario.Date.ToString("dd/MM/yyyy")).Select(x => new Group<BatidaViewModel>(x.Key.ToString(), x)));
            }
            else
            {
                _cacheContext = repositorio.CacheContext;
                // Code runs in Blend --> create design time data.
                Batidas = new ObservableCollection<Group<BatidaViewModel>>();

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
                                          .Select(batidasAgrupadas => new Group<BatidaViewModel>(batidasAgrupadas.Key.ToString("dd/MM/yyyy"), batidasAgrupadas.Select(b => new BatidaViewModel(b))))
                                          .ToList();
                Batidas.Clear();
                agrupamentos.ForEach(batidaViewModels => Batidas.Add(batidaViewModels));
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

        public ObservableCollection<Group<BatidaViewModel>> Batidas { get; set; }
    }
}