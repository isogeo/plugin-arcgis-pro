using Isogeo.Map.Models;

namespace Isogeo.Map.MapLayerFactory.Contracts
{
    public interface IMapLayerFactory
    {
        public void CreateLayerOnMap(IsogeoData isogeoData);
    }
}
