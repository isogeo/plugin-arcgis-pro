using System;
using System.Windows;
using IsogeoLibrary;
using API = IsogeoLibrary.API;

namespace Arcgis_Pro_Isogeo.UI.Authentification
{
    /// <summary>
    /// Logique d'interaction pour Authentification.xaml
    /// </summary>
    public partial class Authentification : Window
    {
        public Authentification()
        {
            InitializeComponent();
            getAuthentification();
            translate();
        }

        private void getAuthentification()
        {
            try
            {
                TxtId.Text = Variables.configurationManager.config.userAuthentification.id;
                String secretValue = Variables.configurationManager.config.userAuthentification.secret;
                if (secretValue != "")
                {
                    TxtSecret.Text = RijndaelManagedEncryption.DecryptRijndael(secretValue, Variables.encryptCode);
                }

            }
            catch (Exception ex)
            {
                String erreur = ex.ToString();
                // TODO : log
            }
        }

        private void BtnCheck_Click(object sender, RoutedEventArgs e)
        {
            checkAuthentification(true);
        }

        private void translate()
        {

            this.Title = Variables.localisationManager.getValue(IsogeoLibrary.Localization.LocalizationItem.Authentification_Title);
            LblApplicationId.Content = Variables.localisationManager.getValue(IsogeoLibrary.Localization.LocalizationItem.Application_ID) + " :";
            LblApplicationSecret.Content = Variables.localisationManager.getValue(IsogeoLibrary.Localization.LocalizationItem.Application_SECRET) + " :";
            BtnCheck.Content = Variables.localisationManager.getValue(IsogeoLibrary.Localization.LocalizationItem.Check);
            BtnSave.Content = Variables.localisationManager.getValue(IsogeoLibrary.Localization.LocalizationItem.Save);
            BtnCancel.Content = Variables.localisationManager.getValue(IsogeoLibrary.Localization.LocalizationItem.Cancel);

        }

        private Boolean checkAuthentification(Boolean isCheck)
        {
            if (TxtId.Text == "")
            {
                MessageBox.Show(this, Variables.localisationManager.getValue(IsogeoLibrary.Localization.LocalizationItem.Message_Query_authentification_id_mandatory), "Isogeo");
                return false;
            }
            if (TxtSecret.Text.Length != 64)
            {
                MessageBox.Show(this, Variables.localisationManager.getValue(IsogeoLibrary.Localization.LocalizationItem.Message_Query_authentification_secret_mandatory), "Isogeo");
                return false;
            }

            API.Token token = Variables.restFunctions.askForToken(TxtId.Text, TxtSecret.Text);
            switch (token.StatusResult)
            {
                case "NotFound":
                    MessageBox.Show(this, Variables.localisationManager.getValue(IsogeoLibrary.Localization.LocalizationItem.Message_Query_authentification_ko_internet) + "\n" + Variables.localisationManager.getValue(IsogeoLibrary.Localization.LocalizationItem.Message_contact_support), "Isogeo");
                    break;
                case "OK":

                    break;
                case "BadRequest":
                    MessageBox.Show(this, Variables.localisationManager.getValue(IsogeoLibrary.Localization.LocalizationItem.Message_Query_authentification_ko_invalid) + "\n" + Variables.localisationManager.getValue(IsogeoLibrary.Localization.LocalizationItem.Message_contact_support), "Isogeo");
                    break;
                case "0":
                    MessageBox.Show(this, Variables.localisationManager.getValue(IsogeoLibrary.Localization.LocalizationItem.Message_Query_authentification_ko_proxy) + "\n" + Variables.localisationManager.getValue(IsogeoLibrary.Localization.LocalizationItem.Message_contact_support), "Isogeo");
                    break;

                default:
                    MessageBox.Show(this, Variables.localisationManager.getValue(IsogeoLibrary.Localization.LocalizationItem.Message_Query_authentification_ko_internet) + "\n" + Variables.localisationManager.getValue(IsogeoLibrary.Localization.LocalizationItem.Message_contact_support), "Isogeo");
                    break;

            }
            if (token.access_token != null)
            {
                if (isCheck)
                {
                    MessageBox.Show(this, Variables.localisationManager.getValue(IsogeoLibrary.Localization.LocalizationItem.Message_Query_authentification_ok), "Isogeo");
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            Boolean check = checkAuthentification(false);
            if (check == false) return;

            Variables.configurationManager.config.userAuthentification.id = TxtId.Text;
            try
            {
                string encryptedstring = RijndaelManagedEncryption.EncryptRijndael(TxtSecret.Text, Variables.encryptCode);
                Variables.configurationManager.config.userAuthentification.secret = encryptedstring;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            Variables.configurationManager.save();

            Variables.restFunctions.setConnexion();
            Variables.restFunctions.sendRequestIsogeo("", 0, false);
            this.Close();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            Variables.restFunctions.setConnexion();
            this.Close();
        }
    }
}
