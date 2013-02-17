using System;
using System.Collections.ObjectModel;
using System.Linq;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Phone.Reactive;

namespace Meu_Ponto.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private string _horario;

        public MainViewModel()
        {
            Batidas = new ObservableCollection<Batida>();

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

                var tipoBatida = Batidas.Count%2 != 0 ? NaturezaEntrada.Saida : NaturezaEntrada.Entrada;

                Batidas.Add(new Batida
                {
                    Horario = dateTime,
                    NaturezaEntrada = tipoBatida
                });
                
                if (AtualizaHorasTrabalhadas)
                    RaiseChangedHorarioTrabalhado();
            });

            RemoverBatida = new RelayCommand<Batida>(b => Batidas.Remove(b));

            Batidas.CollectionChanged += (sender, args) => RaisePropertyChanged("HorarioTrabalhado");

            if (IsInDesignModeStatic)
                CreateFakeData();
        }

        public bool AtualizaHorasTrabalhadas
        {
            get { return Batidas.Any() && Batidas.Last().NaturezaEntrada == NaturezaEntrada.Entrada; }
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
                        return batida.NaturezaEntrada == NaturezaEntrada.Entrada ? tempo + diff : tempo - diff;
                    });
                    return timeSpan.ToString(@"hh\:mm\:ss"); //string.Format("{0:hh:mm:ss}", timeSpan);
                }
                if (AtualizaHorasTrabalhadas)
                    RaisePropertyChanged("HorarioTrabalhado");
                return "00:00:00";
            }
        }

        public ObservableCollection<Batida> Batidas { get; set; }

        public RelayCommand AdicionarBatida { get; set; }

        public RelayCommand<Batida> RemoverBatida { get; set; }

        private void CreateFakeData()
        {
            Batidas.Add(new Batida
            {
                Horario = DateTime.Now,
                NaturezaEntrada = NaturezaEntrada.Entrada
            });
            Batidas.Add(new Batida
            {
                Horario = DateTime.Now.AddHours(1),
                NaturezaEntrada = NaturezaEntrada.Saida
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