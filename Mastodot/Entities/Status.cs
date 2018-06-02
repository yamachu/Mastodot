﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using Mastodot.Enums;
using Newtonsoft.Json;

namespace Mastodot.Entities
{
    public class Status : BaseMastodonEntity, IStreamEntity
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("uri")]
        public string Uri { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("account")]
        public Account Account { get; set; }

        [JsonProperty("in_reply_to_id")]
        public int? InReplyToId { get; set; }

        [JsonProperty("in_reply_to_account_id")]
        public int? InReplyToAccountId { get; set; }

        [JsonProperty("reblog")]
        public Status Reblog { get; set; }

        [JsonProperty("content")]
        public string Content { get; set; }

        // ex: 2017-04-14T11:42:51.266Z
        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("reblogs_count")]
        public int ReblogsCount { get; set; }

        [JsonProperty("favourites_count")]
        public int FavouritesCount { get; set; }

        [JsonProperty("reblogged")]
        public bool? Reblogged { get; set; }

        [JsonProperty("favourited")]
        public bool? Favourited { get; set; }

        [JsonProperty("sensitive")]
        public bool? Sensitive { get; set; }

        [JsonProperty("spoiler_text")]
        public string SpoilerText { get; set; }

        [JsonProperty("visibility")]
        public Visibility Visibility { get; set; }

        [JsonProperty("media_attachments")]
        public IEnumerable<Attachment> MediaAttachments { get; set; }

        [JsonProperty("mentions")]
        public IEnumerable<Mention> Mentions { get; set; }

        [JsonProperty("tags")]
        public IEnumerable<Tag> Tags { get; set; }

        [JsonProperty("application")]
        public Application Application { get; set; }

        [JsonProperty("language", DefaultValueHandling = DefaultValueHandling.Populate)]
        [DefaultValue("en")]
        public string Language { get; set; }
    }
}
