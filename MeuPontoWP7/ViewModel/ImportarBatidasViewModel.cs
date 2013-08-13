using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MeuPonto.Common.Models;
using MeuPonto.Common.Repositorios;
using MeuPontoWP7.Extensions;
using MeuPontoWP7.Services.Fortes;
using MeuPontoWP7.Services.Fortes.Models;

namespace MeuPontoWP7.ViewModel
{
    public class ImportarBatidasViewModel : ViewModelBase
    {
        #region Fields

        private readonly FortesPonto fortesPonto;
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

        private int totalImportado;
        private int totalParaImportar;

        public ImportarBatidasViewModel(IContextProvider repositorio, FortesPonto fortesPonto)
        {
            this.repositorio = repositorio;
            this.fortesPonto = fortesPonto;

            DataFinal = DateTime.Today;
            Nascimento = DateTime.Today.AddYears(-18);
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
                for (var i = 0; i < 5; i++)
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
                OnLogin = new RelayCommand(Login);
                OnFiltrar = new RelayCommand(Filtrar);
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

        public DateTime Nascimento
        {
            get { return nascimento; }
            set
            {
                nascimento = value;
                RaisePropertyChanged("Nascimento");
            }
        }

        public int TotalImportado
        {
            get { return totalImportado; }
            set
            {
                totalImportado = value;
                RaisePropertyChanged("TotalImportado");
                RaisePropertyChanged("Progresso");
            }
        }

        public int TotalParaImportar
        {
            get { return totalParaImportar; }
            set
            {
                totalParaImportar = value;
                RaisePropertyChanged("TotalParaImportar");
                RaisePropertyChanged("Progresso");
            }
        }

        public string Progresso { get { return string.Format("{0} / {1}", TotalImportado, TotalParaImportar); } }

        public RelayCommand OnLogin { get; set; }

        public RelayCommand OnFiltrar { get; set; }

        public ObservableCollection<Empresa> Empresas { get; set; }

        public ObservableCollection<Historico> Historico { get; set; }

        public IEnumerable<KeyGroup<Batida>> Batidas
        {
            get { return Historico.ToBatidas().ToKeyGroup(item => item.Horario.Date.ToString("dd/MM/yyyy")); }
        }

        private void Filtrar()
        {
            fortesPonto.Batidas(RG, EmpresaSelecionada.Codigo, DataInicial, DataFinal, PreencheHistorico);
        }

        private void Login()
        {
            fortesPonto.Login(RG, Nascimento, value =>
            {
                if (value.Value)
                    fortesPonto.Empresas(RG, PreencheEmpresas);
            });
        }

        private void PreencheHistorico(IEnumerable<Historico> historicos)
        {
            ImportarBatidasState = ImportarBatidasState.Importando;
            var totalDeBatidas = TotalParaImportar = historicos.Count() - 2;
            var hist = historicos.Skip(1).Take(totalDeBatidas).ToList();
            TotalParaImportar = hist.ToBatidas().Count();
            foreach (var historico in hist)
                Historico.Add(historico);
        }

        private void PreencheEmpresas(IEnumerable<Empresa> empresas)
        {
            foreach (var empresa in empresas)
                Empresas.Add(empresa);

            EmpresaSelecionada = empresas.FirstOrDefault();
            ImportarBatidasState = ImportarBatidasState.Filtrando;
        }

        public void Importar()
        {
            foreach (var batida in Historico.ToBatidas())
            {
                ++TotalImportado;
                if (!repositorio.CacheContext.Batidas.Any(x => x.Horario == batida.Horario))
                    repositorio.CacheContext.Batidas.InsertOnSubmit(batida);
            }
            repositorio.CacheContext.SubmitChanges();
        }
    }
}