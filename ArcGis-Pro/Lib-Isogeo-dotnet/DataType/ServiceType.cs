using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArcMapAddinIsogeo.DataType
{
    public class ServiceType
    {
        public string type;
        public string title;
        public string url;
        public string name;
        public string creator;        
        public string id;


        public ServiceType(string type, string title, string url, string name, string creator, string id)
        {
            this.type = type;
            this.title = title;
            this.url = url;
            this.name = name;
            this.creator = creator;
            this.id = id;
            
        }
    }
}
