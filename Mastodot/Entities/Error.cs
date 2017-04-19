using System;
using Newtonsoft.Json;

namespace Mastodot.Entities
{
    public class Error: BaseMastodonEntity
    {
        [JsonProperty("error")]
        public string Description { get; set; }
    }
}
