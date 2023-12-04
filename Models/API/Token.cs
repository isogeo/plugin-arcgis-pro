using System;
using System.Text.Json.Serialization;

namespace Isogeo.Models.API
{
    public class Token
    {
        [JsonPropertyName("access_token")]
        public String access_token { get; init; }

        [JsonPropertyName("token_type")]
        public String token_type { get; init; }

        [JsonPropertyName("expires_in")]
        public int expires_in { get; init; }

        public String StatusResult { get; set; }
    }
}
