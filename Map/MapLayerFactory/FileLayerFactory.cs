using System;
using System.IO;
using ArcGIS.Desktop.Framework.Dialogs;
using ArcGIS.Desktop.Mapping;
using Isogeo.Map.MapLayerFactory.Contracts;
using Isogeo.Map.Models;
using Isogeo.Utils.LogManager;

namespace Isogeo.Map.MapLayerFactory
{
    public class FileLayerFactory : IMapLayerFactory
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

        public void CreateLayerOnMap(IsogeoData isogeoData)
        {
            EnsureFileExists(isogeoData.Url);
            LayerFactory.Instance.CreateLayer(new Uri(isogeoData.Url), MapView.Active.Map, 0, isogeoData.Title);
        }
    }
}
