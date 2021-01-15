using System;

namespace Isogeo.Models.API
{
    public class Limitation
    {

        public string _id
        {
            get;
            set;
        }


        public String type
        {
            get;
            set;
        }



        public String description
        {
            get;
            set;
        }

        public string restriction
        {
            get;
            set;
        }

        public Directive directive
        {
            get;
            set;
        }

        
    }
}
