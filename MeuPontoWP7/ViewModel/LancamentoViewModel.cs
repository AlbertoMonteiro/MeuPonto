using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MeuPonto.Common;
using MeuPontoWP7.Models;
using MeuPontoWP7.Repositorios;
using Microsoft.Phone.Reactive;

namespace MeuPontoWP7.ViewModel
{
    public class LancamentoViewModel : ViewModelBase
    {
        private CacheContext _context;
        private DateTime? _horario;
        private Configuracao _configuracao;
        private DateTime? _dia;

        public LancamentoViewModel(IContextProvider repositorio)
        {
            _context = repositorio.CacheContext;
            Batidas = new ObservableCollection<BatidaViewModel>();

            if (IsInDesignMode)
            {
                // Code runs in Blend --> create design time data.
                Batidas.Add(new BatidaViewModel { Horario = new DateTime(2012,1,1,8,0,0), Natureza = NaturezaBatida.Entrada });
                Batidas.Add(new BatidaViewModel { Horario = new DateTime(2012,1,1,12,0,0), Natureza = NaturezaBatida.Saida });
                Batidas.Add(new BatidaViewModel { Horario = new DateTime(2012,1,1,13,0,0), Natureza = NaturezaBatida.Entrada });
                Batidas.Add(new BatidaViewModel { Horario = new DateTime(2012,1,1,18,0,0), Natureza = NaturezaBatida.Saida });
            }
            else
            {
                // Code runs "for real"

                _configuracao = _context.Configuracoes.FirstOrDefault();

                AdicionarBatida = new RelayCommand(AddBatida);
                RemoverBatida = new RelayCommand<BatidaViewModel>(RemoveBatida);
                Batidas.CollectionChanged += (sender, args) =>
                {
                    RaisePropertyChanged("HorarioTrabalhado");
                    RaisePropertyChanged("Resumo");
                };
            }
        }

        private void CarregaBatidas()
        {
            Batidas.Clear();
            _context.Batidas
                    .Where(x => x.Horario.Date == Dia.Value.Date)
                    .ToList()
                    .ForEach(batida => Batidas.Add(new BatidaViewModel(batida.Id, batida.Horario, batida.NaturezaBatida)));
        }

        public ObservableCollection<BatidaViewModel> Batidas { get; set; }

        public DateTime? Dia
        {
            get { return _dia; }
            set
            {
                _dia = value;
                RaisePropertyChanged("Dia");
                CarregaBatidas();
            }
        }

        public DateTime? Horario
        {
            get { return _horario; }
            set
            {
                _horario = value;
                RaisePropertyChanged("Horario");
            }
        }

        public string Resumo
        {
            get
            {
                var horarioRealizado = HorarioRealizado;
                string format;

                if (_configuracao.HorarioDeTrabalhoDiario < horarioRealizado)
                    format = "Crédito de {0}";
                else if (_configuracao.HorarioDeTrabalhoDiario > horarioRealizado)
                    format = "Débito de {0}";
                else
                    format = "Não houve crédito nem débito";

                var diff = horarioRealizado.Subtract(_configuracao.HorarioDeTrabalhoDiario).ToString(@"hh\:mm\:ss");

                return string.Format(format, diff);
            }
        }

        public bool AtualizaHorasTrabalhadas
        {
            get { return Batidas.Any() && Batidas.Last().Natureza == NaturezaBatida.Entrada; }
        }

        public string HorarioTrabalhado
        {
            get
            {
                var diferenca = HorarioRealizado;

                return diferenca.ToString(@"hh\:mm\:ss");
            }
        }

        private TimeSpan HorarioRealizado
        {
            get
            {
                var diferenca = TimeSpan.Zero;
                if (Batidas.Count == 1)
                {
                    var batida = Batidas.First();
                    diferenca = Dia.Value.Subtract(batida.Horario);
                }
                if (Batidas.Count > 1)
                {
                    diferenca = Batidas.Aggregate(TimeSpan.Zero, (tempo, batida) =>
                    {
                        var diff = DateTime.Now.Subtract(batida.Horario);
                        return batida.Natureza == NaturezaBatida.Entrada ? tempo + diff : tempo - diff;
                    });
                }
                return diferenca;
            }
        }

        public RelayCommand AdicionarBatida { get; set; }

        public RelayCommand<BatidaViewModel> RemoverBatida { get; set; }

        private void RemoveBatida(BatidaViewModel batidaViewModel)
        {
            if (batidaViewModel != null)
            {
                Batidas.Remove(batidaViewModel);
                var batida = _context.Batidas.Single(b => b.Id == batidaViewModel.Id);
                _context.Batidas.DeleteOnSubmit(batida);
                _context.SubmitChanges();
            }
        }

        private void AddBatida()
        {
            var tipoBatida = (NaturezaBatida)(Batidas.Count % 2);
            var batidaViewModel = new BatidaViewModel
            {
                Horario = Dia.Value.Date.Add(Horario.Value.TimeOfDay),
                Natureza = tipoBatida
            };
            Batidas.Add(batidaViewModel);

            Batida batida = batidaViewModel;

            _context.Batidas.InsertOnSubmit(batida);
            _context.SubmitChanges();

            batidaViewModel.Id = batida.Id;

            if (AtualizaHorasTrabalhadas)
                RaisePropertyChanged("HorarioTrabalhado");
        }
    }
}
