using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IsogeoLibrary.API
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
