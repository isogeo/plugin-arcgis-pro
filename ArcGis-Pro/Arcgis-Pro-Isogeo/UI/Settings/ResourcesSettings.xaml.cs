using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using IsogeoLibrary;

namespace Arcgis_Pro_Isogeo.UI.Settings
{
    /// <summary>
    /// Logique d'interaction pour ResourcesSettings.xaml
    /// </summary>
    public partial class ResourcesSettings : UserControl
    {
        public ResourcesSettings()
        {
            InitializeComponent();
            Variables.functionsTranslate.Add(translate);
        }

        private void translate()
        {
            GrpResource.Header = Variables.localisationManager.getValue(IsogeoLibrary.Localization.LocalizationItem.Ressources);
            LblHelp.Content = Variables.localisationManager.getValue(IsogeoLibrary.Localization.LocalizationItem.Open_help) + " :";
            LblCredits.Content = Variables.localisationManager.getValue(IsogeoLibrary.Localization.LocalizationItem.Open_plugin_credits) + " :";
            LblContactSupport.Content = Variables.localisationManager.getValue(IsogeoLibrary.Localization.LocalizationItem.Contact_support) + " :";
        }

        private void BtnHelp_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(Variables.configurationManager.config.urlHelp);
        }

        private void BtnCredits_Click(object sender, RoutedEventArgs e)
        {
            Credits frm_credits = new Credits();
            frm_credits.ShowDialog();
        }

        private void BtnContactSupport_Click(object sender, RoutedEventArgs e)
        {
            //Properties.Settings.Default
            String versionArcgis = "";

            string mailto =
                string.Format(
                    "mailto:{0}?Subject={1}&Body={2}",
                    Variables.configurationManager.config.emailSupport, Variables.configurationManager.config.emailSubject, Variables.configurationManager.config.emailBody.Replace("/n", "%0D%0A"));
            System.Diagnostics.Process.Start(mailto);
        }
    }
}
