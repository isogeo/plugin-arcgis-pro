using System.Text.Json.Serialization;

namespace Isogeo.Models.API
{
    public class Layer
    {
        [JsonPropertyName("_id")]
        public string _id
        {
            get;
            set;
        }

        [JsonPropertyName("id")]
        public string id
        {
            get;
            set;
        }

        [JsonPropertyName("titles")]
        public List<Title> titles
        {
            get;
            set;
        }
    }
}
