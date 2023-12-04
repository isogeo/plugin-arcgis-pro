using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Isogeo.Models.API
{
    public class Link
    {
        [JsonPropertyName("_id")]
        public string _id
        {
            get;
            set;
        }

        [JsonPropertyName("link")]
        public Link link
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

        [JsonPropertyName("title")]
        public string title
        {
            get;
            set;
        }

        [JsonPropertyName("url")]
        public string url
        {
            get;
            set;
        }

        [JsonPropertyName("kind")]
        public string kind
        {
            get;
            set;
        }

        [JsonPropertyName("actions")]
        public List<string> actions
        {
            get;
            set;
        }
    }
}
