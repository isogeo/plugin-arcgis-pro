using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IsogeoLibrary.API
{
    public class SearchList
    {
        public String filter;
        public String query="";
        public List<Objects.comboItem> lstItem = new List<Objects.comboItem>();
        public Boolean order=false;
        //public List<Objects.comboItem> lstKeyword = new List<Objects.comboItem>();
        //public List<Objects.comboItem> lstKeywordInspire = new List<Objects.comboItem>();
        //public List<Objects.comboItem> lstFormat = new List<Objects.comboItem>();
        //public List<Objects.comboItem> lstCoordinateSystem = new List<Objects.comboItem>();
        //public List<Objects.comboItem> lstOwner = new List<Objects.comboItem>();
        //public List<Objects.comboItem> lstAction = new List<Objects.comboItem>();
        //public List<Objects.comboItem> lstContact = new List<Objects.comboItem>();
        //public List<Objects.comboItem> lstLicence = new List<Objects.comboItem>();

        public SearchList(String filter,Boolean order)
        {
            this.filter = filter;
            this.order = order;
            this.query = "";
        }
    }
}
