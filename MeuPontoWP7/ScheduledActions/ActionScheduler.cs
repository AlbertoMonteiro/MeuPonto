using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using MeuPonto.Common;
using MeuPonto.Common.Models;
using Microsoft.Phone.Scheduler;

namespace MeuPontoWP7.ScheduledActions
{
    public class ActionScheduler
    {
        private readonly IEnumerable<Batida> _batidas;
        private readonly Configuracao _configuracao;
        private readonly List<Lembrete> _lembretes;

        public ActionScheduler(IEnumerable<Batida> batidas, Configuracao configuracao)
        {
            _batidas = batidas;
            _configuracao = configuracao;
            _lembretes = new List<Lembrete>(2);
        }

        public void Analize()
        {
            if (_batidas.Count() == 1)
                TurnoMaximo();
            else if (_batidas.Count() > 1 && _batidas.Count()%2 == 0)
                TempoDeIntervalo();
            else if (_batidas.Count() > 1 && _batidas.Count()%2 == 0)
            {
                TurnoMaximo();
                JornadaDiariaTotal();
            }
        }

        private void JornadaDiariaTotal()
        {
            var timeSpan = _batidas.Aggregate(TimeSpan.Zero, (tempo, batida) =>
            {
                var diff = DateTime.Now.Subtract(batida.Horario);
                return batida.NaturezaBatida == NaturezaBatida.Entrada ? tempo + diff : tempo - diff;
            });

            var horarioJornadaMaxima = _batidas.First().Horario.Add(timeSpan);
            _lembretes.Add(new Lembrete
            {
                Tipo = TipoLembrete.JornadaMaxima,
                Mensagem = string.Format("Aproximadamente 5 minutos para atingir jornada máxima.\nJornada máxima completa as {0:HH:mm:ss}.", horarioJornadaMaxima),
                Inicio = horarioJornadaMaxima.Add(TimeSpan.FromMinutes(-5)),
                Fim = horarioJornadaMaxima.Add(TimeSpan.FromMinutes(5))
            });
        }

        private void TempoDeIntervalo()
        {
            var batida = _batidas.Last();
            var horarioFimTempoIntervalo = batida.Horario.Add(_configuracao.QuantidadeDeHorasDeAlmoco);

            _lembretes.Add(new Lembrete
            {
                Tipo = TipoLembrete.TempoMinimoDePausa,
                Mensagem = string.Format("Aproximadamente 5 minutos para atingir tempo de pausa.\nTempo de pausa completa as {0:HH:mm:ss}.", horarioFimTempoIntervalo),
                Inicio = horarioFimTempoIntervalo.Add(TimeSpan.FromMinutes(-5)),
                Fim = horarioFimTempoIntervalo.Add(TimeSpan.FromMinutes(5))
            });
        }

        private void TurnoMaximo()
        {
            var batida = _batidas.First();
            var horarioTurnoMaximo = batida.Horario.Add(_configuracao.TurnoMaximo);

            _lembretes.Add(new Lembrete
            {
                Tipo = TipoLembrete.TurnoMaximo,
                Mensagem = string.Format("Aproximadamente 5 minutos para turno máximo.\nTurno maximo expira as {0:HH:mm:ss}.", horarioTurnoMaximo),
                Inicio = horarioTurnoMaximo.Add(TimeSpan.FromMinutes(-5)),
                Fim = horarioTurnoMaximo.Add(TimeSpan.FromMinutes(5))
            });
        }

        public void Schedule()
        {
            foreach (var action in ScheduledActionService.GetActions<Reminder>())
                ScheduledActionService.Remove(action.Name);

            foreach (var lembrete in _lembretes.Where(lem => lem.Inicio > DateTime.Now))
            {
                var periodicTask = ScheduledActionService.Find(lembrete.LembreteName) as Reminder;

                try
                {
                    if (periodicTask != null)
                        ScheduledActionService.Remove(lembrete.LembreteName);

                    periodicTask = new Reminder(lembrete.LembreteName)
                    {
                        Title = "Meu Ponto",
                        Content = lembrete.Mensagem,
                        BeginTime = lembrete.Inicio,
                        ExpirationTime = lembrete.Fim,
                        RecurrenceType = RecurrenceInterval.None
                    };
                    ScheduledActionService.Add(periodicTask);
                }
                catch (InvalidOperationException exception)
                {
                    if (exception.Message.Contains("BNS Error: The action is disabled"))
                    {
                        MessageBox.Show("Background agents for this application have been disabled by the user.");
                    }

                    if (exception.Message.Contains("BNS Error: The maximum number of ScheduledActions of this type have already been added."))
                    {
                        // No user action required. The system prompts the user when the hard limit of periodic tasks has been reached.
                    }
                }
                catch (SchedulerServiceException ex)
                {
                    // No user action required.
                    MessageBox.Show(ex.Message);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}