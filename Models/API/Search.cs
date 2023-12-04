using System.Text.Json.Serialization;

namespace Isogeo.Models.API
{
    public class Search
    {
        [JsonPropertyName("tags")]
        public IDictionary<string, string> tags;

        //[JsonPropertyName("envelope")]
        //public string envelope { get; set; }

        [JsonPropertyName("offset")]
        public double offset { get; set; }

        [JsonPropertyName("limit")]
        public double limit { get; set; }

        [JsonPropertyName("total")]
        public double total { get; set; }

        [JsonPropertyName("results")]
        public List<Result> results { get; set; }
    }
}
