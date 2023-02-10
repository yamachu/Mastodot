using System;
using Mastodot.Enums;
using Newtonsoft.Json;

namespace Mastodot.Entities
{
    public class Attachment : BaseMastodonEntity
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("type")]
        public AttachmentType Type { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("remote_url")]
        public string RemoteUrl { get; set; }

        [JsonProperty("preview_url")]
        public string PreviewUrl { get; set; }

        [JsonProperty("text_url")]
        public string TextUrl { get; set; }
    }
}
