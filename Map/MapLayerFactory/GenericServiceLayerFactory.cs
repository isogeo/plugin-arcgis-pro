using System;
using ArcGIS.Desktop.Mapping;
using Isogeo.Map.MapLayerFactory.Contracts;
using Isogeo.Map.Models;

namespace Isogeo.Map.MapLayerFactory
{
    public class GenericServiceLayerFactory : IMapLayerFactory
    {
        public void CreateLayerOnMap(IsogeoData isogeoData)
        {
            var url = $"{isogeoData.Url}/{isogeoData.Name}";
            LayerFactory.Instance.CreateLayer(new Uri(url), MapView.Active.Map);
        }
    }
}
