using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace Isogeo.Utils.Configuration
{
    public class Proxy
    {
        [XmlElement("proxyUrl")]
        [JsonPropertyName("proxyUrl")]
        public string ProxyUrl { get; set; } = "";

        [XmlElement("proxyUser")]
        [JsonPropertyName("proxyUser")]
        public string ProxyUser { get; set; } = "";

        [XmlElement("proxyPassword")]
        [JsonPropertyName("proxyPassword")]
        public string ProxyPassword { get; set; } = "";
    }


}
