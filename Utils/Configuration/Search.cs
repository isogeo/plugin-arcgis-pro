using System.Text.Json.Serialization;

namespace Isogeo.Utils.Configuration
{
    public class Search
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; } = "";

        [JsonPropertyName("query")]
        public string? Query { get; set; } = "";

        [JsonPropertyName("box")]
        public string? Box { get; set; } = "";
    }
}
