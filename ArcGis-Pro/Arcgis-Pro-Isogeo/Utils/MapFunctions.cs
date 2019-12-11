//using ESRI.ArcGIS.ArcMapUI;
//using ESRI.ArcGIS.Carto;
//using ESRI.ArcGIS.Display;
//using ESRI.ArcGIS.esriSystem;
//using ESRI.ArcGIS.Geometry;
//using ESRI.ArcGIS.GISClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
//using ESRI.ArcGIS.Geodatabase;
using System.IO;
using ArcGIS.Core.Geometry;
using ArcGIS.Desktop.Mapping;

namespace ArcMapAddinIsogeo.Utils
{
    // TODO
    class MapFunctions
    {
       /*
        /// <summary>
        /// Méthode qui retourne le système de coordonnée du document (pas sur sur que ce soit ca au vu du code crée apres compilation)
        /// </summary>
        /// <returns>le nom du système de coord</returns>
        public static string GetCoordSys()
        {
            try
            {
                var a = ArcGIS.Core.Geometry.SpatialReference();
                ArcGIS.Desktop.Core.G
                ArcGIS.Core.Geometry.SpatialReference.
                String srs_map = 
                String srs_map = "EPSG:" + ArcMap.Document.ActiveView.FocusMap.SpatialReference.FactoryCode;
                return srs_map;
            }
            catch
            {
            }
            Type factoryType = Type.GetTypeFromProgID("esriGeometry.SpatialReferenceEnvironment");
            object obj = Activator.CreateInstance(factoryType);
            ISpatialReferenceFactory3 spatialReferenceFactory = obj as ISpatialReferenceFactory3;
            ISpatialReference spatialReference = spatialReferenceFactory.CreateSpatialReference(4326);
            ISpatialReferenceInfo spatialReferenceInfo = spatialReference;
            return "EPSG:" + spatialReferenceInfo.FactoryCode.ToString();

            
            
        }

        public static String getMapExtent()
        {
            IActiveView activeView = (IActiveView)ArcMap.Document.ActiveView.FocusMap;
            IEnvelope mapEnvelope = activeView.Extent;
            if (mapEnvelope.SpatialReference!=null)
            {
                if (mapEnvelope.SpatialReference.FactoryCode != 4326)
                {
                    ISpatialReferenceFactory srFactory = new SpatialReferenceEnvironmentClass();
                    //IProjectedCoordinateSystem pcs = srFactory.CreateProjectedCoordinateSystem((int)esriSRProjCSType.esriSRGeoCS_);
                    //IProjectedCoordinateSystem pcs = srFactory.CreateProjectedCoordinateSystem(102100);                
                    IGeographicCoordinateSystem gcs = srFactory.CreateGeographicCoordinateSystem(4326);
                    //pcs.SetFalseOriginAndUnits(0, 0, 1000);
                    //ISpatialReference sr2;
                    //sr2 = pcs;
                    mapEnvelope.Project(gcs);
                }

            }
                        

            String mapExgtent = mapEnvelope.XMin.ToString().Replace(',', '.') + "," + mapEnvelope.YMin.ToString().Replace(',', '.') + "," + mapEnvelope.XMax.ToString().Replace(',', '.') + "," + mapEnvelope.YMax.ToString().Replace(',', '.');
            return mapExgtent;
        }

        public static String getLayerExtent(ILayer layer)
        {
            IEnvelope mapEnvelope = layer.AreaOfInterest.Envelope;
            if (mapEnvelope.SpatialReference.FactoryCode != 4326)
            {
                ISpatialReferenceFactory srFactory = new SpatialReferenceEnvironmentClass();
                IGeographicCoordinateSystem gcs = srFactory.CreateGeographicCoordinateSystem(4326);
                mapEnvelope.Project(gcs);
            }
            String mapExgtent = mapEnvelope.XMin.ToString().Replace(',', '.') + "," + mapEnvelope.YMin.ToString().Replace(',', '.') + "," + mapEnvelope.XMax.ToString().Replace(',', '.') + "," + mapEnvelope.YMax.ToString().Replace(',', '.');
            return mapExgtent;
        }
        

        public static void AddLayer(DataType.ServiceType serviceType)
        {
            IActiveView activeView = (IActiveView)ArcMap.Document.ActiveView.FocusMap;
            switch (serviceType.type.ToUpper())
            {
                case "WMS":
                    ILayer wmsLayer = OpenWMSLayer(serviceType.url, serviceType.name, serviceType.title);
                    if (wmsLayer != null)
                    {
                        ArcMap.Document.ActiveView.FocusMap.AddLayer(wmsLayer);
                        
                        
                    }
                    break;
                case "WFS":
                    ILayer wfsLayer = OpenWMSLayer(serviceType.url, serviceType.name, serviceType.title);
                    if (wfsLayer != null)
                    {
                        ArcMap.Document.ActiveView.FocusMap.AddLayer(wfsLayer);                        
                    }
                    break;
                case "SHP":
                    //String test="C:/isogeo/data/testlandxml_controle.shp";
                    //serviceType.url=test;
                    if (System.IO.File.Exists(serviceType.url))
                    {

                        IWorkspaceFactory workspaceFactory = new ESRI.ArcGIS.DataSourcesFile.ShapefileWorkspaceFactoryClass();
                        String dir = System.IO.Path.GetDirectoryName(System.IO.Path.GetFullPath(serviceType.url));
                        IFeatureWorkspace featureWorkspace = (IFeatureWorkspace)workspaceFactory.OpenFromFile(dir, 0);
                        IFeatureClass featureClass = featureWorkspace.OpenFeatureClass(System.IO.Path.GetFileName(serviceType.url));
                        IGeoFeatureLayer pGeoFeatureLayer;
                        pGeoFeatureLayer = new FeatureLayerClass();
                        pGeoFeatureLayer.Name = featureClass.AliasName;
                        pGeoFeatureLayer.FeatureClass = featureClass;

                        ArcMap.Document.ActiveView.FocusMap.AddLayer((ILayer)pGeoFeatureLayer);
                        


                    }
                    else
                    {                        
                        MessageBox.Show(Variables.localisationManager.getValue(Localization.LocalizationItem.Message_Data_file_not_found) + " ('" + serviceType.url + "')", "Isogeo");
                    }
                    break;
                case "ARCSDE":
                    try
                    {

                        String pathArcSDE = serviceType.url;//"C:\\isogeo\\test_jdu.gdb - Copie\\test_jdu.gdb";
                        String tableNameArcSDE = serviceType.name;

                        //tableNameArcSDE = "sig.zone_etude";
                        //pathArcSDE="C:\\isogeo\\Connection to 10.1.0.81.sde";
                        pathArcSDE=Variables.configurationManager.config.fileSDE;
                        if (tableNameArcSDE=="")
                        {
                            MessageBox.Show(Variables.localisationManager.getValue(Localization.LocalizationItem.Message_Data_table_undefined), "Isogeo");
                            break;
                        }

                        if (pathArcSDE== "")
                        {
                            MessageBox.Show(Variables.localisationManager.getValue(Localization.LocalizationItem.Message_Data_sde_not_configured), "Isogeo");
                            break;
                        }

                        if (File.Exists(pathArcSDE)==false)
                        {
                            MessageBox.Show(Variables.localisationManager.getValue(Localization.LocalizationItem.Message_Data_file_not_found) + " ('" + pathArcSDE + "')", "Isogeo");
                            break;
                        }

                        Type factoryTypeArcSDE = Type.GetTypeFromProgID("esriDataSourcesGDB.SdeWorkspaceFactory");
                        IWorkspaceFactory workspaceFactoryArcSDE = (IWorkspaceFactory)Activator.CreateInstance
                            (factoryTypeArcSDE);
                        

                        IWorkspace workspaceArcSDE = workspaceFactoryArcSDE.OpenFromFile(pathArcSDE, 0);
                        IFeatureWorkspace featureWorkspaceArcSDE = (IFeatureWorkspace)workspaceArcSDE;

                        IFeatureClass featureClassArcSDE = featureWorkspaceArcSDE.OpenFeatureClass(tableNameArcSDE);

                        IGeoFeatureLayer pGeoFeatureLayerArcSDE;
                        pGeoFeatureLayerArcSDE = new FeatureLayerClass();
                        pGeoFeatureLayerArcSDE.Name = featureClassArcSDE.AliasName;
                        pGeoFeatureLayerArcSDE.FeatureClass = featureClassArcSDE;

                        ArcMap.Document.ActiveView.FocusMap.AddLayer((ILayer)pGeoFeatureLayerArcSDE);
                        


                    }
                    catch (Exception ex)
                    {
                        
                        MessageBox.Show(Variables.localisationManager.getValue(Localization.LocalizationItem.Message_Data_Error) + "\n\n" + ex.Message,"Isogeo");                        
                    }

                    break;
                case "FILEGDB":
                    try
                    {

                        String pathFILEGDB = serviceType.url;//"C:\\isogeo\\test_jdu.gdb - Copie\\test_jdu.gdb";
                        String tableNameFILEGDB = serviceType.name;


                        if (tableNameFILEGDB=="")
                        {
                            MessageBox.Show(Variables.localisationManager.getValue(Localization.LocalizationItem.Message_Data_table_undefined), "Isogeo");
                            break;
                        }

                        if (File.Exists(pathFILEGDB)==false)
                        {
                            MessageBox.Show(Variables.localisationManager.getValue(Localization.LocalizationItem.Message_Data_file_not_found) + " ('" + serviceType.url + "')", "Isogeo");
                            break;
                        }

                        Type factoryTypeFILEGDB = Type.GetTypeFromProgID("esriDataSourcesGDB.FileGDBWorkspaceFactory");
                        IWorkspaceFactory workspaceFactoryFileGDB = (IWorkspaceFactory)Activator.CreateInstance
                            (factoryTypeFILEGDB);
                        IWorkspace workspaceFILEGDB = workspaceFactoryFileGDB.OpenFromFile(pathFILEGDB, 0);
                        IFeatureWorkspace featureWorkspaceFileGDB = (IFeatureWorkspace)workspaceFILEGDB;

                        IFeatureClass featureClassFileGDB = featureWorkspaceFileGDB.OpenFeatureClass(tableNameFILEGDB);

                        IGeoFeatureLayer pGeoFeatureLayerFileGDB;
                        pGeoFeatureLayerFileGDB = new FeatureLayerClass();
                        pGeoFeatureLayerFileGDB.Name = featureClassFileGDB.AliasName;
                        pGeoFeatureLayerFileGDB.FeatureClass = featureClassFileGDB;

                        ArcMap.Document.ActiveView.FocusMap.AddLayer((ILayer)pGeoFeatureLayerFileGDB);
                        


                    }
                    catch (Exception ex)
                    {
                        
                        MessageBox.Show(Variables.localisationManager.getValue(Localization.LocalizationItem.Message_Data_Error) + "\n\n" + ex.Message,"Isogeo");                        
                    }

                    break;

                case "EFS":
                case "EMS":
                case "ETS":
                case "ESRITILESERVICE":
                case "ESRIMAPSERVICE":
                case "ESRIFEATURESERVICE":

                    try
                    {
                        Boolean isEFS = false;
                        if (serviceType.type == "EFS" || serviceType.type.ToUpper() == "ESRIFEATURESERVICE") isEFS = true;

                        int id=-1;
                        try
                        {
                            id = int.Parse(serviceType.name);
                        }
                        catch { 
                        }
                        try
                        {
                                                        int startIndex = serviceType.url.LastIndexOf("/") + 1;
                            String LayerIndice = serviceType.url.Substring(startIndex, serviceType.url.Length - startIndex);
                            id=int.Parse(LayerIndice);
                        }
                        catch
                        {

                        }

                        var layer = GetArcGISMapServiceLayer(serviceType.title, id, serviceType.url, 50, false, isEFS);

                        if (layer != null)
                            ArcMap.Document.FocusMap.AddLayer(layer);
                    }
                    catch (Exception ex)
                    {

                        MessageBox.Show(Variables.localisationManager.getValue(Localization.LocalizationItem.Message_Data_Error) + "\n\n" + ex.Message,"Isogeo");                                                
                    }
                

                    break;
                default:

                    MessageBox.Show(Variables.localisationManager.getValue(Localization.LocalizationItem.Message_Data_Type) + " ('" + serviceType.type + "')","Isogeo");                                                
                    

                    break;
            }
            activeView.Refresh();
        }

        /// <summary>
        /// Méthode qui ouvre un flux WMS et va chercher le bon layer.
        /// </summary>
        /// <param name="serviceURL">L'URL du service WMS</param>
        /// <param name="layerName">Le nom de la couche a afficher</param>
        /// <returns>un layer pour la map en cours qui correspond au layer du service</returns>
        public static ILayer OpenWMSLayer(string serviceURL, string layerName, string layerTitle)
        {
            

            String tst = formatWMSServerUrl(serviceURL);//sans quoi ca plante souvent
            IPropertySet propSet = new PropertySetClass();
            propSet.SetProperty("URL", tst);


            IWMSConnectionName connName = new WMSConnectionNameClass();
            connName.ConnectionProperties = propSet;
            IWMSGroupLayer wmsMapLayer = new WMSMapLayerClass();
            IDataLayer dataLayer = (IDataLayer)wmsMapLayer;
            try
            {
               
                if (dataLayer.get_DataSourceSupported((IName)connName))
                {
                    IName tmp = dataLayer.DataSourceName;
                }
                dataLayer.Connect((IName)connName);
                 
                IWMSServiceDescription serviceDesc = wmsMapLayer.WMSServiceDescription;
                for (int i = 0; i < serviceDesc.LayerDescriptionCount; i++)
                {
                    IWMSLayerDescription layerDesc = serviceDesc.get_LayerDescription(i);
                    ILayer newLayer = null;
                    if (layerDesc.Title == layerName)
                    {
                        String tst22 = "tst";
                    }
                    if (layerDesc.LayerDescriptionCount == 0)
                    {
                        try
                        {
                            IWMSLayer newWMSLayer = wmsMapLayer.CreateWMSLayer(layerDesc);
                            newLayer = (ILayer)newWMSLayer;
                        }
                        catch (Exception)
                        {
                        }
                        if (newLayer == null)
                        {
                            IWMSGroupLayer grpLayer = wmsMapLayer.CreateWMSGroupLayers(layerDesc);
                            newLayer = (ILayer)grpLayer;
                        }
                    }
                    else
                    {
                        IWMSLayer Layer = null;                        
                        List<IWMSLayerDescription> LLyaers = new List<IWMSLayerDescription>();
                        SearchLayer(layerDesc, ref LLyaers);
                        IWMSLayerDescription Result = LLyaers.Find((IWMSLayerDescription x) => x.Name.Equals(layerName));
                        if (Result == null)
                        {
                            Result = LLyaers.Find((IWMSLayerDescription x) => x.Title.Equals(layerName));
                        }
                        if (Result == null)
                        {
                            Result = LLyaers.Find((IWMSLayerDescription x) => x.Title.Equals(layerTitle));
                        }
                        if (Result == null)
                        {
                            Result = LLyaers.Find((IWMSLayerDescription x) => x.Title.Equals(layerTitle));
                        }
                        if (Result != null)
                        {
                            Layer = wmsMapLayer.CreateWMSLayer(Result);
                        }
                        else
                        {
                            //throw new Exception("Impossible de déterminer ou de trouver le Layer demandé");
                            MessageBox.Show(Variables.localisationManager.getValue(Localization.LocalizationItem.Message_Data_layer_not_found), "Isogeo");                                                
                            return null;
                        }
                        newLayer = (ILayer)Layer;
                    }
                    wmsMapLayer.Clear();
                    newLayer.Visible = true;
                    wmsMapLayer.InsertLayer(newLayer, 0);
                }
                ILayer result;
                if (wmsMapLayer == null || wmsMapLayer.Count == 0)
                {
                    result = null;
                    return result;
                }
                ILayer wmsLayer = (ILayer)wmsMapLayer;
                wmsLayer.Visible = true;
                if (string.IsNullOrEmpty(layerName))
                {
                    wmsLayer.Name = serviceDesc.WMSTitle;
                }
                else
                {
                    wmsLayer.Name = layerName;
                }
                result = wmsLayer;
                return result;
            }
            catch (Exception e)
            {
                MessageBox.Show(Variables.localisationManager.getValue(Localization.LocalizationItem.Message_Data_layer_not_found), "Isogeo");                                                
            }
            return null;
        }

        private static String formatWMSServerUrl(String url)
        {            
            String retUrl = String.Empty;
            int startParam = url.LastIndexOf('?') + 1;
            if (startParam > 0)
            {
                retUrl = url.Substring(0, startParam);
            }
            else
            {
                retUrl = url + "?";
            }
            


            return retUrl;
        }

        /// <summary>
        /// Méthode qui va aplatir l'arborescence d'un server WMS pour chercher une couche en particulier 
        /// </summary>
        /// <param name="layerDesc">objet cui contiens la description du server WMS (getcapabilities)</param>
        /// <param name="LLyaers">Liste de tous les layers du server</param>
        private static void SearchLayer(IWMSLayerDescription layerDesc, ref List<IWMSLayerDescription> LLyaers)
        {
            if (layerDesc.LayerDescriptionCount == 0)
            {
                LLyaers.Add(layerDesc);
                return;
            }
            for (int i = 0; i < layerDesc.LayerDescriptionCount; i++)
            {
                SearchLayer(layerDesc.get_LayerDescription(i), ref LLyaers);
            }
        }


        public static ILayer GetArcGISMapServiceLayer(string name, int visibleIndices, string url,short transparency, bool isTiled, bool isFeatureService)
        {
            ILayer outLayer = null;
            ILayer outLayerIndice = null;
            string svcName = GetServiceName(UrlDecode(url));
            string svcUrl = GetServiceUrl(UrlDecode(url));

            var gisServer = OpenConnection(svcUrl);
            

            var soName = FindServerObjectname(gisServer, svcName);
            if (soName == null)
                throw new Exception("unable to find serverobject for " + svcName);
            //si c 'est un map server
            //if (svcName.ToUpper().Contains("MAPSERVER"))
            if (isTiled == true)
            {
                if (svcName.ToUpper().Contains("MAPSERVER"))
                {
                    outLayer = GetArcGisMapServerGroupLayer(soName);
                }
                else
                {
                    outLayer = GetArcGisServiceServerGroupLayer(soName);
                }
            }
            if (isFeatureService==false)
            {
                outLayer = GetArcGisMapServerGroupLayer(soName);
            }
            //sinon c 'est un featureserver
            else
            {
                outLayer = GetArcGisServiceServerGroupLayer(soName);
            }
            if (outLayer != null)
            {
                if (!isTiled)
                {
                    var grpLayer = outLayer as ICompositeLayer;
                    Variables.wfsIdFInd = false;
                    for (int i = 0; i < grpLayer.Count; i++)
                    {
                        SetVisibility(grpLayer.get_Layer(i), visibleIndices, name,-1);
                    }
                    if (Variables.wfsIdFInd == false)
                    {
                        if (visibleIndices < grpLayer.Count && visibleIndices >-1)
                        {
                            SetVisibility(grpLayer.get_Layer(visibleIndices), visibleIndices, name, visibleIndices);
                        }
                    }
                    outLayerIndice = outLayer;
                }
                else
                {
                    outLayerIndice = outLayer;
                    
                }
                outLayerIndice.Name = name;
            }
            else
            {
                outLayerIndice = outLayer;
            }
            return outLayerIndice;
        }

        private static IAGSServerObjectName3 FindServerObjectname(IAGSServerConnection gisServer, string svcName)
        {
            var soNames = gisServer.ServerObjectNames;
            IAGSServerObjectName3 soName;
            while ((soName = (IAGSServerObjectName3)soNames.Next()) != null)
            {

                if ((soName.Type == "MapServer") && (soName.URL.ToUpper() == svcName.ToUpper()))
                {
                    return soName;
                }
                
                if ((soName.Type == "FeatureServer"))
                {
                    String soNameUrl = soName.URL.ToUpper();
                    String svcNameUrl = svcName.ToUpper();
                    //return soName;
                    if (soNameUrl.ToUpper().Equals(svcNameUrl.ToUpper()))
                    {
                        return soName;
                    }
                }

            }
            return null;
        }

        private static string GetServiceUrl(string url)
        {
            if (url.ToUpper().Contains("MAPSERVER"))
            {
                // remove the "rest" part of the url 
                int idx = url.ToString().ToUpper().IndexOf(@"/REST/")+5;
                string svcUrl = url.Substring(0, idx) + @"/services";
                return svcUrl;
            }
            else if (url.ToUpper().Contains("FEATURESERVER"))
            {
                int idx = url.ToString().ToUpper().IndexOf(@"/REST/SERVICES") + 14;
                string svcUrl = url.Substring(0, idx);
                return svcUrl;
            }
            return String.Empty;
        }

        private static ILayer GetArcGisMapServerGroupLayer(IAGSServerObjectName3 soName)
        {
            IMapServerLayer outLayer = null;
            var factory = new MapServerLayerFactory() as ILayerFactory;
            try
            {
                //create an enum of layers using the name object (will contain only a single layer)
                outLayer = factory.Create(soName).Next() as IMapServerLayer;
            }
            catch
            {
                throw;
            }
            finally
            {
                System.Runtime.InteropServices.Marshal.FinalReleaseComObject(factory);
            }
            return outLayer as ILayer;
        }

        private static void SetVisibility(ILayer lyr, int visibleIds,String title,int index)
        {
            // recurse and set visibility
            IMapServerSublayer subLayer = lyr as IMapServerSublayer;
            //ILayerDescription ild = lyr as ILayerDescription;

            if (subLayer == null)
            {

                MapServerRESTSubLayer subFeatureLayer = lyr as MapServerRESTSubLayer;
                
                if (title == lyr.Name)
                {
                    lyr.Visible = true;
                    Variables.wfsIdFInd = true;
                }
                else
                {
                    lyr.Visible = false;
                }
                if (index > -1)
                {
                    if (index == visibleIds)
                    {
                        lyr.Visible = true;
                        Variables.wfsIdFInd = true;
                    }
                }
                IFeatureLayerDefinition dd = lyr as IFeatureLayerDefinition;
                return;
            }
             
            int id = subLayer.LayerDescription.ID;

            if (visibleIds == -1)
            {
                if (title == lyr.Name)
                {
                    lyr.Visible = true;
                    Variables.wfsIdFInd = true;
                }
                else
                {
                    lyr.Visible = false;
                }
            }

            if (visibleIds==id)
            {
                lyr.Visible = true;
                Variables.wfsIdFInd = true;
            }
            else
            {
                lyr.Visible = false;
                
            }
            
            IMapServerGroupLayer gLayer = lyr as IMapServerGroupLayer;
            if (gLayer != null)
            {
                for (int i = 0; i < gLayer.Count; i++)
                    SetVisibility(gLayer.get_Layer(i), visibleIds,title,index);
            }
        }

        private static ILayer geyIlayerById(ILayer lyr, int currentId)
        {
            ILayer outLyr = null;
            // recurse and set visibility
            IMapServerSublayer subLayer = lyr as IMapServerSublayer;
            if (subLayer == null)
                return lyr;
            int id = subLayer.LayerDescription.ID;

            if (currentId == id)
                return lyr;
            

            IMapServerGroupLayer gLayer = lyr as IMapServerGroupLayer;
            if (gLayer != null)
            {
                for (int i = 0; i < gLayer.Count; i++)
                {
                    outLyr = geyIlayerById(gLayer.get_Layer(i), currentId);
                    if (outLyr != null) break; 
                }
                 
            }
            return outLyr;
        }


        private static string GetServiceName(string url)
        {
            int index = -1;

            if (url.Contains("MapServer"))
            {
                index = url.LastIndexOf("MapServer") + 9;
                String tmp = url.Substring(0, index);

                int restStartIndex = tmp.IndexOf("/rest");
                int restEndIndex = restStartIndex + 5;

                string ret = tmp.Substring(0, restStartIndex) + tmp.Substring(restEndIndex);

                return ret;

            }
            else if (url.Contains("FeatureServer"))
            {
                index = url.LastIndexOf("FeatureServer") + 13;

                return url.Substring(0, index);
            }
            return String.Empty;


        }

        public static string UrlDecode(string text)
        {
            // pre-process for + sign space formatting since System.Uri doesn't handle it
            // plus literals are encoded as %2b normally so this should be safe
            text = text.Replace("+", " ");
            return System.Uri.UnescapeDataString(text);
        }

        private static IAGSServerConnection OpenConnection(string svcUrl)
        {
            //Set connection propertyset. sample URL: http://host:port/arcgis/services.
            IPropertySet propertySet = new PropertySetClass();
            propertySet.SetProperty("url", svcUrl);

            //Open an AGS connection.
            Type factoryType = Type.GetTypeFromProgID(
                "esriGISClient.AGSServerConnectionFactory");
            IAGSServerConnectionFactory agsFactory = (IAGSServerConnectionFactory)
                Activator.CreateInstance(factoryType);
            IAGSServerConnection agsConnection = agsFactory.Open(propertySet, 0);
            return agsConnection;
        }

        private static ILayer GetArcGisServiceServerGroupLayer(IAGSServerObjectName3 soName)
        {
            ILayer iLayer;
            ILayerFactory fsLayerFactory = null; //CKE FeatureServer
            try
            {
                //create a layer factory to make a new MapServerLayer from the server object name
                //ILayerFactory msLayerFactory; //CKE MapServer

                //msLayerFactory = new MapServerLayerFactory(); //CKE MapServer
                fsLayerFactory = new FeatureServerLayerFactory(); //CKE FeatureServer
                //create an enum of layers using the name object (will contain only a single layer)
                //IEnumLayer enumLyrs = msLayerFactory.Create(soName); //CKE MapServer
                IEnumLayer enumLyrs = fsLayerFactory.Create(soName); //CKE MapServer
                //get the layer from the enum, store it in a MapServerLayer variable

                iLayer = enumLyrs.Next();

            }
            catch
            {
                throw;
            }
            finally
            {
                System.Runtime.InteropServices.Marshal.FinalReleaseComObject(fsLayerFactory);
            }
            return iLayer;
        }*/
    }
}
