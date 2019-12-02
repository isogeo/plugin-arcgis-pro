using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
//using System.Windows.Forms;
using System.Configuration;
using System.Xml;
using System.Xml.Linq;
using System.Security.Cryptography;
using System.Xml.Serialization;
using System.IO;
using System.Collections.Generic;

namespace ArcMapAddinIsogeo.Configuration
{

    public class ConfigurationManager
    {

        public configuration config;

        private XDocument doc;
        private string configPath;

        public ConfigurationManager()
        {
            try
            {
                string dllPAth = this.GetType().Assembly.Location;

                configPath = dllPAth.Substring(0, dllPAth.LastIndexOf("\\")) + "\\App.config";
                doc = XDocument.Load(configPath);

                config = SerializationUtil.Deserialize<Configuration.configuration>(doc);
                if (config.proxy == null) config.proxy = new Proxy();
                if (config.fileSDE == null) config.fileSDE = "";
                if (config.owner == null) config.owner = "";
                
                //config.languageIndex = -1;
                //config.userAuthentification = new UserAuthentification();
                //config.userAuthentification.id = "ddd";
                //config.userAuthentification.secret= "dsecretdd";
                //config.searchs = new Searchs();
                //Search search1=new Search();
                //search1.name = "test";
                //Search search2 = new Search();
                //search2.name = "test2";
                //config.searchs.searchs=new List<Search>();
                //config.searchs.searchs.Add(search1);
                //config.searchs.searchs.Add(search2  );

                //doc = SerializationUtil.Serialize(config);
                //doc.Save(configPath);


                //ExeConfigurationFileMap map = new ExeConfigurationFileMap();
                //map.ExeConfigFilename = configPath;//this.GetType().Assembly.Location + ".config";
                //config = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);



            }
            catch (Exception ex)
            {
                String erreur = ex.ToString();
            }
        }

        public void save()
        {
            doc = SerializationUtil.Serialize(config);
            doc.Save(configPath);
        }
    }
}
