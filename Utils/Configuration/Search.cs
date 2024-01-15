using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace Isogeo.Utils.Configuration
{
    public class Search
    {
        [XmlElement("name")]
        [JsonPropertyName("name")]
        public string? Name { get; set; } = "";

        [XmlElement("query")]
        [JsonPropertyName("query")]
        public string? Query { get; set; } = "";

        [XmlElement("box")]
        [JsonPropertyName("box")]
        public string? Box { get; set; } = "";
    }
}
