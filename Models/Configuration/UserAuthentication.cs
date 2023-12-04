
using System.Text.Json.Serialization;

namespace Isogeo.Models.Configuration
{
    public class UserAuthentication
    {
        [JsonPropertyName("id")]
        public string id { get; set; }

        [JsonPropertyName("secret")]
        public string secret { get; set; }
    }
}
