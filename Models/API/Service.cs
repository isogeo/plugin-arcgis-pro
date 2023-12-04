using System;
using System.Text.Json.Serialization;

namespace Isogeo.Models.API
{
    public class Service
    {
        [JsonPropertyName("_id")]
        public string _id { get; set; }

        [JsonPropertyName("title")]
        public string title { get; set; }

        [JsonPropertyName("path")]
        public string path { get; set; }

        [JsonPropertyName("format")]
        public string format { get; set; }

    }
}
