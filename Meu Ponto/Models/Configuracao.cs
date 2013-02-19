﻿using System;
using System.Data.Linq.Mapping;

namespace Meu_Ponto.Models
{
    [Table]
    public class Configuracao
    {
        [Column(IsPrimaryKey = true)]
        public int Id { get; set; }

        [Column(DbType = "NVARCHAR(10)", IsVersion = true, UpdateCheck = UpdateCheck.Never)]
        public TimeSpan HorarioDeTrabalhoDiario { get; set; }

        [Column(DbType = "NVARCHAR(10)")]
        public TimeSpan QuantidadeDeHorasDeAlmoco { get; set; }

        [Column]
        public int MinutosDeDiferenca { get; set; }
    }
}
