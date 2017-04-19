using System;
using Mastodot.Enums;
using Newtonsoft.Json;

namespace Mastodot.Entities
{
    public class Notification: BaseMastodonEntity, IStreamEntity
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("type")]
        public NotificationType Type { get; set; }

        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("account")]
        public Account Account { get; set; }

        [JsonProperty("status")]
        public Status Status { get; set; }
    }
}
