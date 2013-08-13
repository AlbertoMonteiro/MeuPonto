using System;
using System.ComponentModel;
using System.Data.Linq.Mapping;

namespace MeuPonto.Common.Models
{
    [Table]
    public class Configuracao : INotifyPropertyChanged
    {
        private TimeSpan horarioDeTrabalhoDiario;
        private TimeSpan horarioDeTrabalhoDiarioMaximo;
        private int id;
        private int minutosDeDiferenca;
        private TimeSpan quantidadeDeHorasDeAlmoco;
        private TimeSpan turnoMaximo;

        [Column(IsPrimaryKey = true, IsDbGenerated = true)]
        public int Id
        {
            get { return id; }
            set
            {
                id = value;
                OnPropertyChanged("Id");
            }
        }

        [Column(DbType = "NVARCHAR(10)")]
        public TimeSpan HorarioDeTrabalhoDiario
        {
            get { return horarioDeTrabalhoDiario; }
            set
            {
                horarioDeTrabalhoDiario = value;
                OnPropertyChanged("HorarioDeTrabalhoDiario");
            }
        }

        [Column(DbType = "NVARCHAR(10)")]
        public TimeSpan HorarioDeTrabalhoDiarioMaximo
        {
            get { return horarioDeTrabalhoDiarioMaximo; }
            set
            {
                horarioDeTrabalhoDiarioMaximo = value;
                OnPropertyChanged("HorarioDeTrabalhoDiarioMaximo");
            }
        }

        [Column(DbType = "NVARCHAR(10)")]
        public TimeSpan QuantidadeDeHorasDeAlmoco
        {
            get { return quantidadeDeHorasDeAlmoco; }
            set
            {
                quantidadeDeHorasDeAlmoco = value;
                OnPropertyChanged("QuantidadeDeHorasDeAlmoco");
            }
        }

        [Column]
        public int MinutosDeDiferenca
        {
            get { return minutosDeDiferenca; }
            set
            {
                minutosDeDiferenca = value;
                OnPropertyChanged("MinutosDeDiferenca");
            }
        }

        [Column(DbType = "NVARCHAR(10)")]
        public TimeSpan TurnoMaximo
        {
            get { return turnoMaximo; }
            set
            {
                turnoMaximo = value;
                OnPropertyChanged("TurnoMaximo");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}