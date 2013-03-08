using System;

namespace MeuPontoWP7.ScheduledActions
{
    public class Lembrete
    {
        public TipoLembrete Tipo { get; set; }
        public DateTime Inicio { get; set; }
        public DateTime Fim { get; set; }
        public string Mensagem { get; set; }

        public string LembreteName
        {
            get
            {
                switch (Tipo)
                {
                    case TipoLembrete.TurnoMaximo:
                        return "MeuPontoTurnoMaximo";
                    case TipoLembrete.TempoMinimoDePausa:
                        return "MeuPontoTempoMinimoDePausa";
                    case TipoLembrete.JornadaMaxima:
                        return "MeuPontoJornadaMaxima";
                    default:
                        return "MeuPontoAgent";
                }
            }
        }
    }
}