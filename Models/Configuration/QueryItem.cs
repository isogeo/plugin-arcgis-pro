
using System.Text.Json.Serialization;

namespace Isogeo.Models.Configuration
{
    public class QueryItem
    {
        [JsonPropertyName("query")]
        public string query { get; set; }
    }
}
