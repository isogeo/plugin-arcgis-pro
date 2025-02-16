using System.Threading.Tasks;
using Isogeo.Map.Models;

namespace Isogeo.Map
{
    public interface IMapManager
    {
        public Task SetMapExtent(string envelope);

        public string GetMapExtent();

        public Task AddLayer(IsogeoData isogeoData);
    }
}
