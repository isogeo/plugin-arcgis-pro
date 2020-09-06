using System;
using System.Collections.Generic;

namespace Isogeo.Models.API
{
    public class Envelope
    {

        public string type
        {
            get;
            set;
        }

        public string editionProfile
        {
            get;
            set;
        }

        public Boolean series
        {
            get;
            set;
        }

        public string geometry
        {
            get;
            set;
        }

        public int features
        {
            get;
            set;
        }

        public List<Double> box
        {
            get;
            set;
        }
        public List<List<List<double>>> coordinates
        {
            get;
            set;
        }
                        
                            
        
        
    }
}
