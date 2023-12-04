
using System.Text.Json.Serialization;

namespace Isogeo.Models.Configuration
{
    public class configuration
    {
        [JsonPropertyName("language")]
        public string language { get; set; }

        [JsonPropertyName("defaultSearch")]
        public string defaultSearch { get; set; }

        [JsonPropertyName("actionType")]
        public string actionType { get; set; }

        [JsonPropertyName("owner")]
        public string owner { get; set; }

        [JsonPropertyName("geographicalOperator")]
        public string geographicalOperator { get; set; }

        [JsonPropertyName("sortMethode")]
        public string sortMethode { get; set; }

        [JsonPropertyName("sortDirection")]
        public string sortDirection { get; set; }

        [JsonPropertyName("userAuthentication")]
        public UserAuthentication userAuthentication { get; set; }

        [JsonPropertyName("searchs")]
        public Searchs searchs { get; set; }

        [JsonPropertyName("proxy")]
        public Proxy proxy { get; set; }

        [JsonPropertyName("emailSupport")]
        public string emailSupport { get; set; }

        [JsonPropertyName("emailSubject")]
        public string emailSubject { get; set; }

        [JsonPropertyName("emailBody")]
        public string emailBody { get; set; }

        [JsonPropertyName("urlHelp")]
        public string urlHelp { get; set; }

        [JsonPropertyName("query")]
        public string query { get; set; }

        [JsonPropertyName("fileSde")]
        public string fileSde { get; set; }

        [JsonPropertyName("apiIdUrl")]
        public string apiIdUrl { get; set; }

        [JsonPropertyName("apiUrl")]
        public string apiUrl { get; set; }
    }
}
