using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ArcGIS.Core.CIM;
using ArcGIS.Core.Data;
using ArcGIS.Core.Geometry;
using ArcGIS.Core.Internal.Geometry;
using ArcGIS.Desktop.Framework.Dialogs;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Mapping;
using Isogeo.Utils.LogManager;
using ServiceType = Isogeo.Map.DataType.ServiceType;

namespace Isogeo.Map
{
    public class MapManager : IMapManager
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

        private static string GetExtendWgs84(Envelope envelope)
        {
            var ptMin = MapPointBuilder.CreateMapPoint(envelope.XMin, MapView.Active.Extent.YMin, envelope.SpatialReference);
            var ptMax = MapPointBuilder.CreateMapPoint(envelope.XMax, MapView.Active.Extent.YMax, envelope.SpatialReference);
            var geometryMin = GeometryEngine.Instance.Project(ptMin, SpatialReferences.WGS84);
            var geometryMax = GeometryEngine.Instance.Project(ptMax, SpatialReferences.WGS84);
            return geometryMin.Extent.XMin.ToString(CultureInfo.InvariantCulture) + "," +
                    geometryMin.Extent.YMin.ToString(CultureInfo.InvariantCulture) + "," +
                    geometryMax.Extent.XMin.ToString(CultureInfo.InvariantCulture) + "," +
                    geometryMax.Extent.YMin.ToString(CultureInfo.InvariantCulture);
        }

        private static async Task<Envelope> StringToEnvelope(string envelope)
        {
            var list = envelope.Split(',');
            var doubleList = list.Select(item => double.Parse(item, CultureInfo.InvariantCulture)).ToList();
            return await QueuedTask.Run(() =>
                EnvelopeBuilder.CreateEnvelope(doubleList[0],
                    doubleList[1],
                    doubleList[2],
                    doubleList[3],
                    SpatialReferences.WGS84));
        }

        public async Task SetMapExtent(string envelope)
        {
            try
            {
                var newEnvelope = await StringToEnvelope(envelope);
                await MapView.Active.ZoomToAsync(newEnvelope, TimeSpan.FromSeconds(0.5));
            }
            catch (Exception e)
            {
                Log.Logger.Error(e.Message);
                MessageBox.Show(Language.Resources.Error_extent_map, "Isogeo");
            }
        }

