using Isogeo.Models.Configuration;

namespace Isogeo.Utils.ConfigurationManager
{
    public interface IConfigurationManager
    {
        public Configuration Config { get; }

        public void Save();
    }
}
