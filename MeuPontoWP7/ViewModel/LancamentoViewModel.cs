using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MeuPonto.Common;
using MeuPonto.Common.Models;
using MeuPonto.Common.Repositorios;
using MeuPontoWP7.Extensions;

namespace MeuPontoWP7.ViewModel
{
    public class LancamentoViewModel : ViewModelBase
    {
        private readonly CacheContext _context;
        private DateTime? _horario;
        private readonly Configuracao _configuracao;
        private DateTime? _dia;
        private double _width;

        public LancamentoViewModel(IContextProvider repositorio)
        {
            _context = repositorio.CacheContext;
            Batidas = new ObservableCollection<Batida>();

            if (IsInDesignMode)
            {
                // Code runs in Blend --> create design time data.

                Batidas.Add(new Batida { Horario = new DateTime(2012, 1, 1, 08, 0, 0), NaturezaBatida = NaturezaBatida.Entrada });
                Batidas.Add(new Batida { Horario = new DateTime(2012, 1, 1, 12, 0, 0), NaturezaBatida = NaturezaBatida.Saida });
                Batidas.Add(new Batida { Horario = new DateTime(2012, 1, 1, 13, 0, 0), NaturezaBatida = NaturezaBatida.Entrada });
                Batidas.Add(new Batida { Horario = new DateTime(2012, 1, 1, 18, 0, 0), NaturezaBatida = NaturezaBatida.Saida });
            }
            else
            {
                // Code runs "for real"
                _configuracao = _context.Configuracoes.FirstOrDefault();

                AdicionarBatida = new RelayCommand(AddBatida);
                RemoverBatida = new RelayCommand<Batida>(RemoveBatida);

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
            if (_context != null)
            {
                _context.Batidas
                        .Where(x => x.Horario.Date == Dia.Value.Date)
                        .ToList()
                        .ForEach(Batidas.Add);
            }
        }

        public ObservableCollection<Batida> Batidas { get; set; }

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
                var horarioRealizado = Batidas.Resumo();
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
            get { return Batidas.Any() && Batidas.Last().NaturezaBatida == NaturezaBatida.Entrada; }
        }

        public string HorarioTrabalhado
        {
            get
            {
                var diferenca = Batidas.Resumo();

                return diferenca.ToString(@"hh\:mm\:ss");
            }
        }

        public Visibility DiaSelecionado
        {
            get { return Dia.HasValue ? Visibility.Visible : Visibility.Collapsed; }
        }

        public RelayCommand AdicionarBatida { get; set; }

        public RelayCommand<Batida> RemoverBatida { get; set; }

        private void RemoveBatida(Batida batidaViewModel)
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
            var batida = new Batida
            {
                Horario = Dia.Value.Date.Add(Horario.Value.TimeOfDay),
                NaturezaBatida = tipoBatida
            };
            Batidas.Add(batida);

            _context.Batidas.InsertOnSubmit(batida);
            _context.SubmitChanges();

            batida.Id = batida.Id;

            if (AtualizaHorasTrabalhadas)
                RaisePropertyChanged("HorarioTrabalhado");
        }
    }
}
