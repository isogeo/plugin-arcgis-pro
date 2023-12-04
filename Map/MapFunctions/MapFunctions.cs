using System;
using System.Globalization;
using System.IO;
using System.Linq;
using ArcGIS.Core.CIM;
using ArcGIS.Core.Data;
using ArcGIS.Core.Geometry;
using ArcGIS.Core.Internal.Geometry;
using ArcGIS.Desktop.Framework.Dialogs;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Mapping;
using Isogeo.Utils.LogManager;
using ServiceType = Isogeo.Map.DataType.ServiceType;


namespace Isogeo.Map.MapFunctions
{
    public static class MapFunctions
    {

        private static void DisplayMessage(string message)
        {
            MessageBox.Show(message, "Isogeo");
            Log.Logger.Info(message);
        }

        private static string GetExtend(Envelope envelope)
        {
            return envelope.XMin.ToString(CultureInfo.InvariantCulture) + "," +
                   envelope.YMin.ToString(CultureInfo.InvariantCulture) + "," +
                   envelope.XMax.ToString(CultureInfo.InvariantCulture) + "," +
                   envelope.YMax.ToString(CultureInfo.InvariantCulture);
        }

        // Same as GetMapExtent() but with first visible Layer of active map, not used, but keep it for example
        public static string GetFirstLayerExtend()
        {
            foreach (var layer in MapView.Active.Map.Layers)
            {
                if (layer.IsVisible)
                {
                    try
                    {
                        return QueuedTask.Run(() => layer.QueryExtent().SpatialReference.Wkid != SpatialReferences.WGS84.Wkid ?
                            GetExtendWgs84(layer.QueryExtent()) :
                            GetExtend(layer.QueryExtent())).GetAwaiter().GetResult();
                    }
                    catch (Exception e)
                    {
                        Log.Logger.Error(e.Message);
                    }
                }
            }
            return null;
        }

        private static string GetExtendWgs84(Envelope envelope)
        {
            var ptMin = MapPointBuilder.CreateMapPoint(envelope.XMin, MapView.Active.Extent.YMin, envelope.SpatialReference);
            var ptMax = MapPointBuilder.CreateMapPoint(envelope.XMax, MapView.Active.Extent.YMax, envelope.SpatialReference);
            var geometryMin = GeometryEngine.Instance.Project(ptMin, SpatialReferences.WGS84);
            var geometryMax = GeometryEngine.Instance.Project(ptMax, SpatialReferences.WGS84);
            return (geometryMin.Extent.XMin.ToString(CultureInfo.InvariantCulture) + "," +
                    geometryMin.Extent.YMin.ToString(CultureInfo.InvariantCulture) + "," +
                    geometryMax.Extent.XMin.ToString(CultureInfo.InvariantCulture) + "," +
                    geometryMax.Extent.YMin.ToString(CultureInfo.InvariantCulture));
        }

        private static Envelope StringToEnvelope(string envelope)
        {
            var list = envelope.Split(',');
            var doubleList = list.Select(item => double.Parse(item, CultureInfo.InvariantCulture)).ToList();
            return QueuedTask.Run(() =>
                EnvelopeBuilder.CreateEnvelope(doubleList[0],
                    doubleList[1],
                    doubleList[2],
                    doubleList[3],
                    SpatialReferences.WGS84)).GetAwaiter().GetResult();
        }

        public static async void SetMapExtent(string envelope)
        {
            try
            {
                await QueuedTask.Run(() =>
                {
                    var newEnvelope = StringToEnvelope(envelope);
                    return MapView.Active.ZoomToAsync(newEnvelope, TimeSpan.FromSeconds(0.5));
                });
            }
            catch (Exception e)
            {
                Log.Logger.Error(e.Message);
                MessageBox.Show(Language.Resources.Error_extent_map, "Isogeo");
            }
        }

