using System.Collections.Generic;
using Isogeo.Models.Filters;

namespace Isogeo.Models.API
{
    public class SearchList
    {
        public string filter;
        public string query;
        public List<FilterItem> lstItem = new List<FilterItem>();
        public bool order;

        public SearchList(string filter, bool order)
        {
            this.filter = filter;
            this.order = order;
            this.query = "";
        }
    }
}
