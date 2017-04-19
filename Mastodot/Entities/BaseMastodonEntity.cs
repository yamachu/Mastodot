using System;
using Newtonsoft.Json;

namespace Mastodot.Entities
{
    public class BaseMastodonEntity : IBaseMastodonEntity
    {
        [JsonIgnore]
        public string RawJson { get; set; }
    }

    public interface IBaseMastodonEntity
    {
        string RawJson { get; set; }
    }
}
