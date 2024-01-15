using System.Text.Json.Serialization;

namespace Isogeo.Models.API
{
    public class Specification
    {
        [JsonPropertyName("conformant")]
        public bool? Conformant { get; set; }

        [JsonPropertyName("specification")]
        public SpecificationDetail? SpecificationDetails { get; set; }
    }
}
