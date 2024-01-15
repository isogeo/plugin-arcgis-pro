using System.Text.Json.Serialization;

namespace Isogeo.Models.API
{
    public class Event
    {
        [JsonPropertyName("_id")]
        public string? Id { get; set; }

        [JsonPropertyName("kind")]
        public string? Kind { get; set; }

        [JsonPropertyName("date")]
        public string? Date { get; set; }

        [JsonPropertyName("format")]
        public string? Format { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }
    }
}
