using System;
using System.IO;
using System.Text.Json;
using System.Xml.Linq;
using Isogeo.Utils.Configuration;
using Isogeo.Utils.LogManager;

namespace Isogeo.Utils.ConfigurationManager
{
    public class ConfigurationManager : IConfigurationManager
    {
        public Configuration.Configuration Config { get; private set; }
        public GlobalSoftwareSettings GlobalSoftwareSettings { get; init; }

        private const int NbResults = 10;
        private const string EncyptCode = "alo(-'oàkd:jdthe";

        private string _configPath;
        private string _filePath;

        public ConfigurationManager()
        {
            InitConfigurationOnDesktop();
            GlobalSoftwareSettings = new GlobalSoftwareSettings(NbResults, EncyptCode);
        }

        private void UpdateExistingOldConfigsOnProduction()
        {
            if (Config.UrlHelp == "http://help.isogeo.com/arcgispro/fr/") // Suppress old help url on existing config for existing users
                Config.UrlHelp = (GetAppConfig(GetType().Assembly.Location)).UrlHelp;
        }

        private void InitConfigurationOnDesktop()
        {
            try
            {
                var localPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                _filePath = localPath + @"/Isogeo/ArcGisPro/AddInConfiguration.json";

                Directory.CreateDirectory(localPath + @"/Isogeo/ArcGisPro/");

                if (!File.Exists(_filePath))
                {
                    SetConfigWithDefaultAppConfig();
                    File.WriteAllText(_filePath, JsonSerializer.Serialize(Config));
                }
                else
                {
                    var json = File.ReadAllText(_filePath);
                    Config = JsonSerializer.Deserialize<Configuration.Configuration>(json);
                    UpdateExistingOldConfigsOnProduction();
                }
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex.Message);
                Log.Logger.Error(ex.StackTrace);
                throw;
            }
        }

        private static Configuration.Configuration GetAppConfig(string assemblyLocation)
        {
            var configPath = assemblyLocation[..assemblyLocation.LastIndexOf("\\", StringComparison.Ordinal)] + "\\" +
                          "App.config";
            var doc = XDocument.Load(configPath);

            var config = SerializationUtil.Deserialize<Configuration.Configuration>(doc);
            config.Proxy ??= new Proxy();
            config.FileSde ??= "";
            return config;
        }

        private void SetConfigWithDefaultAppConfig()
        {
            var dllPAth = GetType().Assembly.Location;
            Config = GetAppConfig(dllPAth);
        }

        public void Save()
        {
            if (!File.Exists(_filePath))
            {
                var doc = SerializationUtil.Serialize(Config);
                doc.Save(_configPath);
            }
            else
            {
                File.WriteAllText(_filePath, JsonSerializer.Serialize(Config));
            }
        }
    }
}
