using System;
using System.Windows.Media.Imaging;
using Meu_Ponto.Models;

namespace Meu_Ponto.ViewModel
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
            NaturezaBatida = naturezaBatida;
        }

        public int Id { get; set; }
        public DateTime Horario { get; set; }
        public NaturezaBatida NaturezaBatida { get; set; }
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