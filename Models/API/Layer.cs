using System.Text.Json.Serialization;

namespace Isogeo.Models.API
{
    public class Layer
    {
        [JsonPropertyName("_id")]
        public string? AnotherId { get; set; }

        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("titles")]
        public List<Title>? Titles { get; set; }
    }
}
