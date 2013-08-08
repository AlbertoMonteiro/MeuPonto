using System;
using System.Collections.Generic;
using System.Net;
using MeuPontoWP7.Extensions;
using MeuPontoWP7.Services.Fortes.Models;

namespace MeuPontoWP7.Services.Fortes
{
    public class FortesPonto
    {
        private const string URL_BASE = @"http://10.1.4.109:8087/datasnap/rest/TSMPonto/";
        private const string GET_BATIDA_EMPREGADO = @"GetBatidaEmpregado/{0}/{1}/{2}/{3}/";
        private const string GET_EMPRESAS_EMPREGADO = @"GetEmpresaEmpregado/{0}";
        private const string AUTENTICA = @"AutenticaUsuario/{0}/{1}";

        public void Batidas(string rg, string empresa, DateTime dataInicial, DateTime dataFinal, Action<IEnumerable<Historico>> callback)
        {
            var deserializer = new Deserializer<IEnumerable<Historico>>(callback);

            var request = new WebClient();

            var url = string.Format(URL_BASE + GET_BATIDA_EMPREGADO, rg, empresa, dataInicial.RemoveBarras(), dataFinal.RemoveBarras());
            var uri = new Uri(url, UriKind.Absolute);

            request.DownloadStringCompleted += deserializer.Deserializa;
            request.DownloadStringAsync(uri);
        }

        public void Empresas(string rg, Action<IEnumerable<Empresa>> callback)
        {
            var deserializer = new Deserializer<IEnumerable<Empresa>>(callback);

            var request = new WebClient();

            var url = string.Format(URL_BASE + GET_EMPRESAS_EMPREGADO, rg);
            var uri = new Uri(url, UriKind.Absolute);

            request.DownloadStringCompleted += deserializer.Deserializa;
            request.DownloadStringAsync(uri);
        }

        public void Login(string rg, DateTime nascimento, Action<BoolValue> callback)
        {
            var deserializer = new Deserializer<BoolValue>(callback);

            var request = new WebClient();

            var url = string.Format(URL_BASE + AUTENTICA, rg, nascimento.ToMD5().ToUpper());
            var uri = new Uri(url, UriKind.Absolute);

            request.DownloadStringCompleted += deserializer.Deserializa;
            request.DownloadStringAsync(uri);            
        }
    }
}
