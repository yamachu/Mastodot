using System;
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
    }
}
