using System;
using System.Globalization;
using System.IO;
using System.Linq;
using ArcGIS.Core.CIM;
using ArcGIS.Core.Data;
using ArcGIS.Core.Geometry;
using ArcGIS.Desktop.Framework.Dialogs;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Mapping;
using Isogeo.Utils.LogManager;
using ServiceType = Isogeo.MapCore.DataType.ServiceType;


namespace Isogeo.Models.MapFunctions
{
    internal static class MapFunctions
    {

        private static void DisplayMessage(string message)
        {
            MessageBox.Show(message, "Isogeo");
            Log.Logger.Info(message);
        }

        public static string EnvelopeToString(Envelope envelope)
        {
            return envelope.XMin.ToString(CultureInfo.InvariantCulture).Replace(',', '.') + "," +
                   envelope.YMin.ToString(CultureInfo.InvariantCulture).Replace(',', '.') + "," +
                   envelope.XMax.ToString(CultureInfo.InvariantCulture).Replace(',', '.') + "," +
                   envelope.YMax.ToString(CultureInfo.InvariantCulture).Replace(',', '.');
        }

        public static string GetFirstLayerExtend()
        {
            foreach (var layer in MapView.Active.Map.Layers)
            {
                if (layer.IsVisible)
                {
                    return EnvelopeToString(layer.QueryExtent());
                }
            }
            return null;
        }

        public static string GetMapExtent()
        {
            return EnvelopeToString(MapView.Active.Extent);
        }

        private static Geodatabase LoadGeoDatabase(ServiceType serviceType)
        {
            var suffix = Path.GetExtension(serviceType.url)?.ToLower();
            dynamic database;
            if (suffix == ".sde")
                database = new DatabaseConnectionFile(new Uri(serviceType.url));
            else if (suffix == ".gdb")
                database = new FileGeodatabaseConnectionPath(new Uri(serviceType.url));
            else
                database = new ServiceConnectionProperties(new Uri(serviceType.url));
            return new Geodatabase(database);
        }

