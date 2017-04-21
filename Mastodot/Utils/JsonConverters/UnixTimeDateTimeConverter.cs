using System;
using Newtonsoft.Json;

namespace Mastodot.Utils.JsonConverters
{
    internal class UnixTimeDateTimeConverter : JsonConverter
    {
        private static readonly DateTime Origin = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var timeSec = (long)reader.Value;
            return Origin.AddSeconds(timeSec);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var date = (DateTime)value;
            var timeSpan = (date - new DateTime(1970, 1, 1, 0, 0, 0));

            writer.WriteValue((long)timeSpan.TotalSeconds);
        }
    }
}
