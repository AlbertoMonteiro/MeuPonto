using System;
using System.IO;
using System.Linq;
using System.Net;
using MeuPontoWP7.Converters;
using Newtonsoft.Json;

namespace MeuPontoWP7.Services
{
    public class Deserializer<T>
    {
        private readonly Action<T> callback;

        public Deserializer(Action<T> callback)
        {
            this.callback = callback;
        }

        public void Deserializa(object sender, DownloadStringCompletedEventArgs e)
        {
            //transf sender em IEnumerable<Historico>
            var jsonSerializer = new JsonSerializer();
            jsonSerializer.Converters.Add(new DateTimeConverter());
            var result = jsonSerializer.Deserialize<Result>(new JsonTextReader(new StringReader(e.Result)));

            var historicos = jsonSerializer.Deserialize<T>(new JsonTextReader(new StringReader(result.result.FirstOrDefault())));

            callback(historicos);
        }

    }
}