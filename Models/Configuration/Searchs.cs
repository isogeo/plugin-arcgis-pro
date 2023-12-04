using System.Text.Json.Serialization;

namespace Isogeo.Models.Configuration
{
    public class Searchs
    {
        [JsonPropertyName("searchs")]
        public List<Search>? SearchDetails { get; set; }
    }
}
