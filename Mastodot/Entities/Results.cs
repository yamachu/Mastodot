using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Mastodot.Entities
{
    public class Results : BaseMastodonEntity
    {
        [JsonProperty("accounts")]
        public IEnumerable<Account> Accounts { get; set; }

        [JsonProperty("statuses")]
        public IEnumerable<Status> Statuses { get; set; }

        [JsonProperty("hashtags")]
        public IEnumerable<string> Hashtags { get; set; }
    }
}
