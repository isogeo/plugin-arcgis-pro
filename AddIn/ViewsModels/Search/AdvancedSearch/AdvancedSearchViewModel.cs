namespace Isogeo.AddIn.ViewsModels.Search.AdvancedSearch
{
    public class AdvancedSearchViewModel : MVVMPattern.ViewModelBase
    {
        public AdvancedSearchItemViewModel ContactFilter { get; set; }
        public AdvancedSearchItemViewModel InspireFilter { get; set; }
        public AdvancedSearchItemViewModel FormatFilter { get; set; }
        public AdvancedSearchItemViewModel GeographyFilter { get; set; }
        public AdvancedSearchItemViewModel LicenseFilter { get; set; }
        public AdvancedSearchItemViewModel CoordinateSystemFilter { get; set; }
        public AdvancedSearchItemViewModel OwnerMetadataFilter { get; set; }
        public AdvancedSearchItemViewModel ResourceTypeFilter { get; set; }


        public AdvancedSearchViewModel()
        {
            InitAdvancedSearchItems();
        }

        public void InitAdvancedSearchItems()
        {
            ContactFilter = new AdvancedSearchItemViewModel(
                Language.Resources.Contact,
                "pack://application:,,,/Isogeo.Resources;component/Resources/phone_orange.png",
                "contact");

            InspireFilter = new AdvancedSearchItemViewModel(
                Language.Resources.INSPIRE_keywords,
                "pack://application:,,,/Isogeo.Resources;component/Resources/leaf.png",
                "keyword:inspire-theme");

            FormatFilter = new AdvancedSearchItemViewModel(
                Language.Resources.Format_source,
                "pack://application:,,,/Isogeo.Resources;component/Resources/cube.png",
                "format");

            GeographyFilter = new AdvancedSearchItemViewModel(
                Language.Resources.Geographic_filter,
                "pack://application:,,,/Isogeo.Resources;component/Resources/map.png",
                "keyword:isogeo");

            LicenseFilter = new AdvancedSearchItemViewModel(
                Language.Resources.Licence,
                "pack://application:,,,/Isogeo.Resources;component/Resources/gavel.png",
                "license");

            CoordinateSystemFilter = new AdvancedSearchItemViewModel(
                Language.Resources.Coordinate_system_source,
                "pack://application:,,,/Isogeo.Resources;component/Resources/globe.png",
                "coordinate-system");

            OwnerMetadataFilter = new AdvancedSearchItemViewModel(
                Language.Resources.Metadata_Advanced_owner,
                "pack://application:,,,/Isogeo.Resources;component/Resources/users.png",
                "owner");

            ResourceTypeFilter = new AdvancedSearchItemViewModel(
                Language.Resources.Resource_type,
                "pack://application:,,,/Isogeo.Resources;component/Resources/asterisk.png",
                "type");
        }
    }
}
