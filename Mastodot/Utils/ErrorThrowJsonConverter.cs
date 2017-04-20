using System;
using Mastodot.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Mastodot
{
    public class ErrorThrowJsonConverter<T> : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (objectType.Name.StartsWith("IEnumerable")) {
                JArray jArray = JArray.Load(reader);
                return jArray.ToObject<T>();
            }

            JObject jObject = JObject.Load(reader);
            if (jObject["error"] != null)
            {
                Error error = jObject.ToObject<Error>();
                var exception = new GeneratedErrorException()
                {
                    Error = error
                };
                throw exception;
            }

            return jObject.ToObject<T>();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }
    }

    public class UnixTimeDateTimeConverter : JsonConverter
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
