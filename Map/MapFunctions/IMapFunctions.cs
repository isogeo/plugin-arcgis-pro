using System.Threading.Tasks;

namespace Isogeo.Map.MapFunctions
{
    public interface IMapFunctions
    {
        public Task SetMapExtent(string envelope);

        public string GetMapExtent();

        public Task AddLayer(DataType.ServiceType serviceType);
    }
}
