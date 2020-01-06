﻿using System;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Arcgis_Pro_Isogeo;
using UI = Arcgis_Pro_Isogeo.UI;

//using System.Windows.Forms;
//using ESRI.ArcGIS.Carto;

namespace IsogeoLibrary
{
    public static class Variables
    {
        public static String _assemblyName = "Arcgis-Pro-Isogeo";

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



        public static UI.Search.AdvancedSearch.AdvancedSearchItem advancedSearchItem_geographicFilter; 

        public static API.RestFunctions restFunctions;
        public static API.Token token;
        public static String encryptCode = "alo(-'oàkd:jdthe";
        public static API.Search search=new API.Search();
        public static API.SearchLists searchLists;
        public static Boolean ListLoading = false;
        public static Boolean TranslateInProgress = false;
        public static ComboBox CmbLang;

        public static Boolean haveResult = false;

        public static Boolean wfsIdFInd = false;

        // TODO Implement it
        //public static List<ILayer> layersVisible = new List<ILayer>();

        public static API.Result currentResult;

        public static int nbResult = 10;

    }
    
}
