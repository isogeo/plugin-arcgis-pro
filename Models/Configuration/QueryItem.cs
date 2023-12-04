
using System.Text.Json.Serialization;

namespace Isogeo.Models.Configuration
{
    public class QueryItem
    {
        [JsonPropertyName("query")]
        public string? Query { get; set; }
    }
}
