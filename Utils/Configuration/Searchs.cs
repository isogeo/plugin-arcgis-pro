using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Isogeo.Utils.Configuration
{
    public class Searchs
    {
        [JsonPropertyName("searchs")]
        public List<Search>? SearchDetails { get; set; }
    }
}
