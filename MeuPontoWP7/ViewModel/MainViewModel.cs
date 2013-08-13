using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MeuPonto.Common;
using MeuPonto.Common.Models;
using MeuPonto.Common.Repositorios;
using MeuPontoWP7.Extensions;
using MeuPontoWP7.ScheduledActions;
using Microsoft.Phone.Reactive;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace MeuPontoWP7.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private readonly CacheContext context;
        private DateTime? horario;

        public MainViewModel(IContextProvider repositorio)
        {
            context = repositorio.CacheContext;
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

                context.Batidas
                    .Where(x => x.Horario.Date == DateTime.Now.Date)
                    .ToList()
                    .ForEach(Batidas.Add);

                CarregaConfiguracao();

                AdicionarBatida = new RelayCommand(AddBatida);
                RemoverBatida = new RelayCommand<Batida>(RemoveBatida);
                Batidas.CollectionChanged += (sender, args) =>
                {
                    RaiseChangedHorarioTrabalhado();
                    RegisterTasks();
                };

                if (AtualizaHorasTrabalhadas)
                    RaiseChangedHorarioTrabalhado();
            }
        }

        public ObservableCollection<Batida> Batidas { get; set; }

        public DateTime? Horario
        {
            get { return horario; }
            set
            {
                horario = value;
                RaisePropertyChanged("Horario");
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
                var resumo = Batidas.Resumo();
                return resumo.ToString(@"hh\:mm\:ss");
            }
        }

        public int DiaHoje
        {
            get { return DateTime.Now.Day; }
        }

        public Configuracao Configuracao { get; set; }

        public RelayCommand AdicionarBatida { get; set; }

        public RelayCommand<Batida> RemoverBatida { get; set; }

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

        private void RegisterTasks()
        {
            var actionScheduler = new ActionScheduler(Batidas, Configuracao);
            actionScheduler.Analize();
            actionScheduler.Schedule();
        }

        private void RemoveBatida(Batida batida)
        {
            if (batida != null)
            {
                Batidas.Remove(batida);
                context.Batidas.DeleteOnSubmit(batida);
                context.SubmitChanges();
            }
        }

        private void AddBatida()
        {
            var dateTime = Horario.HasValue ? Horario.Value : DateTime.Now;

            var tipoBatida = (NaturezaBatida)(Batidas.Count % 2);
            var batida = new Batida
            {
                Horario = dateTime,
                NaturezaBatida = tipoBatida
            };
            Batidas.Add(batida);

            context.Batidas.InsertOnSubmit(batida);
            context.SubmitChanges();

            if (AtualizaHorasTrabalhadas)
                RaiseChangedHorarioTrabalhado();
        }

        private void CarregaConfiguracao()
        {
            Configuracao = context.Configuracoes.FirstOrDefault() ?? new Configuracao();
            if (Configuracao.Id == 0)
            {
                context.Configuracoes.InsertOnSubmit(Configuracao);
                context.SubmitChanges();
            }

            Configuracao.PropertyChanged += (sender, args) => context.SubmitChanges();
        }
    }
}