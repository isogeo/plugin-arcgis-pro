using System.Text.Json.Serialization;

namespace Isogeo.Models.API
{
    public class FeatureAttributes
    {
        [JsonPropertyName("_id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("alias")]
        public string Alias { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("comment")]
        public string Comment { get; set; }

        [JsonPropertyName("dataType")]
        public string DataType { get; set; }
    }
}
