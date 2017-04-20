using System;
using System.Reflection;
using Mastodot.Entities;
using Newtonsoft.Json;

namespace Mastodot.Utils
{
    public class MastodonJsonConverter
    {
        public static T TryDeserialize<T>(string body)
        {
            var deserialized = JsonConvert.DeserializeObject<T>(body, new ErrorThrowJsonConverter<T>());
            if (deserialized is Entities.IBaseMastodonEntity) {
                ((IBaseMastodonEntity)deserialized).RawJson = body;
            }
            return deserialized;
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
