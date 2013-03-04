using System;
using System.Collections.ObjectModel;
using System.Linq;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MeuPonto.Common;
using MeuPontoWP7.Models;
using MeuPontoWP7.Repositorios;
using Microsoft.Phone.Reactive;

namespace MeuPontoWP7.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private readonly CacheContext _context;
        private int _diferencaEntreRelogioECelular;
        private DateTime? _horario;
        private TimeSpan _horarioDeTrabalhoDiario;
        private TimeSpan _tempoDoAlmoco;
        private TimeSpan _turnoMaximo;
        private Configuracao _configuracao;

        public MainViewModel(IContextProvider repositorio)
        {
            _context = repositorio.CacheContext;
            //_repositorio = repositorio;
            Batidas = new ObservableCollection<BatidaViewModel>();

            if (IsInDesignMode)
            {
                // Code runs in Blend --> create design time data.
                Batidas.Add(new BatidaViewModel { Horario = DateTime.Now.AddHours(-3), Natureza = NaturezaBatida.Entrada });
                Batidas.Add(new BatidaViewModel { Horario = DateTime.Now.AddHours(-2), Natureza = NaturezaBatida.Saida });
                Batidas.Add(new BatidaViewModel { Horario = DateTime.Now.AddHours(-1), Natureza = NaturezaBatida.Entrada });
            }
            else
            {
                // Code runs "for real"

                CarregaConfiguracao();

                AdicionarBatida = new RelayCommand(() =>
                    {
                        var dateTime = Horario.HasValue ? Horario.Value : DateTime.Now;

                        var tipoBatida = (NaturezaBatida)(Batidas.Count % 2);
                        var batidaViewModel = new BatidaViewModel
                            {
                                Horario = dateTime,
                                Natureza = tipoBatida
                            };
                        Batidas.Add(batidaViewModel);

                        Batida batida = batidaViewModel;

                        _context.Batidas.InsertOnSubmit(batida);
                        _context.SubmitChanges();

                        batidaViewModel.Id = batida.Id;

                        if (AtualizaHorasTrabalhadas)
                            RaiseChangedHorarioTrabalhado();
                    });

                RemoverBatida = new RelayCommand<BatidaViewModel>(batidaViewModel =>
                    {
                        if (batidaViewModel != null)
                        {
                            Batidas.Remove(batidaViewModel);
                            var batida = _context.Batidas.Single(b => b.Id == batidaViewModel.Id);
                            _context.Batidas.DeleteOnSubmit(batida);
                            _context.SubmitChanges();
                        }
                    });

                Batidas.CollectionChanged += (sender, args) => RaiseChangedHorarioTrabalhado();
            }
        }

        private void CarregaConfiguracao()
        {
            _configuracao = _context.Configuracoes.FirstOrDefault() ?? new Configuracao();
            _context.Configuracoes.InsertOnSubmit(_configuracao);
            _context.SubmitChanges();

            DiferencaPonto = _configuracao.MinutosDeDiferenca;
            TempoDiario = _configuracao.HorarioDeTrabalhoDiario;
            TempoIntervalo = _configuracao.QuantidadeDeHorasDeAlmoco;
            TurnoMaximo = _configuracao.TurnoMaximo;
        }

        public ObservableCollection<BatidaViewModel> Batidas { get; set; }

        public DateTime? Horario
        {
            get { return _horario; }
            set
            {
                _horario = value;
                RaisePropertyChanged("Horario");
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
                if (Batidas.Count == 1)
                {
                    BatidaViewModel batida = Batidas.First();
                    TimeSpan diferenca = DateTime.Now.Subtract(batida.Horario);
                    return diferenca.ToString(@"hh\:mm\:ss");
                }
                if (Batidas.Count > 1)
                {
                    var timeSpan = Batidas.Aggregate(TimeSpan.Zero, (tempo, batida) =>
                        {
                            var diff = DateTime.Now.Subtract(batida.Horario);
                            return batida.Natureza == NaturezaBatida.Entrada ? tempo + diff : tempo - diff;
                        });
                    return timeSpan.ToString(@"hh\:mm\:ss");
                }
                if (AtualizaHorasTrabalhadas)
                    RaisePropertyChanged("HorarioTrabalhado");
                return "00:00:00";
            }
        }

        public int DiaHoje
        {
            get { return DateTime.Now.Day; }
        }

        public TimeSpan TempoDiario
        {
            get { return _horarioDeTrabalhoDiario; }
            set
            {
                _horarioDeTrabalhoDiario = value;
                RaisePropertyChanged("TempoDiario");
                _configuracao.HorarioDeTrabalhoDiario = value;
                _context.SubmitChanges();
            }
        }

        public TimeSpan TempoIntervalo
        {
            get { return _tempoDoAlmoco; }
            set
            {
                _tempoDoAlmoco = value;
                RaisePropertyChanged("TempoIntervalo");
                _configuracao.QuantidadeDeHorasDeAlmoco = value;
                _context.SubmitChanges();
            }
        }

        public int DiferencaPonto
        {
            get { return _diferencaEntreRelogioECelular; }
            set
            {
                _diferencaEntreRelogioECelular = value;
                RaisePropertyChanged("DiferencaPonto");
                _configuracao.MinutosDeDiferenca = value;
                _context.SubmitChanges();
            }
        }

        public TimeSpan TurnoMaximo
        {
            get { return _turnoMaximo; }
            set
            {
                _turnoMaximo = value;
                RaisePropertyChanged("TurnoMaximo");
                _configuracao.TurnoMaximo = value;
                _context.SubmitChanges();
            }
        }

        public RelayCommand AdicionarBatida { get; set; }

        public RelayCommand<BatidaViewModel> RemoverBatida { get; set; }

        private void RaiseChangedHorarioTrabalhado()
        {
            IScheduler scheduler = Scheduler.NewThread;
            scheduler.Schedule(() =>
                {
                    if (AtualizaHorasTrabalhadas)
                    {
                        Scheduler.Dispatcher.Dispatcher.BeginInvoke(() => RaisePropertyChanged("HorarioTrabalhado"));
                        RaiseChangedHorarioTrabalhado();
                    }
                }, TimeSpan.FromSeconds(1));
        }
    }
}