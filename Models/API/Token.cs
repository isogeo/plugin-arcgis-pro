using System.Text.Json.Serialization;

namespace Isogeo.Models.API
{
    public class Token
    {
        [JsonPropertyName("access_token")]
        public string? AccessToken { get; init; }

        [JsonPropertyName("token_type")]
        public string? TokenType { get; init; }

        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; init; }

        public string? StatusResult { get; set; }
    }
}
