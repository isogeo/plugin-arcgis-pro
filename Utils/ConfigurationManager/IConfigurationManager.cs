using Isogeo.Utils.Configuration;

namespace Isogeo.Utils.ConfigurationManager
{
    public interface IConfigurationManager
    {
        public Configuration.Configuration Config { get; }

        public GlobalSoftwareSettings GlobalSoftwareSettings { get; }

        public void Save();
    }
}
