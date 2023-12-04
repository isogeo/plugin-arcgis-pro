using System.Text.Json.Serialization;

namespace Isogeo.Models.API
{
    public class Coordinate_system
    {

        [JsonPropertyName("name")]
        public string name
        {
            get;
            set;
        }

        [JsonPropertyName("code")]
        public string code
        {
            get;
            set;
        }






    }
}