        private static bool AddLayerFromGeoDatabase(ServiceType serviceType)
        {
            using (var geoDb = LoadGeoDatabase(serviceType))
            {
                var defNames = geoDb.GetDefinitions<FeatureClassDefinition>()
                    .Select(def => def.GetName()).ToArray();
                foreach (var defName in defNames)
                {

                    if (defName.Equals(serviceType.name))
                    {
                        using (var featureClass = geoDb.OpenDataset<FeatureClass>(defName))
                        {
                            LayerFactory.Instance.CreateFeatureLayer(featureClass, MapView.Active.Map);
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        private static void CheckErrorFile(ServiceType serviceType)
        {
            if (!(Directory.Exists(serviceType.url) ||
                  File.Exists(serviceType.url)))
            {
                DisplayMessage(Language.Resources.Message_Data_file_not_found + ": " + '"' + serviceType.url + '"');
                throw new FileNotFoundException();
            }

            //var tableName = serviceType.name;

            /*if (tableName == null)
            {
                DisplayMessage(Language.Resources.Message_Data_table_undefined);
                Log.Logger.Error(Language.Resources.Message_Data_table_undefined);
                return false;
            }*/
        }

        public static bool AddGeoDatabaseLayer(ServiceType serviceType)
        {
            bool find;
            switch (serviceType?.type.ToUpper())
            {
                case "ARCSDE":
                    CheckErrorFile(serviceType);
                    AddLayerFromGeoDatabase(serviceType);
                    find = true;
                    break;
                case "FILEGDB":
                    CheckErrorFile(serviceType);
                    AddLayerFromGeoDatabase(serviceType);
                    find = true;
                    break;
                default:
                    find = false;
                    break;
            }
            return find;
        }

        public static bool AddFileLayer(ServiceType serviceType)
        {
            bool find;
            switch (serviceType?.type.ToUpper())
            {
                case "SHP":
                    find = true;
                    break;
                case "RASTER":
                    find = true;
                    break;
                default:
                    find = false;
                    break;
            }

            if (find)
            {
                CheckErrorFile(serviceType);
                LayerFactory.Instance.CreateLayer(new Uri(serviceType.url), MapView.Active.Map, 0, serviceType.title);
            }
            return find;
        }

        /// <summary>
        /// In theory : Transform WMSServer URL to correct URL. 
        ///  Code from Isogeo Arcmap plugin, don't know if it's really efficiency or useful (so not used currently)
        /// </summary>
        /// <param name="url">WMS URL</param>
        /// <returns></returns>
        private static string FormatWmsServerUrl(string url)
        {
            string retUrl;
            var startParam = url.LastIndexOf('?') + 1;
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

        public static bool AddCimServiceLayer(ServiceType serviceType)
        {
            var serverConnection = new CIMInternetServerConnection { URL = serviceType?.url };
            dynamic connection = null;
            switch (serviceType?.type.ToUpper())
            {
                case "WMS":
                    connection = new CIMWMSServiceConnection { ServerConnection = serverConnection };
                    //IDictionary<string, object> wmsParams = new Dictionary<string, object>();
                    //wmsParams.Add("CQL_FILTER", "ingestion='myImage'");
                    //((CIMWMSServiceConnection) connection).MapParameters = wmsParams;
                    break;
                case "WMTS":
                    connection = new CIMWMTSServiceConnection { ServerConnection = serverConnection };
                    break;
                case "WCS":
                    connection = new CIMWCSServiceConnection { ServerConnection = serverConnection };
                    break;
                case "WFS":
                    connection = new CIMWFSServiceConnection { ServerConnection = serverConnection };
                    break;
            }

            dynamic layer;
            if (connection == null)
            {
                if (serviceType?.url == null) return false;
                layer = LayerFactory.Instance.CreateLayer(new Uri(serviceType.url + '/' + serviceType.name),
                    MapView.Active.Map);
                if (layer != null) return true;
                layer = LayerFactory.Instance.CreateLayer(new Uri(serviceType.url + '/' + serviceType.title),
                    MapView.Active.Map);
                if (layer != null) return true; 
                layer = LayerFactory.Instance.CreateLayer(new Uri(serviceType.url), 
                    MapView.Active.Map);
                return layer != null;
            }
            layer =  LayerFactory.Instance.CreateLayer(connection, MapView.Active.Map);
            return layer != null;
        }

        private static bool CheckErrorServiceType(ServiceType serviceType)
        {
            Log.Logger.Info("Sheet - name: " + serviceType?.name + " url: " + serviceType?.url + " id: "
                            + serviceType?.id + " creator: " + serviceType?.creator + "type: " + serviceType?.type +
                            " title: " + serviceType?.title);
            if (serviceType?.url == null || string.IsNullOrWhiteSpace(serviceType.url))
            {
                DisplayMessage(Language.Resources.Error_bad_metadata);
                return false;
            }
            return true;
        }

        public static async void AddLayer(ServiceType serviceType)
        {
            Log.Logger.Info("Add Layer");

            if (serviceType != null && serviceType.type?.ToUpper() == "ARCSDE")
                serviceType.url = Variables.configurationManager?.config?.fileSde;

            if (!CheckErrorServiceType(serviceType))
                return;
            try
            {
                await QueuedTask.Run(() =>
                {
                    if (AddFileLayer(serviceType))
                    {
                        Log.Logger.Info("END - Add Layer");
                        return;
                    }

                    if (AddGeoDatabaseLayer(serviceType))
                    {
                        Log.Logger.Info("END - Add Layer");
                        return;
                    }

                    if (!AddCimServiceLayer(serviceType))
                        DisplayMessage(Language.Resources.Message_Data_Error);
                    Log.Logger.Info("END - Add Layer");
                });
            }
            catch (FileNotFoundException)
            {
                Log.Logger.Info("END - Add Layer");
            }
            catch (Exception ex)
            {
                DisplayMessage(Language.Resources.Message_Data_Error);
                Log.Logger.Error("Error Add Layer - " + 
                                 ex.Message + " " + 
                                 serviceType?.url + " " +  
                                 serviceType?.id + " " + 
                                 serviceType?.name + " " + 
                                 serviceType?.title + " " + 
                                 serviceType?.type + " " + 
                                 serviceType?.creator);
            }
            Log.Logger.Info("END - Add Layer");
        }
    }
}
