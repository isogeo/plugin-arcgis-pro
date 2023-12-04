using System.Text.Json.Serialization;

namespace Isogeo.Models.Configuration
{
    public class Proxy
    {
        [JsonPropertyName("proxyUrl")]
        public string ProxyUrl { get; set; } = "";

        [JsonPropertyName("proxyUser")]
        public string ProxyUser { get; set; } = "";

        [JsonPropertyName("proxyPassword")]
        public string ProxyPassword { get; set; } = "";
    }


}
