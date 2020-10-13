﻿using System.Collections.Generic;

namespace Isogeo.Models.API
{
    public class Link
    {
        public string _id
        {
            get;
            set;
        }

        public Link link
        {
            get;
            set;
        }

        public string type
        {
            get;
            set;
        }

        public string title
        {
            get;
            set;
        }

        public string url
        {
            get;
            set;
        }

        public string kind
        {
            get;
            set;
        }

        public List<string> actions
        {
            get;
            set;
        }
    }
}