using System.Text.Json.Serialization;

namespace Isogeo.Models.API
{
    public class SpecificationDetail
    {

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("published")]
        public string? Published { get; set; }
    }
}
