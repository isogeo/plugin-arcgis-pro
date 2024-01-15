using System.Text.Json.Serialization;

namespace Isogeo.Models.API
{
    public class Envelope
    {
        [JsonPropertyName("type")]
        public string? Type { get; set; }

        [JsonPropertyName("editionProfile")]
        public string? EditionProfile { get; set; }

        [JsonPropertyName("series")]
        public bool? Series { get; set; }

        [JsonPropertyName("geometry")]
        public string? Geometry { get; set; }

        [JsonPropertyName("features")]
        public int? Features { get; set; }

        [JsonPropertyName("box")]
        public List<double>? Box { get; set; }

        [JsonPropertyName("coordinates")]
        public object? Coordinates { get; set; }
    }
}