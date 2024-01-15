using System.Text.Json.Serialization;

namespace Isogeo.Models.API
{
    public class ContactParent
    {
        [JsonPropertyName("contact")]
        public Contact? Contact { get; set; }

        [JsonPropertyName("role")]
        public string? Role { get; set; }
    }
}
