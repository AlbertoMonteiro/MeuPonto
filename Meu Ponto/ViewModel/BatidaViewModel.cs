using System;
using Meu_Ponto.Models;

namespace Meu_Ponto.ViewModel
{
    public class BatidaViewModel
    {
        public int Id { get; set; }
        public DateTime Horario { get; set; }
        public NaturezaBatida NaturezaBatida { get; set; }

        public static implicit operator Batida(BatidaViewModel batida)
        {
            return new Batida
            {
                Id = batida.Id,
                Horario = batida.Horario,
                NaturezaBatida = batida.NaturezaBatida
            };
        }
    }
}