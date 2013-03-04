﻿using System;
using System.Data.Linq.Mapping;

namespace MeuPontoWP7.Models
{
    [Table]
    public class Configuracao
    {
        [Column(IsPrimaryKey = true)]
        public int Id { get; set; }

        [Column(DbType = "NVARCHAR(10)")]
        public TimeSpan HorarioDeTrabalhoDiario { get; set; }

        [Column(DbType = "NVARCHAR(10)")]
        public TimeSpan QuantidadeDeHorasDeAlmoco { get; set; }

        [Column]
        public int MinutosDeDiferenca { get; set; }

        [Column(DbType = "NVARCHAR(10)")]
        public TimeSpan TurnoMaximo { get; set; }
    }
}
