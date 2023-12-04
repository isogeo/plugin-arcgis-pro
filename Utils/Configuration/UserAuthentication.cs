
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace Isogeo.Utils.Configuration
{
    public class UserAuthentication
    {
        [XmlElement("id")]
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [XmlElement("secret")]
        [JsonPropertyName("secret")]
        public string? Secret { get; set; }
    }
}
