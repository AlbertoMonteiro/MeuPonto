using MeuPonto.Common;
using MeuPontoWP7.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MeuPontoWP7.Extensions
{
    public static class Extensions
    {
        public static TimeSpan Resumo(this IEnumerable<BatidaViewModel> batidas)
        {
            var diferenca = TimeSpan.Zero;
            var batidaViewModels = batidas.ToList();
            
            if (batidaViewModels.Count == 1)
            {
                var batida = batidaViewModels.First();
                var dia = batida.Horario.Date;
                diferenca = dia.Subtract(batida.Horario);
            }
            if (batidaViewModels.Count > 1)
            {
                diferenca = batidaViewModels.Aggregate(TimeSpan.Zero, (tempo, batida) =>
                {
                    var diff = DateTime.Now.Subtract(batida.Horario);
                    return batida.Natureza == NaturezaBatida.Entrada ? tempo + diff : tempo - diff;
                });
            }
            return diferenca;
        }
    }
}
