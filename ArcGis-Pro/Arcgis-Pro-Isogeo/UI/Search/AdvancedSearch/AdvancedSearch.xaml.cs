using System;
using System.Drawing;
using System.Reflection;
using ArcMapAddinIsogeo;
using UserControl = System.Windows.Controls.UserControl;

namespace Arcgis_Pro_Isogeo.UI.Search.AdvancedSearch
{
    /// <summary>
    /// Logique d'interaction pour AdvancedSearch.xaml
    /// </summary>
    public partial class AdvancedSearch : UserControl
    {
        private readonly string _assemblyName;

        public AdvancedSearch()
        {
            InitializeComponent();
            _assemblyName = "Arcgis-Pro-Isogeo";
            Variables.functionsTranslate.Add(translate);
            InitAdvancedSearchItems();
        }

        public void InitAdvancedSearchItems()
        {
            this.ContactFilter.Init(
                ArcMapAddinIsogeo.Localization.LocalizationItem.Contact,
                "pack://application:,,,/" + _assemblyName + ";component/Resources/phone_orange.png",
                "contact");

            this.InspireFilter.Init(
                ArcMapAddinIsogeo.Localization.LocalizationItem.INSPIRE_keywords,
                "pack://application:,,,/" + _assemblyName + ";component/Resources/leaf.png", 
                "keyword:inspire-theme");

            this.FormatFilter.Init(
                ArcMapAddinIsogeo.Localization.LocalizationItem.Format_source,
                "pack://application:,,,/" + _assemblyName + ";component/Resources/cube.png",
                "format");

            this.GeographyFilter.Init(
                ArcMapAddinIsogeo.Localization.LocalizationItem.Geographic_filter,
                "pack://application:,,,/" + _assemblyName + ";component/Resources/map.png", 
                "geographicFilter");

            this.LicenseFilter.Init(
                ArcMapAddinIsogeo.Localization.LocalizationItem.Licence,
                "pack://application:,,,/" + _assemblyName + ";component/Resources/gavel.png", 
                "license");

            this.CoordinateSystemFilter.Init(
                ArcMapAddinIsogeo.Localization.LocalizationItem.Coordinate_system_source,
                "pack://application:,,,/" + _assemblyName + ";component/Resources/globe.png",
                "coordinate-system");

            this.OwnerMetadataFilter.Init(
                ArcMapAddinIsogeo.Localization.LocalizationItem.Metadata_owner,
                "pack://application:,,,/" + _assemblyName + ";component/Resources/users.png",
                "owner");

            this.ResourceTypeFilter.Init(
                ArcMapAddinIsogeo.Localization.LocalizationItem.Ressource_type,
                "pack://application:,,,/" + _assemblyName + ";component/Resources/asterisk.png", 
                "type");
        }

        private void translate()
        {
            this.GrpAdvancedSearch.Header = "     " + Variables.localisationManager.getValue(ArcMapAddinIsogeo.Localization.LocalizationItem.Advanced_search);
        }
    }
}
