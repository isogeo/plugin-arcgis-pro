using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IsogeoLibrary.API
{
    public class Result
    {
        public string _id
        {
            get;
            set;
        }

        public string _created
        {
            get;
            set;
        }

        public string _modified
        {
            get;
            set;
        }

        public bool _deleted
        {
            get;
            set;
        }

        public Creator _creator
        {
            get;
            set;
        }

        public List<Keyword> keywords
        {
            get;
            set;
        }

        public string type
        {
            get;
            set;
        }

        public bool series
        {
            get;
            set;
        }

        public int scale
        {
            get;
            set;
        }

        public double distance
        {
            get;
            set;
        }

        public string validFrom
        {
            get;
            set;
        }

        public string validTo
        {
            get;
            set;
        }

        public string validityComment
        {
            get;
            set;
        }
        

        public string updateFrequency
        {
            get;
            set;
        }

        public string encoding
        {
            get;
            set;
        }

        public string collectionMethod
        {
            get;
            set;
        }

        public string collectionContext
        {
            get;
            set;
        }

        public string editionProfile
        {
            get;
            set;
        }

        public string title
        {
            get;
            set;
        }

        public string @abstract
        {
            get;
            set;
        }

        public string format
        {
            get;
            set;
        }
                        
        public string formatVersion
        {
            get;
            set;
        }

        public string language
        {
            get;
            set;
        }

        public Envelope envelope
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

        public string name
        {
            get;
            set;
        }

        public string path
        {
            get;
            set;
        }

        public string topologicalConsistency
        {
            get;
            set;

        }       
        public List<string> _abilities
        {
            get;
            set;
        }

        public List<double> bbox
        {
            get;
            set;
        }

        //public List<Link> links
        //{
        //    get;
        //    set;
        //}

        public List<Layer> layers
        {
            get;
            set;

        }

        public List<ServiceLayer> serviceLayers
        {
            get;
            set;

        }

        public IDictionary<string, string> tags
        {
            get;
            set;
        }

        public Objects.Tags tagsLists
        {
            get;
            set;
        }

        public List<ContactParent> contacts
        {
            get;
            set;

        }

        public Coordinate_system coordinate_system
        {
            get;
            set;
        }

        public List<Event> events
        {
            get;
            set;

        }

        public List<Specification> specifications
        {
            get;
            set;

        }


        public List<Condition> conditions
        {
            get;
            set;

        }

        public List<Limitation> limitations
        {
            get;
            set;

        }
        

 
    }	
}
