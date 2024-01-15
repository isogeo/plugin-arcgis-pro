using System.Text.Json.Serialization;

namespace Isogeo.Models.API
{
    public class Search
    {
        [JsonPropertyName("tags")]
        public IDictionary<string, string>? Tags { get; set; }

        [JsonPropertyName("offset")]
        public double Offset { get; set; }

        [JsonPropertyName("limit")]
        public double Limit { get; set; }

        [JsonPropertyName("total")]
        public double Total { get; set; }

        [JsonPropertyName("results")]
        public List<Result>? Results { get; set; }
    }
}
