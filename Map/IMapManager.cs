using System.Threading.Tasks;

namespace Isogeo.Map
{
    public interface IMapManager
    {
        public Task SetMapExtent(string envelope);

        public string GetMapExtent();

        public Task AddLayer(DataType.ServiceType serviceType);
    }
}
