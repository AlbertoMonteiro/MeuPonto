using System;
using System.ComponentModel;
using System.Data.Linq.Mapping;

namespace MeuPonto.Common.Models
{
    [Table]
    public class Configuracao : INotifyPropertyChanged
    {
        private TimeSpan _turnoMaximo;
        private int _minutosDeDiferenca;
        private TimeSpan _quantidadeDeHorasDeAlmoco;
        private TimeSpan _horarioDeTrabalhoDiario;
        private int _id;

        [Column(IsPrimaryKey = true, IsDbGenerated = true)]
        public int Id
        {
            get { return _id; }
            set
            {
                _id = value;
                OnPropertyChanged("Id");
            }
        }

        [Column(DbType = "NVARCHAR(10)")]
        public TimeSpan HorarioDeTrabalhoDiario
        {
            get { return _horarioDeTrabalhoDiario; }
            set
            {
                _horarioDeTrabalhoDiario = value;
                OnPropertyChanged("HorarioDeTrabalhoDiario");
            }
        }

        [Column(DbType = "NVARCHAR(10)")]
        public TimeSpan QuantidadeDeHorasDeAlmoco
        {
            get { return _quantidadeDeHorasDeAlmoco; }
            set
            {
                _quantidadeDeHorasDeAlmoco = value;
                OnPropertyChanged("QuantidadeDeHorasDeAlmoco");
            }
        }

        [Column]
        public int MinutosDeDiferenca
        {
            get { return _minutosDeDiferenca; }
            set
            {
                _minutosDeDiferenca = value;
                OnPropertyChanged("MinutosDeDiferenca");
            }
        }

        [Column(DbType = "NVARCHAR(10)")]
        public TimeSpan TurnoMaximo
        {
            get { return _turnoMaximo; }
            set
            {
                _turnoMaximo = value;
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
