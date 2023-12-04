using System.Text.Json.Serialization;

namespace Isogeo.Models.API
{
    public class Contact
    {
        [JsonPropertyName("Name")]
        public string Name
        {
            get;
            set;
        }

        [JsonPropertyName("Organization")]
        public string Organization
        {
            get;
            set;
        }

        [JsonPropertyName("Email")]
        public string Email
        {
            get;
            set;
        }

        [JsonPropertyName("Phone")]
        public string Phone
        {
            get;
            set;
        }

        [JsonPropertyName("AddressLine1")]
        public string AddressLine1
        {
            get;
            set;
        }

        [JsonPropertyName("AddressLine2")]
        public string AddressLine2
        {
            get;
            set;
        }

        [JsonPropertyName("AddressLine3")]
        public string AddressLine3
        {
            get;
            set;
        }

        [JsonPropertyName("ZipCode")]
        public string ZipCode
        {
            get;
            set;
        }

        [JsonPropertyName("City")]
        public string City
        {
            get;
            set;
        }

        [JsonPropertyName("CountryCode")]
        public string CountryCode
        {
            get;
            set;
        }
    }
}
