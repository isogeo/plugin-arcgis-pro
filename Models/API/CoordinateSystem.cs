using System.Text.Json.Serialization;

namespace Isogeo.Models.API
{
    public class CoordinateSystem
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("code")]
        public string? Code { get; set; }
    }
}
