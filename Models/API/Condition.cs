using System.Text.Json.Serialization;

namespace Isogeo.Models.API
{
    public class Condition
    {
        public string _id
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

        [JsonPropertyName("license")]
        public License license
        {
            get;
            set;
        }

        public string email
        {
            get;
            set;
        }

        public string phone
        {
            get;
            set;
        }

        public string addressLine1
        {
            get;
            set;
        }

        public string addressLine2
        {
            get;
            set;
        }
        public string zipCode
        {
            get;
            set;
        }
        public string city
        {
            get;
            set;
        }
        public string country
        {
            get;
            set;
        }
        
                
                
                    
                    
                        
                        
                            
        
        
    }
}
