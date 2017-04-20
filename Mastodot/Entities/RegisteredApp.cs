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

        [JsonProperty("host")]
        public string Host { get; set; }

        [JsonProperty("scope")]
        public Scope Scope { get; set; }

        // For dump to file
        public bool ShouldSerializeHost() {
            return RegisteredApp.ForceSerializeForDump;
        }
        public bool ShouldSerializeScope() {
            return RegisteredApp.ForceSerializeForDump;
        }
        [JsonIgnore]
        public static bool ForceSerializeForDump { get; set; } = false;
    }
}
