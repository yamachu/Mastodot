using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Mastodot.Entities
{
    public class Context: BaseMastodonEntity
    {
        [JsonProperty("ancestors")]
        public IEnumerable<Status> Ancestors { get; set; }

        [JsonProperty("descendants")]
        public IEnumerable<Status> Descendants { get; set; }
    }
}
