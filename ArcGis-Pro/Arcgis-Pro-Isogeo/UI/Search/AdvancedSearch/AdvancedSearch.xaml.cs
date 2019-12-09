using System;
using System.Drawing;
using ArcMapAddinIsogeo;
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
            Variables.functionsTranslate.Add(translate);
            InitAdvancedSearchItems();
        }

        public void InitAdvancedSearchItems()
        {
            this.ContactFilter.Init(
                ArcMapAddinIsogeo.Localization.LocalizationItem.Contact,
                Properties.Resources.phone_orange,
                "contact");

            this.InspireFilter.Init(
                ArcMapAddinIsogeo.Localization.LocalizationItem.INSPIRE_keywords, 
                Properties.Resources.leaf, 
                "keyword:inspire-theme");

            this.FormatFilter.Init(
                ArcMapAddinIsogeo.Localization.LocalizationItem.Format_source,
                Properties.Resources.cube, 
                "format");

            this.GeographyFilter.Init(
                ArcMapAddinIsogeo.Localization.LocalizationItem.Geographic_filter,
                Properties.Resources.map, 
                "geographicFilter");

            this.LicenseFilter.Init(
                ArcMapAddinIsogeo.Localization.LocalizationItem.Licence,
                Properties.Resources.gavel, 
                "license");

            this.CoordinateSystemFilter.Init(
                ArcMapAddinIsogeo.Localization.LocalizationItem.Coordinate_system_source, 
                Properties.Resources.globe,
                "coordinate-system");

            this.OwnerMetadataFilter.Init(
                ArcMapAddinIsogeo.Localization.LocalizationItem.Metadata_owner, 
                Properties.Resources.users,
                "owner");

            this.ResourceTypeFilter.Init(
                ArcMapAddinIsogeo.Localization.LocalizationItem.Ressource_type, 
                Properties.Resources.asterisk, 
                "type");
        }

        private void translate()
        {
            this.GrpAdvancedSearch.Content = "     " + Variables.localisationManager.getValue(ArcMapAddinIsogeo.Localization.LocalizationItem.Advanced_search);
        }
    }
}
