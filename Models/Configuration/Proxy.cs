using System.Text.Json.Serialization;

namespace Isogeo.Models.Configuration
{
    public class Proxy
    {
        [JsonPropertyName("proxyUrl")]
        public string proxyUrl { get; set; }

        [JsonPropertyName("proxyUser")]
        public string proxyUser { get; set; }

        [JsonPropertyName("proxyPassword")]
        public string proxyPassword { get; set; }

        public Proxy()
        {
            proxyUrl = "";
            proxyUser = "";
            proxyPassword = "";
        }
    
    }


}
