using System;
using System.Reflection;
using Mastodot.Entities;
using Mastodot.Exceptions;
using Mastodot.Utils.JsonConverters;
using Newtonsoft.Json;

namespace Mastodot.Utils
{
    internal class MastodonJsonConverter
    {
        public static T TryDeserialize<T>(string body)
        {
            try
            {
                var deserialized = JsonConvert.DeserializeObject<T>(body, new ErrorThrowJsonConverter<T>());
                if (deserialized is IBaseMastodonEntity)
                {
                    ((IBaseMastodonEntity)deserialized).RawJson = body;
                }
                return deserialized;
            }
            catch (JsonReaderException readerException)
            {
                var error = new Error
                {
                    RawJson = "Cannot read original JSON"
                };
                var exception = new DeserializeErrorException($"Invalid Json Format", readerException);
                exception.Error = error;
                throw exception;
            }
        }

        public static string TrySerialize<T>(T obj)
        {
            PropertyInfo _info = null;

            foreach (var info in typeof(T).GetRuntimeProperties())
            {
                if (info.Name.Equals("ForceSerializeForDump"))
                {
                    _info = info;
                    info.SetValue(null, true);
                    break;
                }
            }

            var serialized = JsonConvert.SerializeObject(obj);
            if (_info != null)
            {
                _info.SetValue(null, false);
            }

            return serialized;
        }
    }
}
