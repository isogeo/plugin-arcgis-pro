using System.Text.Json.Serialization;

namespace Isogeo.Models.API
{
    public class Limitation
    {
        [JsonPropertyName("_id")]
        public string? Id { get; set; }

        [JsonPropertyName("type")]
        public string? Type { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("restriction")]
        public string? Restriction { get; set; }

        [JsonPropertyName("directive")]
        public Directive? Directive { get; set; }
    }
}
