using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArcMapAddinIsogeo.API
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
