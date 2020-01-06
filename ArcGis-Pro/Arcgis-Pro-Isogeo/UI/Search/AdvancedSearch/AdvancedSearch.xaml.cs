using System;
using System.Drawing;
using System.Reflection;
using IsogeoLibrary;
using UserControl = System.Windows.Controls.UserControl;

namespace Arcgis_Pro_Isogeo.UI.Search.AdvancedSearch
{
    /// <summary>
    /// Logique d'interaction pour AdvancedSearch.xaml
    /// </summary>
    public partial class AdvancedSearch : UserControl
    {
        public AdvancedSearch()
        {
            InitializeComponent();
            InitAdvancedSearchItems();
            Variables.functionsTranslate.Add(translate);
        }

        public void InitAdvancedSearchItems()
        {
            this.ContactFilter.Init(
                IsogeoLibrary.Localization.LocalizationItem.Contact,
                "pack://application:,,,/" + Variables._assemblyName + ";component/Resources/phone_orange.png",
                "contact");

            this.InspireFilter.Init(
                IsogeoLibrary.Localization.LocalizationItem.INSPIRE_keywords,
                "pack://application:,,,/" + Variables._assemblyName + ";component/Resources/leaf.png", 
                "keyword:inspire-theme");

            this.FormatFilter.Init(
                IsogeoLibrary.Localization.LocalizationItem.Format_source,
                "pack://application:,,,/" + Variables._assemblyName + ";component/Resources/cube.png",
                "format");

            this.GeographyFilter.Init(
                IsogeoLibrary.Localization.LocalizationItem.Geographic_filter,
                "pack://application:,,,/" + Variables._assemblyName + ";component/Resources/map.png", 
                "geographicFilter");

            this.LicenseFilter.Init(
                IsogeoLibrary.Localization.LocalizationItem.Licence,
                "pack://application:,,,/" + Variables._assemblyName + ";component/Resources/gavel.png", 
                "license");

            this.CoordinateSystemFilter.Init(
                IsogeoLibrary.Localization.LocalizationItem.Coordinate_system_source,
                "pack://application:,,,/" + Variables._assemblyName + ";component/Resources/globe.png",
                "coordinate-system");

            this.OwnerMetadataFilter.Init(
                IsogeoLibrary.Localization.LocalizationItem.Metadata_owner,
                "pack://application:,,,/" + Variables._assemblyName + ";component/Resources/users.png",
                "owner");

            this.ResourceTypeFilter.Init(
                IsogeoLibrary.Localization.LocalizationItem.Ressource_type,
                "pack://application:,,,/" + Variables._assemblyName + ";component/Resources/asterisk.png", 
                "type");
        }

        private void translate()
        {
            this.GrpAdvancedSearch.Header = "     " + Variables.localisationManager.getValue(IsogeoLibrary.Localization.LocalizationItem.Advanced_search);
        }
    }
}
