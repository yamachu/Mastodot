using System;
using Mastodot.Utils.JsonConverters;
using Newtonsoft.Json;

namespace Mastodot.Entities
{
    public class TokenInfo : BaseMastodonEntity
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        // ToDo: Research how to type cast to Scope...
        [JsonProperty("scope")]
        public string Scope { get; set; }

        // ex: 1234567890
        [JsonProperty("created_at")]
        [JsonConverter(typeof(UnixTimeDateTimeConverter))]
        public DateTime CreatedAt { get; set; }
    }
}
