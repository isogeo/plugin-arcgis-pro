
using System.Text.Json.Serialization;

namespace Isogeo.Utils.Configuration
{
    public class QueryItem
    {
        [JsonPropertyName("query")]
        public string? Query { get; set; }
    }
}
