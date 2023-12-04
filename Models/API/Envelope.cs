using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Isogeo.Models.API
{
    public class Envelope
    {

        [JsonPropertyName("type")]
        public string type
        {
            get;
            set;
        }

        [JsonPropertyName("editionProfile")]
        public string editionProfile
        {
            get;
            set;
        }

        [JsonPropertyName("series")]
        public Boolean series
        {
            get;
            set;
        }

        [JsonPropertyName("geometry")]
        public string geometry
        {
            get;
            set;
        }

        [JsonPropertyName("features")]
        public int features
        {
            get;
            set;
        }

        [JsonPropertyName("box")]
        public List<Double> box
        {
            get;
            set;
        }

        [JsonPropertyName("coordinates")]
        public Object coordinates
        {
            get;
            set;
        }


    }
}
