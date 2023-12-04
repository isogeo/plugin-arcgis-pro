
using System.Text.Json.Serialization;

namespace Isogeo.Utils.Configuration
{
    public class UserAuthentication
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("secret")]
        public string? Secret { get; set; }
    }
}
