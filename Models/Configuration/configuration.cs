
using System.Text.Json.Serialization;

namespace Isogeo.Models.Configuration
{
    public class Configuration
    {
        [JsonPropertyName("language")]
        public string? Language { get; set; }

        [JsonPropertyName("defaultSearch")]
        public string? DefaultSearch { get; set; }

        [JsonPropertyName("actionType")]
        public string? ActionType { get; set; }

        [JsonPropertyName("owner")]
        public string? Owner { get; set; }

        [JsonPropertyName("geographicalOperator")]
        public string? GeographicalOperator { get; set; }

        [JsonPropertyName("sortMethode")]
        public string? sortMethode { get; set; }

        [JsonPropertyName("sortDirection")]
        public string? SortDirection { get; set; }

        [JsonPropertyName("userAuthentication")]
        public UserAuthentication? UserAuthentication { get; set; }

        [JsonPropertyName("searchs")]
        public Searchs? Searchs { get; set; }

        [JsonPropertyName("proxy")]
        public Proxy? Proxy { get; set; }

        [JsonPropertyName("emailSupport")]
        public string? EmailSupport { get; set; }

        [JsonPropertyName("emailSubject")]
        public string? EmailSubject { get; set; }

        [JsonPropertyName("emailBody")]
        public string? EmailBody { get; set; }

        [JsonPropertyName("urlHelp")]
        public string? UrlHelp { get; set; }

        [JsonPropertyName("query")]
        public string? Query { get; set; }

        [JsonPropertyName("fileSde")]
        public string? FileSde { get; set; }

        [JsonPropertyName("apiIdUrl")]
        public string? ApiIdUrl { get; set; }

        [JsonPropertyName("apiUrl")]
        public string? ApiUrl { get; set; }
    }
}
