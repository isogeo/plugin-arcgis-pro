using System.Text.Json.Serialization;

namespace Isogeo.Models.API
{
    public class Keyword
    {
        [JsonPropertyName("_id")]
        public string? Id { get; set; }

        [JsonPropertyName("_tag")]
        public string? Tag { get; set; }

        [JsonPropertyName("code")]
        public string? Code { get; set; }

        [JsonPropertyName("text")]
        public string? Text { get; set; }
    }
}
