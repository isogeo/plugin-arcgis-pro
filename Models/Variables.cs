using System;
using System.Collections.Generic;
using Isogeo.Models.API;
using Isogeo.Models.Configuration;
using Isogeo.Models.Filters;
using Isogeo.Models.Network;
using Search = Isogeo.Models.API.Search;

namespace Isogeo.Models
{
    /// <summary>
    /// Singleton
    /// </summary>
    public static class Variables
    {
        public static ConfigurationManager configurationManager;

        public static List<Action> functionsSetlist = new List<Action>();
        public static List<Filters.Filters> listComboFilter = new List<Filters.Filters>();
        public static Filters.Filters geographicFilter;

        public static RestFunctions restFunctions;

        public static string encryptCode = "alo(-'oàkd:jdthe";
        public static Search search=new Search();
        public static SearchLists searchLists;
        public static bool listLoading = false;

        public static int nbResult = 10;

        public static FilterItem cmbSortingMethodSelectedItem;
        public static FilterItem cmbSortingDirectionSelectedItem;

        public static string searchText;

    }
}
