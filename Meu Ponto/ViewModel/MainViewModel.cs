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
        private string _horario;
        private TimeSpan _horarioDeTrabalhoDiario;
        private readonly CacheContext _context;

        public MainViewModel()
        {
            _context = new CacheContext();
            
            Batidas = new ObservableCollection<BatidaViewModel>();

            foreach (var batida in _context.Batidas.Where(x => x.Horario.Date == DateTime.Now.Date))
            {
                Batidas.Add(new BatidaViewModel()
                {
                    Horario = batida.Horario,
                    Id = batida.Id,
                    NaturezaBatida = batida.NaturezaBatida
                });
            }

            var configuracao = _context.Configuracoes.FirstOrDefault() ?? new Configuracao();

            HorarioDeTrabalhoDiario = configuracao.HorarioDeTrabalhoDiario;

            AdicionarBatida = new RelayCommand(() =>
            {
                DateTime dateTime;

                if (!string.IsNullOrWhiteSpace(Horario))
                {
                    var timeSpan = TimeSpan.Parse(Horario);
                    dateTime = DateTime.Today.Add(timeSpan);
                }
                else
                    dateTime = DateTime.Now;

                var tipoBatida = Batidas.Count % 2 != 0 ? NaturezaBatida.Saida : NaturezaBatida.Entrada;
                var batidaViewModel = new BatidaViewModel
                {
                    Horario = dateTime, NaturezaBatida = tipoBatida
                };
                Batidas.Add(batidaViewModel);
                
                _context.Batidas.InsertOnSubmit(batidaViewModel);
                _context.SubmitChanges();
                
                if (AtualizaHorasTrabalhadas)
                    RaiseChangedHorarioTrabalhado();
            });

            RemoverBatida = new RelayCommand<BatidaViewModel>(b =>
            {
                Batidas.Remove(b);
                _context.Batidas.DeleteOnSubmit(b);
                _context.SubmitChanges();
            });

            Batidas.CollectionChanged += (sender, args) => RaisePropertyChanged("HorarioTrabalhado");

            if (IsInDesignModeStatic)
                CreateFakeData();

            if (AtualizaHorasTrabalhadas)
                RaiseChangedHorarioTrabalhado();
        }

        public bool AtualizaHorasTrabalhadas
        {
            get { return Batidas.Any() && Batidas.Last().NaturezaBatida == NaturezaBatida.Entrada; }
        }

        public string Horario
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
                    var configuracao = new Configuracao {HorarioDeTrabalhoDiario = value};
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
                    return diferenca.ToString(@"hh\:mm\:ss"); //string.Format("{0:hh:mm:ss}", diferenca);
                }
                if (Batidas.Count > 1)
                {
                    var timeSpan = Batidas.Aggregate(TimeSpan.Zero, (tempo, batida) =>
                    {
                        var diff = DateTime.Now.Subtract(batida.Horario);
                        return batida.NaturezaBatida == NaturezaBatida.Entrada ? tempo + diff : tempo - diff;
                    });
                    return timeSpan.ToString(@"hh\:mm\:ss"); //string.Format("{0:hh:mm:ss}", timeSpan);
                }
                if (AtualizaHorasTrabalhadas)
                    RaisePropertyChanged("HorarioTrabalhado");
                return "00:00:00";
            }
        }

        public ObservableCollection<BatidaViewModel> Batidas { get; set; }

        public RelayCommand AdicionarBatida { get; set; }

        public RelayCommand<BatidaViewModel> RemoverBatida { get; set; }

        private void CreateFakeData()
        {
            Batidas.Add(new BatidaViewModel
            {
                Horario = DateTime.Now,
                NaturezaBatida = NaturezaBatida.Entrada
            });
            Batidas.Add(new BatidaViewModel
            {
                Horario = DateTime.Now.AddHours(1),
                NaturezaBatida = NaturezaBatida.Saida
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
