using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Mastodot.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum NotificationType
    {
        [EnumMember(Value = "mention")]
        Mention,
        [EnumMember(Value = "reblog")]
        Reblog,
        [EnumMember(Value = "favourite")]
        Favourite,
        [EnumMember(Value = "follow")]
        Follow
    }
}
