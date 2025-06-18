using System.Linq;
using ArcGIS.Core.CIM;
using ArcGIS.Desktop.Mapping;
using Isogeo.Map.MapLayerFactory.Contracts;
using Isogeo.Map.Models;

namespace Isogeo.Map.MapLayerFactory
{
    public class WfsLayerFactory : IMapLayerFactory
    {
        public void CreateLayerOnMap(IsogeoData isogeoData)
        {
            var dataset = isogeoData.Name.Contains(':') ? isogeoData.Name?.Split(':')
                .Skip(2)
                .FirstOrDefault() ?? string.Empty : isogeoData.Name;
            var cimStandardDataConnection = new CIMStandardDataConnection()
            {

                WorkspaceConnectionString = $@"SWAPXY=FALSE;SWAPXYFILTER=FALSE;URL={isogeoData.Url};VERSION=2.0.0",
                WorkspaceFactory = WorkspaceFactory.WFS,
                Dataset = dataset,
                DatasetType = esriDatasetType.esriDTFeatureClass
            };
            var layerParameters = new LayerCreationParams(cimStandardDataConnection);
            LayerFactory.Instance.CreateLayer<FeatureLayer>(layerParameters, MapView.Active.Map);
        }
    }
}
