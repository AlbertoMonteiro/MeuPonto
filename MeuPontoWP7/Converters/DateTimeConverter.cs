using System;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MeuPontoWP7.Converters
{
    public class DateTimeConverter : JsonConverter
    {

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) { }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var readFrom = JToken.ReadFrom(reader);

            var dataString = readFrom.ToString();

            var culture = new CultureInfo("pt-br");

            return DateTime.Parse(dataString, culture.DateTimeFormat);
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(DateTime);
        }
    }
}