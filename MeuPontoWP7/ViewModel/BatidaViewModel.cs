using System;
using MeuPonto.Common;
using MeuPonto.Common.Models;

namespace MeuPontoWP7.ViewModel
{
    public class BatidaViewModel
    {
        public BatidaViewModel()
        {
        }

        public BatidaViewModel(int id, DateTime horario, NaturezaBatida naturezaBatida)
        {
            Id = id;
            Horario = horario;
            Natureza = naturezaBatida;
        }

        public BatidaViewModel(Batida batida)
        {
            Id = batida.Id;
            Horario = batida.Horario;
            Natureza = batida.NaturezaBatida;
        }

        public int Id { get; set; }
        public DateTime Horario { get; set; }
        public NaturezaBatida Natureza { get; set; }

        public static implicit operator Batida(BatidaViewModel batida)
        {
            return new Batida
                {
                    Id = batida.Id,
                    Horario = batida.Horario,
                    NaturezaBatida = batida.Natureza
                };
        }
    }
}