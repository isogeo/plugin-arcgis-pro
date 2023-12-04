using System.Text.Json.Serialization;

namespace Isogeo.Models.API
{
    public class Creator
    {
        [JsonPropertyName("_id")]
        public string _id
        {
            get;
            set;
        }

        [JsonPropertyName("contact")]
        public Contact contact
        {
            get;
            set;
        }
    }
}
