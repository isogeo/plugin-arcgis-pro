using System.Text.Json.Serialization;

namespace Isogeo.Models.API
{
    public class ServiceLayer
    {
        [JsonPropertyName("_id")]
        public string? Id { get; set; }

        [JsonPropertyName("id")]
        public string? Name { get; set; }

        [JsonPropertyName("service")]
        public Service? Service { get; set; }

        [JsonPropertyName("titles")]
        public List<Title>? Titles { get; set; }
    }
}
