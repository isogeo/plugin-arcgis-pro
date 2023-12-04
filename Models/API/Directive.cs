using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Isogeo.Models.API
{
    public class Directive
    {
        [JsonPropertyName("_id")]
        public string _id
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

        [JsonPropertyName("description")]
        public string description
        {
            get;
            set;
        }
    }
}
