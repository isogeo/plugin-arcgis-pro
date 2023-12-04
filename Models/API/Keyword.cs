using System.Text.Json.Serialization;

namespace Isogeo.Models.API
{
    public class Keyword
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

        [JsonPropertyName("code")]
        public string code
        {
            get;
            set;
        }

        [JsonPropertyName("text")]
        public string text
        {
            get;
            set;
        }
    }
}
