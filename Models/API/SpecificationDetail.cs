using System.Text.Json.Serialization;

namespace Isogeo.Models.API
{
    public class SpecificationDetail
    {

        [JsonPropertyName("name")]
        public string name { get; set; }

        [JsonPropertyName("published")]
        public string published { get; set; }
    }
}
