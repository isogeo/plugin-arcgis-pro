using System.Text.Json.Serialization;

namespace Isogeo.Models.API
{
    public class Title
    {
        [JsonPropertyName("value")]
        public string value { get; set; }
    }
}
