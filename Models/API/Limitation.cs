using System.Text.Json.Serialization;

namespace Isogeo.Models.API
{
    public class Limitation
    {

        [JsonPropertyName("_id")]
        public string _id
        {
            get;
            set;
        }

        [JsonPropertyName("type")]
        public string type
        {
            get;
            set;
        }

        [JsonPropertyName("description")]
        public string description
        {
            get;
            set;
        }

        [JsonPropertyName("restriction")]
        public string restriction
        {
            get;
            set;
        }

        [JsonPropertyName("directive")]
        public Directive directive
        {
            get;
            set;
        }
    }
}
