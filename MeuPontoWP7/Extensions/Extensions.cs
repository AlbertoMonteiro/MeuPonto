using MeuPonto.Common;
using MeuPonto.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MeuPontoWP7.Extensions
{
    public static class Extensions
    {
        public static TimeSpan Resumo(this IEnumerable<Batida> batidas)
        {
            var diferenca = TimeSpan.Zero;
            var batidaViewModels = batidas.ToList();
            
            if (batidaViewModels.Count == 1)
            {
                var batida = batidaViewModels.First();
                if (batida.Horario.Date == DateTime.Now.Date)
                {
                    var dia = batida.Horario.Date;
                    diferenca = DateTime.Now.Subtract(batida.Horario); 
                }
                else
                {
                    var dia = batida.Horario.Date;
                    diferenca = dia.Subtract(batida.Horario);
                }
            }
            if (batidaViewModels.Count > 1)
            {
                diferenca = batidaViewModels.Aggregate(TimeSpan.Zero, (tempo, batida) =>
                {
                    var diff = DateTime.Now.Subtract(batida.Horario);
                    return batida.NaturezaBatida == NaturezaBatida.Entrada ? tempo + diff : tempo - diff;
                });
            }
            return diferenca;
        }

        public static string RemoveBarras(this DateTime data)
        {
            return data.ToString("ddMMyyyy");
        }
    }
}
