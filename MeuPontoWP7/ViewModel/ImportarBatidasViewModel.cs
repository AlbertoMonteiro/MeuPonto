using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MeuPonto.Common.Models;
using MeuPonto.Common.Repositorios;
using MeuPontoWP7.Extensions;
using MeuPontoWP7.Services.Fortes.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MeuPontoWP7.ViewModel
{
    public class ImportarBatidasViewModel : ViewModelBase
    {
        #region Fields
        private readonly IContextProvider repositorio;
        private DateTime dataFinal;
        private DateTime dataInicial;
        private Empresa empresaSelecionada;
        private ImportarBatidasState importarBatidasState;
        private DateTime nascimento;
        private string nome;
        private string rg;
        private string saldo;
        private string saldoInicial; 
        #endregion

        public ImportarBatidasViewModel(IContextProvider repositorio)
        {
            this.repositorio = repositorio;

            Nascimento = DataFinal = DateTime.Today;
            DataInicial = DateTime.Today.Subtract(TimeSpan.FromDays(14));

            Empresas = new ObservableCollection<Empresa>();
            Historico = new ObservableCollection<Historico>();
            Historico.CollectionChanged += (sender, args) => RaisePropertyChanged("Batidas");

            if (IsInDesignMode)
            {
                #region Design Data
                RG = "2007009198057";
                Nome = "Alberto Monteiro";
                Saldo = "07:00C";
                SaldoInicial = "06:00C";

                var fortes = new Empresa { Codigo = "006", Nome = "Fortes Informática", RazaoSocial = "Fortes Informática" };
                Empresas.Add(fortes);
                EmpresaSelecionada = fortes;

                for (int i = 0; i < 5; i++)
                {
                    var historico = new Historico
                    {
                        Data = DateTime.Today.AddDays((-1) * i),
                        Diferenca = "a",
                        Previsto = "b",
                        Realizado = "c",
                        Saldo = "s",
                        Informacoes = new List<Informacao>
                        {
                            new Informacao {Descricao = @"Trabalho de 08:00 a 12:00"},
                            new Informacao {Descricao = @"Trabalho de 13:00 a 18:00"}
                        }
                    };
                    Historico.Add(historico);
                } 
                #endregion
            }
            else
            {
                OnFiltrar = new RelayCommand(Filtrar);
                OnImportar = new RelayCommand(Importar);
            }
        }

        public ImportarBatidasState ImportarBatidasState
        {
            get { return importarBatidasState; }
            set
            {
                importarBatidasState = value;
                RaisePropertyChanged("ImportarBatidasState");
            }
        }

        public string RG
        {
            get { return rg; }
            set
            {
                rg = value;
                RaisePropertyChanged("RG");
            }
        }

        public string Nome
        {
            get { return nome; }
            set
            {
                nome = value;
                RaisePropertyChanged("Nome");
            }
        }

        public Empresa EmpresaSelecionada
        {
            get { return empresaSelecionada; }
            set
            {
                empresaSelecionada = value;
                RaisePropertyChanged("EmpresaSelecionada");
            }
        }

        public DateTime DataInicial
        {
            get { return dataInicial; }
            set
            {
                dataInicial = value;
                RaisePropertyChanged("DataInicial");
            }
        }

        public DateTime DataFinal
        {
            get { return dataFinal; }
            set
            {
                dataFinal = value;
                RaisePropertyChanged("DataFinal");
            }
        }

        public string Periodo
        {
            get { return string.Format("{0:dd/MM/yyyy} a {1:dd/MM/yyyy}", DataInicial, DataFinal); }
        }

        public string Saldo
        {
            get { return saldo; }
            set
            {
                saldo = value;
                RaisePropertyChanged("Saldo");
            }
        }

        public string SaldoInicial
        {
            get { return saldoInicial; }
            set
            {
                saldoInicial = value;
                RaisePropertyChanged("SaldoInicial");
            }
        }

        public RelayCommand OnFiltrar { get; set; }

        public RelayCommand OnImportar { get; set; }

        public ObservableCollection<Empresa> Empresas { get; set; }

        public ObservableCollection<Historico> Historico { get; set; }

        public IEnumerable<Group<Batida>> Batidas
        {
            get
            {
                return from batida in Historico.ToBatidas()
                       group batida by batida.Horario.Date.ToString("dd/MM/yyyy") into batidasGrouped
                       select new Group<Batida>(batidasGrouped.Key, batidasGrouped);
            }
        }

        public DateTime Nascimento
        {
            get { return nascimento; }
            set
            {
                nascimento = value;
                RaisePropertyChanged("Nascimento");
            }
        }

        private void Importar()
        {
            ImportarBatidasState = ImportarBatidasState.Importando;
        }

        private void Filtrar()
        {
            ImportarBatidasState = ImportarBatidasState.Filtrando;
        }
    }
}