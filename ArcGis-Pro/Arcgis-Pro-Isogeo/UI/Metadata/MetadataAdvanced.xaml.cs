using System;
using System.Windows;
using System.Windows.Forms;
using ArcMapAddinIsogeo;
using UserControl = System.Windows.Controls.UserControl;

namespace Arcgis_Pro_Isogeo.UI.Metadata
{
    /// <summary>
    /// Logique d'interaction pour MetadataAdvanced.xaml
    /// </summary>
    public partial class MetadataAdvanced : UserControl
    {
        public MetadataAdvanced()
        {
            InitializeComponent();
        }

        public void setValues()
        {
            GrpMetadataProperty.Header = Variables.localisationManager.getValue(ArcMapAddinIsogeo.Localization.LocalizationItem.Metadata_Advanced_owner);
            GrpMetadataDetails.Header = Variables.localisationManager.getValue(ArcMapAddinIsogeo.Localization.LocalizationItem.Metadata_Advanced_details);
            MetadataDetails.LblLanguage.Content = Variables.localisationManager.getValue(ArcMapAddinIsogeo.Localization.LocalizationItem.Metadata_Advanced_lang);
            MetadataDetails.LblCreatedAt.Content = Variables.localisationManager.getValue(ArcMapAddinIsogeo.Localization.LocalizationItem.Metadata_Advanced_create);
            MetadataDetails.LblLastModification.Content = Variables.localisationManager.getValue(ArcMapAddinIsogeo.Localization.LocalizationItem.Metadata_Advanced_last_update);

            // -- ADVANCED  TAB ---------------------------------------------------
            //Workgroup owner
            MetadataDetails.LblCreatedAtValue.Content = "";
            MetadataDetails.LblLanguageValue.Content = "";
            MetadataDetails.LblLastModificationValue.Content = "";

            ContactItem.LblAddress.Content = "";
            ContactItem.LblCity.Content = "";
            ContactItem.LblName.Content = "";
            ContactItem.LblEmail.Content = "";
            ContactItem.LblPhone.Content = "";

            if (Variables.currentResult._creator != null)
            {
                if (Variables.currentResult._creator.contact != null)
                {
                    ContactItem.LblName.Content = Variables.currentResult._creator.contact.name;
                    ContactItem.LblEmail.Content = Variables.currentResult._creator.contact.email;


                    ContactItem.LblPhone.Content = Variables.currentResult._creator.contact.phone;
                    if (Variables.currentResult._creator.contact.addressLine1 != null)
                    {
                        ContactItem.LblAddress.Content = Variables.currentResult._creator.contact.addressLine1;
                    }
                    if (Variables.currentResult._creator.contact.addressLine2 != null)
                    {
                        ContactItem.LblAddress.Content += " " + Variables.currentResult._creator.contact.addressLine2;
                    }
                    ContactItem.LblCity.Content = Variables.currentResult._creator.contact.zipCode + " " + Variables.currentResult._creator.contact.city + " " + Variables.currentResult._creator.contact.country;
                }
            }
            MetadataDetails.LblLanguageValue.Content = Variables.currentResult.language;
            MetadataDetails.LblCreatedAtValue.Content = formatDate(Variables.currentResult._modified);
            MetadataDetails.LblLastModificationValue.Content = formatDate(Variables.currentResult._created);
        }

        private String formatDate(String dateStr)
        {
            if (dateStr == null) return "";
            return dateStr.Split('T')[0];

        }

        private void val_owner_email_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (ContactItem.LblEmail.Content.ToString() != "")
            {
                string mailto =
                    string.Format(
                        "mailto:{0}?Subject={1}&Body={2}",
                        ContactItem.LblEmail.Content, "Isogeo", "");
                System.Diagnostics.Process.Start(mailto);
            }

        }
    }
}
