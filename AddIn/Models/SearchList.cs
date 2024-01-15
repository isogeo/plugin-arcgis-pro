using System.Collections.Generic;
using Isogeo.AddIn.Models.Filters.Components;

namespace Isogeo.AddIn.Models
{
    public class SearchList
    {
        public string filter;
        public string query;
        public List<FilterItem> lstItem = new();
        public bool order;

        public SearchList(string filter, bool order)
        {
            this.filter = filter;
            this.order = order;
            this.query = "";
        }
    }
}
