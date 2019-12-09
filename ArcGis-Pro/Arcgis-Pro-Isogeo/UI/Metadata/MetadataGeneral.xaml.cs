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
using ArcMapAddinIsogeo;

namespace Arcgis_Pro_Isogeo.UI.Metadata
{
    /// <summary>
    /// Logique d'interaction pour MetadataGeneral.xaml
    /// </summary>
    public partial class MetadataGeneral : UserControl
    {
        public MetadataGeneral()
        {
            InitializeComponent();
        }

        public void setValues()
        { 
            LblTitle.Content = "";
            LblOwner.Content = "";
            LblKeywordsValue.Content = "";
            LblThemesValue.Content = "";
            LblConformityValue.Content = "";
            LblDescriptionValue.Content = "";

            LblOwner.Content = Variables.localisationManager.getValue(ArcMapAddinIsogeo.Localization.LocalizationItem.Metadata_General_owner);
            LblKeywords.Content = Variables.localisationManager.getValue(ArcMapAddinIsogeo.Localization.LocalizationItem.Metadata_General_keywords);
            LblThemes.Content = Variables.localisationManager.getValue(ArcMapAddinIsogeo.Localization.LocalizationItem.Metadata_General_themes);
            LblConformity.Content = Variables.localisationManager.getValue(ArcMapAddinIsogeo.Localization.LocalizationItem.Metadata_General_conformity);
            LblDescription.Content = Variables.localisationManager.getValue(ArcMapAddinIsogeo.Localization.LocalizationItem.Metadata_General_description);



            if (Variables.currentResult.title != null) LblTitle.Content = Variables.currentResult.title;
            if (Variables.currentResult._creator != null) LblOwnerValue.Content = Variables.currentResult._creator.contact.name;
            if (Variables.currentResult.tagsLists != null) LblKeywordsValue.Content = string.Join(" ; ", Variables.currentResult.tagsLists.keywords.ToArray());
            if (Variables.currentResult.tagsLists != null) LblThemesValue.Content = string.Join(" ; ", Variables.currentResult.tagsLists.themeinspire.ToArray());

            // TODO : why "Non" is not a translate method like others part ??? Need to change it !
            LblConformity.Content = "Non";
            if (Variables.currentResult.tagsLists != null)
            {
                // TODO : Same as before...
                if (Variables.currentResult.tagsLists.conformity == 1) LblConformityValue.Content = "Oui";
            }

            if (Variables.currentResult.@abstract != null) LblDescriptionValue.Content = Variables.currentResult.@abstract;

        }
    }
}
