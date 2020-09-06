using System.Collections.Generic;

namespace Isogeo.Models.API
{
    public class Tags
    {
        public List<string> ResourcesTypes { get; set; }
        public List<string> Owners { get; set; }
        public List<string> Keywords { get; set; }
        public List<string> ThemeInspire { get; set; }
        public List<string> Formats { get; set; }
        public List<string> Srs { get; set; }
        public List<string> Actions { get; set; }
        public List<string> Contacts { get; set; }
        public List<string> Licenses { get; set; }
        public int conformity;

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

            if (result.tags == null) return;
            foreach (var item in result.tags)
            {
                var key = item.Key;
                var val = item.Value;

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
                    conformity = 1;
                }
            }
        }
    }
}
