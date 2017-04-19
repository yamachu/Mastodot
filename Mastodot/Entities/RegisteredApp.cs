using System;
using Mastodot.Enums;
using Newtonsoft.Json;

namespace Mastodot.Entities
{
    public class RegisteredApp: BaseMastodonEntity
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("redirect_uri")]
        public string RedirectUri { get; set; }

        [JsonProperty("client_id")]
        public string ClientId { get; set; }

        [JsonProperty("client_secret")]
        public string ClientSecret { get; set; }

        [JsonIgnore]
        public string Host { get; set; }

        [JsonIgnore]
        public Scope Scope { get; set; }
    }
}
