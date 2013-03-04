using System;
using System.Data.Linq.Mapping;
using Meu_Ponto.ViewModel;
using MeuPonto.Common;

namespace Meu_Ponto.Models
{
    [Table]
    public class Batida
    {
        [Column(IsPrimaryKey = true,IsDbGenerated = true)]
        public int Id { get; set; }

        [Column(DbType = "Int")]
        public NaturezaBatida NaturezaBatida { get; set; }

        [Column]
        public DateTime Horario { get; set; }
    }
}