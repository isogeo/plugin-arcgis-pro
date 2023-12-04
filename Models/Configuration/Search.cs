using System.Text.Json.Serialization;

namespace Isogeo.Models.Configuration
{
    public class Search
    {
        [JsonPropertyName("name")]
        public string name { get; set; } = "";

        [JsonPropertyName("query")]
        public string query { get; set; } = "";

        [JsonPropertyName("box")]
        public string box { get; set; } = "";
    }
}
