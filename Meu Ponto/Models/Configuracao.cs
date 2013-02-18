using System;
using System.Data.Linq.Mapping;

namespace Meu_Ponto.Models
{
    [Table]
    public class Configuracao
    {
        [Column(IsPrimaryKey = true)]
        public int Id { get; set; }

        [Column(DbType = "NVARCHAR(10)")]
        public TimeSpan HorarioDeTrabalhoDiario { get; set; }
    }
}
