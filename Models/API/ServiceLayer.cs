using System.Collections.Generic;

namespace Isogeo.Models.API
{
    public class ServiceLayer
    {
        public string _id
        {
            get;
            set;
        }

        public string id
        {
            get;
            set;
        }

        public Service service
        {
            get;
            set;
        }
        
        public List<Title> titles
        {
            get;
            set;
        }
    }
}
