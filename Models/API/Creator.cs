using System.Text.Json.Serialization;

namespace Isogeo.Models.API
{
    public class Creator
    {
        [JsonPropertyName("_id")]
        public string? Id { get; set; }

        [JsonPropertyName("contact")]
        public Contact? Contact { get; set; }
    }
}
