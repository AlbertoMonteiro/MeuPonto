using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Meu_Ponto.Data;
using Meu_Ponto.Models;
using Microsoft.Phone.Reactive;

namespace Meu_Ponto.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private DateTime? _horario;
        private TimeSpan _horarioDeTrabalhoDiario;
        private readonly CacheContext _context;
        private int _diferencaEntreRelogioECelular;
        private int _tempoDoAlmoco;
        private readonly int _diaHoje;

        public MainViewModel()
        {
            Batidas = new ObservableCollection<BatidaViewModel>();

            if (IsInDesignModeStatic)
                CreateFakeData();
            else
            {
                _diaHoje = DateTime.Now.Day;

                _context = new CacheContext();

                if (_context != null && _context.Batidas != null)
                    foreach (var batida in _context.Batidas.Where(x => x.Horario.Date == DateTime.Now.Date))
                        Batidas.Add(new BatidaViewModel(batida.Id, batida.Horario, batida.NaturezaBatida));

                var configuracao = _context.Configuracoes.FirstOrDefault() ?? new Configuracao();

                HorarioDeTrabalhoDiario = configuracao.HorarioDeTrabalhoDiario;
                DiferencaEntreRelogioECelular = configuracao.DiferencaEntreRelogioECelular;
                TempoDoAlmoco = configuracao.TempoDoAlmoco;

                AdicionarBatida = new RelayCommand(() =>
                {
                    var dateTime = Horario.HasValue ? Horario.Value : DateTime.Now;

                    var tipoBatida = Batidas.Count % 2 != 0 ? NaturezaBatida.Saida : NaturezaBatida.Entrada;
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
                    Batidas.Remove(batidaViewModel);

                    var batida = _context.Batidas.Single(b => b.Id == batidaViewModel.Id);
                    _context.Batidas.DeleteOnSubmit(batida);
                    _context.SubmitChanges();
                });

                Batidas.CollectionChanged += (sender, args) => RaisePropertyChanged("HorarioTrabalhado");

                if (AtualizaHorasTrabalhadas)
                    RaiseChangedHorarioTrabalhado();
            }
        }

        public bool AtualizaHorasTrabalhadas
        {
            get { return Batidas.Any() && Batidas.Last().Natureza == NaturezaBatida.Entrada; }
        }

        public DateTime? Horario
        {
            get { return _horario; }
            set
            {
                _horario = value;
                RaisePropertyChanged("Horarios");
            }
        }

        public TimeSpan HorarioDeTrabalhoDiario
        {
            get { return _horarioDeTrabalhoDiario; }
            set
            {
                _horarioDeTrabalhoDiario = value;
                RaisePropertyChanged("HorarioDeTrabalhoDiario");

                if (_context.Configuracoes.Any())
                    _context.Configuracoes.First().HorarioDeTrabalhoDiario = value;
                else
                {
                    var configuracao = new Configuracao { HorarioDeTrabalhoDiario = value };
                    _context.Configuracoes.InsertOnSubmit(configuracao);
                }

                _context.SubmitChanges();
            }
        }

        public int TempoDoAlmoco
        {
            get { return _tempoDoAlmoco; }
            set
            {
                _tempoDoAlmoco = value;
                RaisePropertyChanged("TempoDoAlmoco");

                if (_context.Configuracoes.Any())
                    _context.Configuracoes.First().TempoDoAlmoco = value;
                else
                {
                    var configuracao = new Configuracao { TempoDoAlmoco = value };
                    _context.Configuracoes.InsertOnSubmit(configuracao);
                }

                _context.SubmitChanges();
            }
        }

        public int DiferencaEntreRelogioECelular
        {
            get { return _diferencaEntreRelogioECelular; }
            set
            {
                _diferencaEntreRelogioECelular = value;
                RaisePropertyChanged("DiferencaEntreRelogioECelular");

                if (_context.Configuracoes.Any())
                    _context.Configuracoes.First().DiferencaEntreRelogioECelular = value;
                else
                {
                    var configuracao = new Configuracao { DiferencaEntreRelogioECelular = value };
                    _context.Configuracoes.InsertOnSubmit(configuracao);
                }

                _context.SubmitChanges();
            }
        }

        public string HorarioTrabalhado
        {
            get
            {
                if (Batidas.Count == 1)
                {
                    var batida = Batidas.First();
                    var diferenca = DateTime.Now.Subtract(batida.Horario);
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
            get { return _diaHoje; }
        }

        public ObservableCollection<BatidaViewModel> Batidas { get; set; }

        public RelayCommand AdicionarBatida { get; set; }

        public RelayCommand<BatidaViewModel> RemoverBatida { get; set; }

        private void CreateFakeData()
        {
            Batidas.Add(new BatidaViewModel
            {
                Horario = DateTime.Now,
                Natureza = NaturezaBatida.Entrada
            });
            Batidas.Add(new BatidaViewModel
            {
                Horario = DateTime.Now.AddHours(1),
                Natureza = NaturezaBatida.Saida
            });
        }

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
