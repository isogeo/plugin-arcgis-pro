using System;
using System.Collections.Generic;

namespace Isogeo.Models.API
{
    public class Search
    {
        //public IEnumerable<IDictionary<string, string>> tags { get; set; }
        public IDictionary<string, string> tags;
        //public String envelope { get; set; }
        //query
        //results
        public Double offset { get; set; }
        public Double limit { get; set; }
        public Double total { get; set; }
        public List<Result> results
        {
            get;
            set;
        }
    }
}
