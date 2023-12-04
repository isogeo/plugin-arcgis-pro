using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace Isogeo.Utils.Configuration
{
    [XmlType("configuration")]
    public class Configuration
    {
        [XmlElement("defaultSearch")]
        [JsonPropertyName("defaultSearch")]
        public string? DefaultSearch { get; set; }

        [XmlElement("geographicalOperator")]
        [JsonPropertyName("geographicalOperator")]
        public string? GeographicalOperator { get; set; }

        [XmlElement("sortMethode")]
        [JsonPropertyName("sortMethode")]
        public string? SortMethode { get; set; }

        [XmlElement("sortDirection")]
        [JsonPropertyName("sortDirection")]
        public string? SortDirection { get; set; }

        [XmlElement("userAuthentication")]
        [JsonPropertyName("userAuthentication")]
        public UserAuthentication? UserAuthentication { get; set; }

        [XmlElement("searchs")]
        [JsonPropertyName("searchs")]
        public Searchs? Searchs { get; set; }

        [XmlElement("proxy")]
        [JsonPropertyName("proxy")]
        public Proxy? Proxy { get; set; }

        [XmlElement("emailSupport")]
        [JsonPropertyName("emailSupport")]
        public string? EmailSupport { get; set; }

        [XmlElement("emailSubject")]
        [JsonPropertyName("emailSubject")]
        public string? EmailSubject { get; set; }

        [XmlElement("emailBody")]
        [JsonPropertyName("emailBody")]
        public string? EmailBody { get; set; }

        [XmlElement("urlHelp")]
        [JsonPropertyName("urlHelp")]
        public string? UrlHelp { get; set; }

        [XmlElement("fileSde")]
        [JsonPropertyName("fileSde")]
        public string? FileSde { get; set; }

        [XmlElement("apiIdUrl")]
        [JsonPropertyName("apiIdUrl")]
        public string? ApiIdUrl { get; set; }

        [XmlElement("apiUrl")]
        [JsonPropertyName("apiUrl")]
        public string? ApiUrl { get; set; }
    }
}
