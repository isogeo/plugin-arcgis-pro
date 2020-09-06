using System;
using System.IO;
using System.Windows;
using System.Xml.Linq;
using Isogeo.Utils.LogManager;
using Newtonsoft.Json;

namespace Isogeo.Models.Configuration
{

    public class ConfigurationManager
    {

        public configuration config;

        private XDocument _doc;
        private string _configPath;
        private string _filePath;

        public void InitConfigurationOnDesktop()
        {
            try
            {
                var localPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                _filePath = localPath + @"/Isogeo/ArcGisPro/AddInConfiguration.json";

                Directory.CreateDirectory(localPath + @"/Isogeo/ArcGisPro/");

                if (!File.Exists(_filePath))
                {
                    ReadAppConfig();
                    File.WriteAllText(_filePath, JsonConvert.SerializeObject(config, Formatting.Indented));
                }
                else
                {
                    var json = File.ReadAllText(_filePath);
                    config = JsonConvert.DeserializeObject<configuration>(json);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error configuration : missing or wrong App.config file", "Isogeo");
                Log.Logger.Error(ex.Message);
                Log.Logger.Error(ex.StackTrace);
            }
        }

        private void ReadAppConfig()
        {
            var dllPAth = GetType().Assembly.Location;

            _configPath = dllPAth.Substring(0, dllPAth.LastIndexOf("\\", StringComparison.Ordinal)) + "\\" +
                          "App.config";
            _doc = XDocument.Load(_configPath);

            config = SerializationUtil.Deserialize<configuration>(_doc);
            if (config.proxy == null) config.proxy = new Proxy();
            if (config.fileSde == null) config.fileSde = "";
            if (config.owner == null) config.owner = "";
        }

        public ConfigurationManager()
        {
            InitConfigurationOnDesktop();
        }

        public void Save()
        {
            if (!File.Exists(_filePath))
            {
                _doc = SerializationUtil.Serialize(config);
                _doc.Save(_configPath);
            }
            else
            {
                File.WriteAllText(_filePath, JsonConvert.SerializeObject(config, Formatting.Indented));
            }
        }
    }
}
