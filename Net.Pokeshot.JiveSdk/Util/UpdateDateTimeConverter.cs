using Newtonsoft.Json.Converters;
using System;
using Newtonsoft.Json;

namespace Net.Pokeshot.JiveSdk.Util
{
    public class UpdateDateTimeConverter : DateTimeConverterBase
    {
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return DateTime.Parse(reader.Value.ToString());
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var dateTimeValue = (DateTime)value;
            var dateTimeRoundTrip = new DateTime(dateTimeValue.Ticks, DateTimeKind.Unspecified);
            var dateString = dateTimeRoundTrip.ToString("O");
            dateString = $"{dateString.Substring(0, dateString.Length - 4)}+0000";
            writer.WriteValue(dateString);
        }
    }
}
