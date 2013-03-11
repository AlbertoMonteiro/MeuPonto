using System;
using System.Data.Linq.Mapping;

namespace MeuPonto.Common.Models
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