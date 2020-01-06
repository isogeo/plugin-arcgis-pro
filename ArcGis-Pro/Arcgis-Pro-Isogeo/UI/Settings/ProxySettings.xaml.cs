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
    /// Logique d'interaction pour ProxySettings.xaml
    /// </summary>
    public partial class ProxySettings : UserControl
    {
        public ProxySettings()
        {
            InitializeComponent();
            Variables.functionsTranslate.Add(translate);
        }

        private void translate()
        {
            BtnSave.Content = Variables.localisationManager.getValue(IsogeoLibrary.Localization.LocalizationItem.Save);
            BtnCancel.Content = Variables.localisationManager.getValue(IsogeoLibrary.Localization.LocalizationItem.Cancel);
            LblProxyUrl.Content = Variables.localisationManager.getValue(IsogeoLibrary.Localization.LocalizationItem.Proxy_url) + " :";
            LblUser.Content = Variables.localisationManager.getValue(IsogeoLibrary.Localization.LocalizationItem.Proxy_user) + " :";
            LblPassword.Content = Variables.localisationManager.getValue(IsogeoLibrary.Localization.LocalizationItem.Proxy_password) + " :";
            GrpProxyParameter.Header = "     " + Variables.localisationManager.getValue(IsogeoLibrary.Localization.LocalizationItem.Proxy_settings);
        }

        public void setValues()
        {
            TxtProxyUrl.Text = Variables.configurationManager.config.proxy.proxyUrl;
            TxtUser.Text = Variables.configurationManager.config.proxy.proxyUser;
            TxtPassword.Password = "";
            try
            {
                if (Variables.configurationManager.config.proxy.proxyPassword != "")
                {
                    string encryptedstring = RijndaelManagedEncryption.DecryptRijndael(Variables.configurationManager.config.proxy.proxyPassword, Variables.encryptCode);
                    TxtPassword.Password = encryptedstring;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            Variables.configurationManager.config.proxy.proxyUrl = TxtProxyUrl.Text;
            Variables.configurationManager.config.proxy.proxyUser = TxtUser.Text;
            Variables.configurationManager.config.proxy.proxyPassword = "";
            try
            {
                if (TxtPassword.Password != "")
                {
                    string encryptedstring = RijndaelManagedEncryption.EncryptRijndael(TxtPassword.Password, Variables.encryptCode);
                    Variables.configurationManager.config.proxy.proxyPassword = encryptedstring;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            Variables.configurationManager.save();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            setValues();
        }
    }
}
