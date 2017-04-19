using System;
using Newtonsoft.Json;

namespace Mastodot.Entities
{
    public class Application : BaseMastodonEntity
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("website")]
        public string Website { get; set; }
    }
}
