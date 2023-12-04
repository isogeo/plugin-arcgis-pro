using System.Text.Json.Serialization;

namespace Isogeo.Models.API
{
    public class License
    {

        [JsonPropertyName("_id")]
        public string _id
        {
            get;
            set;
        }

        [JsonPropertyName("_tag")]
        public string _tag
        {
            get;
            set;
        }

        [JsonPropertyName("name")]
        public string name
        {
            get;
            set;
        }

        [JsonPropertyName("link")]
        public string link
        {
            get;
            set;
        }

        [JsonPropertyName("content")]
        public string content
        {
            get;
            set;
        }
    }
}
