using System;
using Newtonsoft.Json;

namespace Mastodot.Entities
{
    public class Mention: BaseMastodonEntity
    {
        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("username")]
        public string UserName { get; set; }

        [JsonProperty("acct")]
        public string FullUserName { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }
    }
}
