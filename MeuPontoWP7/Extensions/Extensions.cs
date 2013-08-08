using System.IO;
using System.Text;
using MeuPonto.Common;
using MeuPonto.Common.Models;
using MeuPontoWP7.Services.Fortes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

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

        public static string ToMD5(this DateTime data)
        {
            var md5 = new System.Security.Cryptography.MD5();
            var bytes = Encoding.UTF8.GetBytes(data.ToString("ddMMyyyy"));
            var hash = md5.ComputeHash(bytes);
            return BitConverter.ToString(hash).Replace("-", string.Empty).ToLower();
        }

        public static IEnumerable<Batida> ToBatidas(this IEnumerable<Historico> historicos)
        {
            var regex = new Regex(@"Trabalho de (\d{2}:\d{2}) a (\d{2}:\d{2})");
            var contador = 0;
            return from historico in historicos
                   from informacao in historico.Informacoes
                   where regex.IsMatch(informacao.Descricao)
                   from @group in regex.Match(informacao.Descricao).Groups.Cast<Group>()
                   where @group.Length == 5
                   let horario = TimeSpan.Parse(@group.Value)
                   let batida = new Batida { Horario = historico.Data.Add(horario), NaturezaBatida = ((NaturezaBatida)(contador++ % 2)) }
                   select batida;
        }
    }
}
