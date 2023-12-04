using System.Text.Json.Serialization;

namespace Isogeo.Models.API
{
    public class License
    {
        [JsonPropertyName("_id")]
        public string? Id { get; set; }

        [JsonPropertyName("_tag")]
        public string? Tag { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("link")]
        public string? Link { get; set; }

        [JsonPropertyName("content")]
        public string? Content { get; set; }
    }
}
