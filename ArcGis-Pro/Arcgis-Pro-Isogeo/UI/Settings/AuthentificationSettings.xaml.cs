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
    /// Logique d'interaction pour AuthentificationSettings.xaml
    /// </summary>
    public partial class AuthentificationSettings : UserControl
    {
        public AuthentificationSettings()
        {
            InitializeComponent();
            Variables.functionsTranslate.Add(translate);
        }

        private void translate()
        {
            GrpAuthenticationSettings.Content = Variables.localisationManager.getValue(IsogeoLibrary.Localization.LocalizationItem.Authentification_settings); 
            LblAuthenticationParameters.Content = Variables.localisationManager.getValue(IsogeoLibrary.Localization.LocalizationItem.Set_pluggin_authentification) + " :";
        }

        private void BtnAuthentication_Click(object sender, RoutedEventArgs e)
        {
            UI.Authentification.Authentification frmAuthentification = new UI.Authentification.Authentification();
            frmAuthentification.ShowDialog();
        }
    }
}
