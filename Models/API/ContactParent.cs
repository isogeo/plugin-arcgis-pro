using System;
using System.Text.Json.Serialization;

namespace Isogeo.Models.API
{
    public class ContactParent
    {

        [JsonPropertyName("contact")]
        public Contact contact
        {
            get;
            set;
        }

        [JsonPropertyName("role")]
        public string role
        {
            get;
            set;
        }


    }
}
