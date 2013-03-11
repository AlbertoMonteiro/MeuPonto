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