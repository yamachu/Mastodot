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
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }
    }
}
