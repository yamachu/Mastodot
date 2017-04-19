using System;
using Newtonsoft.Json;

namespace Mastodot.Entities
{
    public class Instance: BaseMastodonEntity
    {
        [JsonProperty("uri")]
        public string Uri { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }
    }
}
