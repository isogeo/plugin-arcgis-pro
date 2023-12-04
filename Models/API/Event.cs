using System;
using System.Text.Json.Serialization;

namespace Isogeo.Models.API
{
    public class Event
    {
        [JsonPropertyName("_id")]
        public string _id
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

        [JsonPropertyName("date")]
        public string date
        {
            get;
            set;
        }

        [JsonPropertyName("format")]
        public string format
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


    }
}
