using System;
using System.Collections.Generic;

namespace IsogeoLibrary.Objects
{
    public class Tags
    {
        public List<String> resources_types { get; set; }
        public List<String> owners { get; set; }
        public List<String> keywords { get; set; }
        public List<String> themeinspire { get; set; }
        public List<String> formats { get; set; }
        public List<String> srs { get; set; }
        public List<String> actions { get; set; }
        public List<String> contacts { get; set; }
        public List<String> licenses { get; set; }
        public int conformity=0;

        public Tags(API.Result result)
        {
            resources_types = new List<string>();
            owners = new List<string>();
            keywords = new List<string>();
            themeinspire = new List<string>();
            formats = new List<string>();
            srs = new List<string>();
            actions = new List<string>();
            contacts = new List<string>();
            licenses = new List<string>();

            if (result.tags == null) return;
            foreach (var item in result.tags)
            {
                String key = item.Key;
                String val = item.Value;

                if (key.StartsWith("owner"))
                {
                    owners.Add(val);
                }
                else if (key.StartsWith("keyword:isogeo"))
                {
                    keywords.Add(val);
                }
                else if (key.StartsWith("keyword:inspire-theme"))
                {
                    themeinspire.Add(val);
                }
                else if (key.StartsWith("format"))
                {
                    formats.Add(val);
                }
                else if (key.StartsWith("coordinate-system"))
                {
                    srs.Add(val);
                }
                else if (key.StartsWith("contact"))
                {
                    contacts.Add(val);
                }
                else if (key.StartsWith("license"))
                {
                    licenses.Add(val);
                }
                else if (key.StartsWith("action"))
                {
                    actions.Add(val);
                }
                else if (key.StartsWith("type"))
                {
                    resources_types.Add(val);
                }
                else if (key.StartsWith("conformity"))
                {
                    conformity = 1;
                }

                //array to string
                //string resources_types_str = string.Join(" ", resources_types.ToArray());

                //    elif tag.startswith('action'):
                //        if tag.startswith('action:view'):
                //            actions[tag] = u'View'
                //        elif tag.startswith('action:download'):
                //            actions[tag] = u'Download'
                //        elif tag.startswith('action:other'):
                //            actions[tag] = u'Other action'
                //        # Test : to be removed eventually
                //        else:
                //            actions[tag] = u'fonction get_tag à revoir'
                //            self.dockwidget.txt_input.setText(tag)
                //    # resources type
                //    elif tag.startswith('type'):
                //        if tag.startswith('type:vector'):
                //            resources_types[tag] = u'Vecteur'
                //        elif tag.startswith('type:raster'):
                //            resources_types[tag] = u'Raster'
                //        elif tag.startswith('type:resource'):
                //            resources_types[tag] = u'Ressource'
                //        elif tag.startswith('type:service'):
                //            resources_types[tag] = u'Service géographique'
                //    elif tag.startswith('conformity'):
                //        conformity = 1


            }
        }

        
        
    }
}
