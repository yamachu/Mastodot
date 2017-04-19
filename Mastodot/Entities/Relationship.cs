using System;
using Newtonsoft.Json;

namespace Mastodot.Entities
{
    public class Relationship : BaseMastodonEntity
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("following")]
        public bool Following { get; set; }

        [JsonProperty("followed_by")]
        public bool FollowedBy { get; set; }

        [JsonProperty("blocking")]
        public bool Blocking { get; set; }

        [JsonProperty("muting")]
        public bool Muting { get; set; }

        [JsonProperty("requested")]
        public bool Requested { get; set; }
    }
}
