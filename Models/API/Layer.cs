using System.Collections.Generic;

namespace Isogeo.Models.API
{
    public class Layer
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

        public List<Title> titles
        {
            get;
            set;
        }
    }
}