        public static string GetMapExtent()
        {
            try
            {
                return MapView.Active.Map.SpatialReference.Wkid != SpatialReferences.WGS84.Wkid ?
                    GetExtendWgs84(MapView.Active.Extent) :
                    GetExtend(MapView.Active.Extent);
            }
            catch (Exception e)
            {
                Log.Logger.Error(e.Message);
            }
            return null;
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
                            Log.Logger.Debug($"Add Layer From GeoDatabase - {featureClass.GetName()}");
                            var layerParams = new FeatureLayerCreationParams(featureClass)
                            {
                                MapMemberPosition = MapMemberPosition.AddToBottom
                            };
                            LayerFactory.Instance.CreateLayer<FeatureLayer>(layerParams, MapView.Active.Map);
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
        }

        public static bool AddGeoDatabaseLayer(ServiceType serviceType)
        {
            bool find;
            switch (serviceType?.type.ToUpper())
            {
                case "ARCSDE":
                    Log.Logger.Debug("Add GeoDatabase Layer - ARCSDE");
                    CheckErrorFile(serviceType);
                    AddLayerFromGeoDatabase(serviceType);
                    find = true;
                    break;
                case "FILEGDB":
                    Log.Logger.Debug("Add GeoDatabase Layer - FILEGDB");
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
                    Log.Logger.Debug("Add File Layer - SHP");
                    find = true;
                    break;
                case "RASTER":
                    Log.Logger.Debug("Add File Layer - RASTER");
                    find = true;
                    break;
                default:
                    find = false;
                    break;
            }

            if (find)
            {
                CheckErrorFile(serviceType);
                Log.Logger.Debug($"Add File Layer - Uri: {serviceType.url} | Title: {serviceType.title}");
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
                    Log.Logger.Debug("Add Cim Service Layer - WMS");
                    connection = new CIMWMSServiceConnection { ServerConnection = serverConnection };
                    break;
                case "WMTS":
                    Log.Logger.Debug("Add Cim Service Layer - WMTS");
                    connection = new CIMWMTSServiceConnection { ServerConnection = serverConnection };
                    break;
                case "WCS":
                    Log.Logger.Debug("Add Cim Service Layer - WCS");
                    connection = new CIMWCSServiceConnection { ServerConnection = serverConnection };
                    break;
                case "WFS":
                    Log.Logger.Debug("Add Cim Service Layer - WFS");
                    connection = new CIMWFSServiceConnection { ServerConnection = serverConnection };
                    break;
            }

            dynamic layer;
            if (connection == null)
            {
                if (serviceType?.url == null)
                {
                    Log.Logger.Debug("Add Cim Service Layer - Undefined service URL");
                    return false;
                }

                Log.Logger.Debug($"Add Cim Service Layer - Try with {serviceType.url + '/' + serviceType.name}");
                layer = LayerFactory.Instance.CreateLayer(new Uri(serviceType.url + '/' + serviceType.name),
                    MapView.Active.Map);
                if (layer != null) return true;

                Log.Logger.Debug($"Add Cim Service Layer - Try with {serviceType.url + '/' + serviceType.title}");
                layer = LayerFactory.Instance.CreateLayer(new Uri(serviceType.url + '/' + serviceType.title),
                    MapView.Active.Map);
                if (layer != null) return true;

                Log.Logger.Debug($"Add Cim Service Layer - Try with {serviceType.url}");
                layer = LayerFactory.Instance.CreateLayer(new Uri(serviceType.url),
                    MapView.Active.Map);

                return layer != null;
            }
            Log.Logger.Debug("Add Cim Service Layer - Connection already existing");
            layer = LayerFactory.Instance.CreateLayer(connection, MapView.Active.Map);
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

        private static bool CheckSDEConnection(ServiceType serviceType)
        {
            if (serviceType.type?.ToUpper() == "ARCSDE")
            {
                if (serviceType.url.Length == 0)
                {
                    DisplayMessage(Language.Resources.Message_Data_sde_not_configured);
                    return false;
                }
            }

            return true;
        }

        public static async void AddLayer(ServiceType serviceType)
        {
            Log.Logger.Info($"START - Add Layer {serviceType?.title}");

            if (!CheckSDEConnection(serviceType))
                return;

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
                Log.Logger.Error("Error Add Layer - File not found");
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
        }
    }
}
