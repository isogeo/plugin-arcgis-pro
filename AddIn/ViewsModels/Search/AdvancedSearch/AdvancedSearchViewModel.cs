using Isogeo.Map.MapFunctions;

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
        private readonly IMapFunctions _mapFunctions;


        public AdvancedSearchViewModel(IMapFunctions mapFunctions)
        {
            _mapFunctions = mapFunctions;
            InitAdvancedSearchItems();
        }

        public void InitAdvancedSearchItems()
        {
            ContactFilter = new AdvancedSearchItemViewModel(
                Language.Resources.Contact,
                "pack://application:,,,/Isogeo.Resources;component/Resources/phone_orange.png",
                "contact", _mapFunctions);

            InspireFilter = new AdvancedSearchItemViewModel(
                Language.Resources.INSPIRE_keywords,
                "pack://application:,,,/Isogeo.Resources;component/Resources/leaf.png",
                "keyword:inspire-theme", _mapFunctions);

            FormatFilter = new AdvancedSearchItemViewModel(
                Language.Resources.Format_source,
                "pack://application:,,,/Isogeo.Resources;component/Resources/cube.png",
                "format", _mapFunctions);

            GeographyFilter = new AdvancedSearchItemViewModel(
                Language.Resources.Geographic_filter,
                "pack://application:,,,/Isogeo.Resources;component/Resources/map.png",
                "keyword:isogeo", _mapFunctions);

            LicenseFilter = new AdvancedSearchItemViewModel(
                Language.Resources.Licence,
                "pack://application:,,,/Isogeo.Resources;component/Resources/gavel.png",
                "license", _mapFunctions);

            CoordinateSystemFilter = new AdvancedSearchItemViewModel(
                Language.Resources.Coordinate_system_source,
                "pack://application:,,,/Isogeo.Resources;component/Resources/globe.png",
                "coordinate-system", _mapFunctions);

            OwnerMetadataFilter = new AdvancedSearchItemViewModel(
                Language.Resources.Metadata_Advanced_owner,
                "pack://application:,,,/Isogeo.Resources;component/Resources/users.png",
                "owner", _mapFunctions);

            ResourceTypeFilter = new AdvancedSearchItemViewModel(
                Language.Resources.Resource_type,
                "pack://application:,,,/Isogeo.Resources;component/Resources/asterisk.png",
                "type", _mapFunctions);
        }
    }
}
