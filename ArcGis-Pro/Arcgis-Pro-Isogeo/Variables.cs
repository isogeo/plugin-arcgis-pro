using System;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Arcgis_Pro_Isogeo;

//using System.Windows.Forms;
//using ESRI.ArcGIS.Carto;

namespace ArcMapAddinIsogeo
{
    public static class Variables
    {

        public static String emailSupport = "support+arcmap@isogeo.fr";
        public static String emailBodySupport = "support+arcmap@isogeo.fr";
        public static String emailSubjectSupport = "support+arcmap@isogeo.fr";


        public static Configuration.ConfigurationManager configurationManager;
        public static Localization.LocalizationManager localisationManager;
        public static DockpaneView dockableWindowIsogeo;
        public static List<Action> functionsTranslate = new List<Action>();
        public static List<Action> functionsSetlist = new List<Action>();
        public static List<ComboBox> listComboFilter = new List<ComboBox>();

        public static Boolean isFirstLoad = true;
        
        

        //public static Ui.Search.AdvancedSearch.AdvancedSreachItem advancedSreachItem_geographicFilter; 

        public static API.RestFunctions restFunctions;
        public static API.Token token;
        public static String encryptCode = "alo(-'oàkd:jdthe";
        public static API.Search search=new API.Search();
        public static API.SearchLists searchLists;
        public static Boolean ListLoading = false;
        public static Boolean TranslateInProgress = false;
        //public static ComboBox combo_lang;
        public static int currentPage=1;

        public static Boolean haveResult = false;

        public static Boolean wfsIdFInd = false;

        //public static List<ILayer> layersVisible = new List<ILayer>();

        public static API.Result currentResult;
        
        
    }
    
}
