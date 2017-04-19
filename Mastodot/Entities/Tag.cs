using System;
using Newtonsoft.Json;

namespace Mastodot.Entities
{
    public class Tag : BaseMastodonEntity
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }
    }
}
