using System;
using System.ComponentModel;
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

        // version over 1.3
        [JsonProperty("version", DefaultValueHandling = DefaultValueHandling.Populate)]
        [DefaultValue(null)]
        public string Version { get; set; }
    }
}
