using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Isogeo.Models.API
{
    public class ServiceLayer
    {
        [JsonPropertyName("_id")]
        public string _id { get; set; }

        [JsonPropertyName("id")]
        public string id { get; set; }

        [JsonPropertyName("service")]
        public Service service { get; set; }

        [JsonPropertyName("titles")]
        public List<Title> titles { get; set; }
    }
}
