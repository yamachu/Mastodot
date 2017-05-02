using System;
using Mastodot.Entities;
using Mastodot.Exceptions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Mastodot.Utils.JsonConverters
{
    internal class ErrorThrowJsonConverter<T> : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (objectType.Name.StartsWith("IEnumerable"))
            {
                var jArray = JArray.Load(reader);
                return jArray.ToObject<T>();
            }

            var jObject = JObject.Load(reader);
            if (jObject["error"] != null)
            {
                var error = jObject.ToObject<Error>();
                error.RawJson = jObject.ToString();
                var exception = new DeserializeErrorException($"Cant deserialize response, Type {objectType}")
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
}
