using System.Text.Json.Serialization;

namespace Isogeo.Models.API
{
    public class Title
    {
        [JsonPropertyName("value")]
        public string? Value { get; set; }
    }
}
