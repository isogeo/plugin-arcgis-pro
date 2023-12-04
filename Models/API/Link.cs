using System.Text.Json.Serialization;

namespace Isogeo.Models.API
{
    public class Link
    {
        [JsonPropertyName("_id")]
        public string? Id { get; set; }

        [JsonPropertyName("link")]
        public Link? OneLink { get; set; }

        [JsonPropertyName("type")]
        public string? Type { get; set; }

        [JsonPropertyName("title")]
        public string? Title { get; set; }

        [JsonPropertyName("url")]
        public string? Url { get; set; }

        [JsonPropertyName("kind")]
        public string? Kind { get; set; }

        [JsonPropertyName("actions")]
        public List<string>? Actions { get; set; }
    }
}