        public string GetMapExtent()
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
            var suffix = Path.GetExtension(serviceType.Url)?.ToLower();
            dynamic database = suffix switch
            {
                ".sde" => new DatabaseConnectionFile(new Uri(serviceType.Url)),
                ".gdb" => new FileGeodatabaseConnectionPath(new Uri(serviceType.Url)),
                _ => new ServiceConnectionProperties(new Uri(serviceType.Url))
            };
            return new Geodatabase(database);
        }

        private static void AddLayerFromGeoDatabase(ServiceType serviceType)
        {
            using (var geoDb = LoadGeoDatabase(serviceType))
            {
                var fc = geoDb.OpenDataset<FeatureClass>(serviceType.Name);
                var layerParams = new FeatureLayerCreationParams(fc)
                {
                    MapMemberPosition = MapMemberPosition.AddToTop
                };
                LayerFactory.Instance.CreateLayer<FeatureLayer>(layerParams, MapView.Active.Map);
            }
        }

        private static void EnsureFileExists(string path)
        {
            if (!(Directory.Exists(path) || File.Exists(path)))
            {
                DisplayMessage(Language.Resources.Message_Data_file_not_found + ": " + '"' + path + '"');
                throw new FileNotFoundException();
            }
        }

        private static void AddGeoDatabaseLayer(ServiceType serviceType)
        {
            EnsureFileExists(serviceType.Url);
            AddLayerFromGeoDatabase(serviceType);
        }

        private static void AddFileLayer(ServiceType serviceType)
        {
            EnsureFileExists(serviceType.Url);
            LayerFactory.Instance.CreateLayer(new Uri(serviceType.Url), MapView.Active.Map, 0, serviceType.Title);
        }

        private bool TryAddServiceLayer(ServiceType serviceType)
        {
            var serverConnection = new CIMInternetServerConnection { URL = serviceType?.Url };
            dynamic connection = serviceType?.Type?.ToUpper() switch
            {
                "WMS" => new CIMWMSServiceConnection { ServerConnection = serverConnection },
                "WMTS" => new CIMWMTSServiceConnection { ServerConnection = serverConnection },
                "WCS" => new CIMWCSServiceConnection { ServerConnection = serverConnection },
                "WFS" => new CIMWFSServiceConnection { ServerConnection = serverConnection },
                _ => null
            };

            dynamic layer;
            if (connection == null)
            {
                if (serviceType?.Url == null)
                {
                    Log.Logger.Debug("Add Cim Service Layer - Undefined service URL");
                    return false;
                }

                var urls = new[]
                {
                    $"{serviceType.Url}/{serviceType.Name}",
                    $"{serviceType.Url}/{serviceType.Title}",
                    serviceType.Url
                };

                foreach (var url in urls)
                {
                    Log.Logger.Debug($"Add Cim Service Layer - Try with {url}");
                    layer = LayerFactory.Instance.CreateLayer(new Uri(url), MapView.Active.Map);
                    if (layer != null)
                        return true;
                }
                return false;
            }
            Log.Logger.Debug($"Add Cim Service Layer - {serviceType.Type.ToUpper()}");
            Log.Logger.Debug("Add Cim Service Layer - Connection already existing");
            layer = LayerFactory.Instance.CreateLayer(connection, MapView.Active.Map);
            return layer != null;
        }

        private static bool CheckServiceTypeValidity(ServiceType serviceType)
        {
            Log.Logger.Info("Sheet - name: " + serviceType?.Name + " url: " + serviceType?.Url + " id: "
                            + serviceType?.Id + " creator: " + serviceType?.Creator + "type: " + serviceType?.Type +
                            " title: " + serviceType?.Title);

            if ((serviceType.Type?.ToUpper() == "ARCSDE" || serviceType.Type?.ToUpper() == "POSTGIS")
                && string.IsNullOrWhiteSpace(serviceType.Url))
            {
                DisplayMessage(Language.Resources.Message_Data_sde_not_configured);
                return false;
            }

            if (serviceType?.Url == null || string.IsNullOrWhiteSpace(serviceType.Url))
            {
                DisplayMessage(Language.Resources.Error_bad_metadata);
                return false;
            }
            return true;
        }

        public Task AddLayer(ServiceType serviceType)
        {
            Log.Logger.Info($"START - Add Layer {serviceType?.Title}");

            if (!CheckServiceTypeValidity(serviceType))
                return Task.CompletedTask;

            try
            {
                var serviceTypeType = serviceType?.Type?.ToUpper();
                switch (serviceTypeType)
                {
                    case ("SHP" or "RASTER"):
                        Log.Logger.Debug($"Add File Layer - {serviceTypeType}");
                        AddFileLayer(serviceType);
                        Log.Logger.Info("END - Add Layer");
                        return Task.CompletedTask;
                    case ("ARCSDE" or "FILEGDB" or "POSTGIS"):
                        try
                        {
                            Log.Logger.Debug($"Add GeoDatabase Layer - {serviceTypeType}");
                            AddGeoDatabaseLayer(serviceType);
                            Log.Logger.Info("END - Add Layer");
                            return Task.CompletedTask;
                        }
                        catch (FileNotFoundException)
                        {
                            Log.Logger.Error("Error Add Layer - File not found");
                        }
                        catch (Exception ex)
                        {
                            DisplayMessage(Language.Resources.Error_Read_Data_SDE);
                            Log.Logger.Error("Error Add Layer - " +
                                             ex.Message + " " +
                                             serviceType?.Url + " " +
                                             serviceType?.Id + " " +
                                             serviceType?.Name + " " +
                                             serviceType?.Title + " " +
                                             serviceType?.Type + " " +
                                             serviceType?.Creator);
                        }
                        return Task.CompletedTask;
                }

                if (!TryAddServiceLayer(serviceType))
                    DisplayMessage(Language.Resources.Message_Data_Error);
                Log.Logger.Info("END - Add Layer");
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
                                 serviceType?.Url + " " +
                                 serviceType?.Id + " " +
                                 serviceType?.Name + " " +
                                 serviceType?.Title + " " +
                                 serviceType?.Type + " " +
                                 serviceType?.Creator);
            }

            return Task.CompletedTask;
        }
    }
}
