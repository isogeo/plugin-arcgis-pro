using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace Isogeo.Utils.Configuration
{
    public class Searchs
    {
        [XmlElement("searchs")]
        [JsonPropertyName("searchs")]
        public List<Search>? SearchDetails { get; set; }
    }
}
