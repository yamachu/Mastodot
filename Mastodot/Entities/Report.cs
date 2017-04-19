using System;
using Newtonsoft.Json;

namespace Mastodot.Entities
{
    public class Report : BaseMastodonEntity
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("action_taken")]
        public string ActionTaken { get; set; }
    }
}
