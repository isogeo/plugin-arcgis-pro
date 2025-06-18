#nullable enable
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using ArcGIS.Core.Geometry;
using ArcGIS.Core.Internal.Geometry;
using ArcGIS.Desktop.Framework.Dialogs;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Mapping;
using Isogeo.Map.MapLayerFactory;
using Isogeo.Map.Models;
using Isogeo.Utils.LogManager;

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

        private static void AddGeoDatabaseLayer(IsogeoData isogeoData)
        {
            var layerFactory = new GeoDatabaseLayerFactory();
            layerFactory.CreateLayerOnMap(isogeoData);
        }

        private static void AddFileLayer(IsogeoData isogeoData)
        {
            var layerFactory = new FileLayerFactory();
            layerFactory.CreateLayerOnMap(isogeoData);
        }

        private static void AddServiceLayer(IsogeoData? data)
        {
            if (data?.Url == null || data.Type == null)
                return;

            switch (data.Type?.ToUpper())
            {
                case "ETS":
                    var layerFactory = new EtsLayerFactory();
                    layerFactory.CreateLayerOnMap(data);
                    break;
                case "WMS":
                case "WMTS":
                    var webLayerFactory = new GenericWebMapLayerFactory();
                    webLayerFactory.CreateLayerOnMap(data);
                    break;
                case "WFS":
                    var wfsLayerFactory = new WfsLayerFactory();
                    wfsLayerFactory.CreateLayerOnMap(data);
                    break;
                default:
                    var genericLayerFactory = new GenericServiceLayerFactory();
                    genericLayerFactory.CreateLayerOnMap(data);
                    break;
            }
        }

        private static bool CheckAddLayerRequestValidity(IsogeoData data)
        {
            if (!MapHelper.HasActiveMap())
            {
                DisplayMessage(Language.Resources.Message_Missing_Map);
                return false;
            }

            if (MapHelper.IsIsogeoDataUsingSdeFileButMissingFilePath(data))
            {
                DisplayMessage(Language.Resources.Message_Data_sde_not_configured);
                return false;
            }

            if (MapHelper.IsogeoDataHasInvalidUrl(data))
            {
                DisplayMessage(Language.Resources.Error_bad_metadata);
                return false;
            }
            return true;
        }

        public Task AddLayer(IsogeoData isogeoData)
        {
            Log.Logger.Info("START - Add Layer : " + isogeoData?.Name + " url: " + isogeoData?.Url + " id: "
                            + isogeoData?.Id + " creator: " + isogeoData?.Creator + "type: " + isogeoData?.Type +
                            " title: " + isogeoData?.Title);

            if (!CheckAddLayerRequestValidity(isogeoData))
                return Task.CompletedTask;

            var metadataType = isogeoData?.Type?.ToUpper();

            try
            {
                switch (metadataType)
                {
                    case ("SHP" or "RASTER"):
                        AddFileLayer(isogeoData);
                        break;
                    case ("ARCSDE" or "FILEGDB" or "POSTGIS"):
                        AddGeoDatabaseLayer(isogeoData);
                        break;
                    default:
                        AddServiceLayer(isogeoData);
                        break;
                }
            }
            catch (Exception ex)
            {
                var errorMessage = metadataType switch
                {
                    "ARCSDE" or "FILEGDB" or "POSTGIS" => Language.Resources.Error_Read_Data_SDE,
                    _ => Language.Resources.Message_Data_Error
                };

                DisplayMessage(errorMessage);
                Log.Logger.Error($"Error Add Layer - {ex.Message}");
            }
            return Task.CompletedTask;
        }
    }
}
