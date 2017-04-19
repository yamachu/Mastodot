using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Mastodot.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Visibility
    {
        [EnumMember(Value = "public")]
        Public,
        [EnumMember(Value = "unlisted")]
        Unlisted,
        [EnumMember(Value = "private")]
        Private,
        [EnumMember(Value = "direct")]
        Direct
    }
}
