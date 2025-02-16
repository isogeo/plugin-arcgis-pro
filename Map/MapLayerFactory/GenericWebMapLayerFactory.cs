using ArcGIS.Core.CIM;
using ArcGIS.Desktop.Mapping;
using Isogeo.Map.MapLayerFactory.Contracts;
using Isogeo.Map.Models;

namespace Isogeo.Map.MapLayerFactory
{
    public class GenericWebMapLayerFactory : IMapLayerFactory
    {
        private static void AddServiceLayerOnMap<T>(T connection) where T : CIMServiceConnection
        {
            var layerPams = new LayerCreationParams(connection);
            LayerFactory.Instance.CreateLayer<FeatureLayer>(layerPams, MapView.Active.Map);
        }

        public void CreateLayerOnMap(IsogeoData isogeoData)
        {
            var serverConnection = new CIMInternetServerConnection { URL = isogeoData.Url };
            var connectionType = isogeoData.Type.ToUpper();

            CIMServiceConnection? connection = connectionType switch
            {
                "WMS" => new CIMWMSServiceConnection { ServerConnection = serverConnection, LayerName = isogeoData.Name },
                "WMTS" => new CIMWMTSServiceConnection { ServerConnection = serverConnection, LayerName = isogeoData.Name },
                _ => null
            };

            if (connection == null) 
                return;
            AddServiceLayerOnMap(connection);
        }
    }
}
