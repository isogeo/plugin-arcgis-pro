using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace ArcMapAddinIsogeo.Utils
{
    public static class Util
    {
        public static String getLocale()
        {
            String locale = "en";
            CultureInfo culture = CultureInfo.CurrentCulture;
            if (Variables.CmbLang.SelectedValue != null)
            {
                if ((String)Variables.CmbLang.SelectedValue == "default")
                {
                    return culture.Name.Substring(0, 2);
                }
                else
                {
                    return (String)Variables.CmbLang.SelectedValue;
                }
            }
            
            //CultureInfo cultureAPP = System.Threading.Thread.CurrentThread.CurrentUICulture;

            return locale;
        }
    }
}
