using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;

using System.Xml;
using System.Xml.Linq;

namespace ArcMapAddinIsogeo.Localization
{
    public class LocalizationManager
    {
        
        private XDocument doc;

        public LocalizationManager()
        {
            openLocalizationFile();

        }
        
        public void openLocalizationFile()
        {
            String locale = Utils.Util.getLocale();
            string dllPAth = this.GetType().Assembly.Location;
            string configPath = dllPAth.Substring(0, dllPAth.LastIndexOf("\\")) + "\\Localization\\" + locale + ".xml";
            if (File.Exists(configPath) == true)
            {
                doc = XDocument.Load(configPath);
            }
            else
            {
                configPath = dllPAth.Substring(0, dllPAth.LastIndexOf("\\")) + "\\Localization\\" + "en" + ".xml"; // Path.Combine(Environment.CurrentDirectory, @"Localization/en.xml");
                doc = XDocument.Load(configPath);
            }
        }

        public void translatesAll()
        {

            Variables.TranslateInProgress = true;
            foreach (Action func in Variables.functionsTranslate)
            {
                func();
            }
            Variables.TranslateInProgress = false;
        }
        public String getValue(String item)
        {
            try
            {
                
                return doc.Element("locale").Element(item).Value;
            }
            catch { }
            return "";
        }


    }
}
