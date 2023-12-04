using System.Text.Json.Serialization;

namespace Isogeo.Models.API
{
    public class Tags
    {
        [JsonPropertyName("resourcesTypes")]
        public List<string>? ResourcesTypes { get; set; }

        [JsonPropertyName("owners")]
        public List<string>? Owners { get; set; }

        [JsonPropertyName("keywords")]
        public List<string>? Keywords { get; set; }

        [JsonPropertyName("themeInspire")]
        public List<string>? ThemeInspire { get; set; }

        [JsonPropertyName("formats")]
        public List<string>? Formats { get; set; }

        [JsonPropertyName("srs")]
        public List<string>? Srs { get; set; }

        [JsonPropertyName("actions")]
        public List<string>? Actions { get; set; }

        [JsonPropertyName("contacts")]
        public List<string>? Contacts { get; set; }

        [JsonPropertyName("licenses")]
        public List<string>? Licenses { get; set; }

        [JsonPropertyName("conformity")]
        public int? Conformity { get; set; }

        public Tags(Result result)
        {
            ResourcesTypes = new List<string>();
            Owners = new List<string>();
            Keywords = new List<string>();
            ThemeInspire = new List<string>();
            Formats = new List<string>();
            Srs = new List<string>();
            Actions = new List<string>();
            Contacts = new List<string>();
            Licenses = new List<string>();

            if (result.Tags == null) return;
            foreach (var (key, val) in result.Tags)
            {
                if (key.StartsWith("owner"))
                {
                    Owners.Add(val);
                }
                else if (key.StartsWith("keyword:isogeo"))
                {
                    Keywords.Add(val);
                }
                else if (key.StartsWith("keyword:inspire-theme"))
                {
                    ThemeInspire.Add(val);
                }
                else if (key.StartsWith("format"))
                {
                    Formats.Add(val);
                }
                else if (key.StartsWith("coordinate-system"))
                {
                    Srs.Add(val);
                }
                else if (key.StartsWith("contact"))
                {
                    Contacts.Add(val);
                }
                else if (key.StartsWith("license"))
                {
                    Licenses.Add(val);
                }
                else if (key.StartsWith("action"))
                {
                    Actions.Add(val);
                }
                else if (key.StartsWith("type"))
                {
                    ResourcesTypes.Add(val);
                }
                else if (key.StartsWith("conformity"))
                {
                    Conformity = 1;
                }
            }
        }
    }
}
