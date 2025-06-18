using System;
using System.IO;
using ArcGIS.Core.Data;
using ArcGIS.Desktop.Framework.Dialogs;
using ArcGIS.Desktop.Mapping;
using Isogeo.Map.MapLayerFactory.Contracts;
using Isogeo.Map.Models;
using Isogeo.Utils.LogManager;

namespace Isogeo.Map.MapLayerFactory
{
    public class GeoDatabaseLayerFactory : IMapLayerFactory
    {
        private static void DisplayMessage(string message)
        {
            MessageBox.Show(message, "Isogeo");
            Log.Logger.Info(message);
        }

        private static void EnsureFileExists(string path)
        {
            if (!(Directory.Exists(path) || File.Exists(path)))
            {
                DisplayMessage(Language.Resources.Message_Data_file_not_found + ": " + '"' + path + '"');
                throw new FileNotFoundException();
            }
        }

        private static Geodatabase? LoadGeoDatabase(IsogeoData isogeoData)
        {
            var suffix = Path.GetExtension(isogeoData.Url).ToLower();

            return suffix switch
            {
                ".sde" => new Geodatabase(new DatabaseConnectionFile(new Uri(isogeoData.Url))),
                ".gdb" => new Geodatabase(new FileGeodatabaseConnectionPath(new Uri(isogeoData.Url))),
                _ => null
            };
        }

        private static void AddLayerFromGeoDatabase(IsogeoData isogeoData)
        {
            using (var geoDb = LoadGeoDatabase(isogeoData))
            {
                var fc = geoDb.OpenDataset<FeatureClass>(isogeoData.Name);
                var layerParams = new FeatureLayerCreationParams(fc)
                {
                    MapMemberPosition = MapMemberPosition.AddToTop
                };
                LayerFactory.Instance.CreateLayer<FeatureLayer>(layerParams, MapView.Active.Map);
            }
        }

        public void CreateLayerOnMap(IsogeoData isogeoData)
        {
            EnsureFileExists(isogeoData.Url);
            AddLayerFromGeoDatabase(isogeoData);
        }
    }
}
