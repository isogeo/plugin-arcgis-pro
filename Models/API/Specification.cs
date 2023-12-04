using System;
using System.Text.Json.Serialization;

namespace Isogeo.Models.API
{
    public class Specification
    {
        [JsonPropertyName("conformant")]
        public bool conformant { get; set; }

        [JsonPropertyName("specification")]
        public SpecificationDetail specification { get; set; }
    }
}
