using System.Text.Json.Serialization;

namespace Isogeo.Models.API
{
    public class Service
    {
        [JsonPropertyName("_id")]
        public string? Id { get; set; }

        [JsonPropertyName("title")]
        public string? Title { get; set; }

        [JsonPropertyName("path")]
        public string? Path { get; set; }

        [JsonPropertyName("format")]
        public string? Format { get; set; }

    }
}
