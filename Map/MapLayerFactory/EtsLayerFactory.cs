using System;
using ArcGIS.Desktop.Mapping;
using Isogeo.Map.MapLayerFactory.Contracts;
using Isogeo.Map.Models;

namespace Isogeo.Map.MapLayerFactory
{
    internal class EtsLayerFactory : IMapLayerFactory
    {
        public void CreateLayerOnMap(IsogeoData isogeoData)
        {
            var uri = new Uri($"{isogeoData.Url}/{isogeoData.Name}");
            LayerFactory.Instance.CreateLayer(uri, MapView.Active.Map);
        }
    }
}
