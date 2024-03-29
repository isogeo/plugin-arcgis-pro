﻿using System;
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
            dynamic database;
            if (suffix == ".sde")
                database = new DatabaseConnectionFile(new Uri(serviceType.Url));
            else if (suffix == ".gdb")
                database = new FileGeodatabaseConnectionPath(new Uri(serviceType.Url));
            else
                database = new ServiceConnectionProperties(new Uri(serviceType.Url));
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
                    if (defName.Equals(serviceType.Name))
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
            if (!(Directory.Exists(serviceType.Url) ||
                  File.Exists(serviceType.Url)))
            {
                DisplayMessage(Language.Resources.Message_Data_file_not_found + ": " + '"' + serviceType.Url + '"');
                throw new FileNotFoundException();
            }
        }

        private bool AddGeoDatabaseLayer(ServiceType serviceType)
        {
            bool find;
            switch (serviceType?.Type.ToUpper())
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

        private bool AddFileLayer(ServiceType serviceType)
        {
            bool find;
            switch (serviceType?.Type.ToUpper())
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
                Log.Logger.Debug($"Add File Layer - Uri: {serviceType.Url} | Title: {serviceType.Title}");
                LayerFactory.Instance.CreateLayer(new Uri(serviceType.Url), MapView.Active.Map, 0, serviceType.Title);
            }
            return find;
        }

        private bool AddCimServiceLayer(ServiceType serviceType)
        {
            var serverConnection = new CIMInternetServerConnection { URL = serviceType?.Url };
            dynamic connection = null;
            switch (serviceType?.Type.ToUpper())
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
                if (serviceType?.Url == null)
                {
                    Log.Logger.Debug("Add Cim Service Layer - Undefined service URL");
                    return false;
                }

                Log.Logger.Debug($"Add Cim Service Layer - Try with {serviceType.Url + '/' + serviceType.Name}");
                layer = LayerFactory.Instance.CreateLayer(new Uri(serviceType.Url + '/' + serviceType.Name),
                    MapView.Active.Map);
                if (layer != null) return true;

                Log.Logger.Debug($"Add Cim Service Layer - Try with {serviceType.Url + '/' + serviceType.Title}");
                layer = LayerFactory.Instance.CreateLayer(new Uri(serviceType.Url + '/' + serviceType.Title),
                    MapView.Active.Map);
                if (layer != null) return true;

                Log.Logger.Debug($"Add Cim Service Layer - Try with {serviceType.Url}");
                layer = LayerFactory.Instance.CreateLayer(new Uri(serviceType.Url),
                    MapView.Active.Map);

                return layer != null;
            }
            Log.Logger.Debug("Add Cim Service Layer - Connection already existing");
            layer = LayerFactory.Instance.CreateLayer(connection, MapView.Active.Map);
            return layer != null;
        }

        private static bool CheckErrorServiceType(ServiceType serviceType)
        {
            Log.Logger.Info("Sheet - name: " + serviceType?.Name + " url: " + serviceType?.Url + " id: "
                            + serviceType?.Id + " creator: " + serviceType?.Creator + "type: " + serviceType?.Type +
                            " title: " + serviceType?.Title);
            if (serviceType?.Url == null || string.IsNullOrWhiteSpace(serviceType.Url))
            {
                DisplayMessage(Language.Resources.Error_bad_metadata);
                return false;
            }
            return true;
        }

        private static bool CheckSdeConnection(ServiceType serviceType)
        {
            if (serviceType.Type?.ToUpper() == "ARCSDE")
            {
                if (serviceType.Url.Length == 0)
                {
                    DisplayMessage(Language.Resources.Message_Data_sde_not_configured);
                    return false;
                }
            }

            return true;
        }

        public Task AddLayer(ServiceType serviceType)
        {
            Log.Logger.Info($"START - Add Layer {serviceType?.Title}");

            if (!CheckSdeConnection(serviceType))
                return Task.CompletedTask;

            if (!CheckErrorServiceType(serviceType))
                return Task.CompletedTask;

            try
            {
                if (AddFileLayer(serviceType))
                {
                    Log.Logger.Info("END - Add Layer");
                    return Task.CompletedTask;
                }

                if (AddGeoDatabaseLayer(serviceType))
                {
                    Log.Logger.Info("END - Add Layer");
                    return Task.CompletedTask;
                }

                if (!AddCimServiceLayer(serviceType))
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
