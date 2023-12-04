using Isogeo.AddIn.Models;
using Isogeo.Map;
using Isogeo.Network;

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
        private readonly IMapManager _mapManager;
        private readonly FilterManager _filterManager;
        private readonly INetworkManager _networkManager;


        public AdvancedSearchViewModel(IMapManager mapManager, FilterManager filterManager, INetworkManager networkManager)
        {
            _mapManager = mapManager;
            _filterManager = filterManager;
            _networkManager = networkManager;
            InitAdvancedSearchItems();
        }

        public void InitAdvancedSearchItems()
        {
            ContactFilter = new AdvancedSearchItemViewModel(
                Language.Resources.Contact,
                "pack://application:,,,/Isogeo.Resources;component/Resources/phone_orange.png",
                "contact", _networkManager, _mapManager, _filterManager);

            InspireFilter = new AdvancedSearchItemViewModel(
                Language.Resources.INSPIRE_keywords,
                "pack://application:,,,/Isogeo.Resources;component/Resources/leaf.png",
                "keyword:inspire-theme", _networkManager, _mapManager, _filterManager);

            FormatFilter = new AdvancedSearchItemViewModel(
                Language.Resources.Format_source,
                "pack://application:,,,/Isogeo.Resources;component/Resources/cube.png",
                "format", _networkManager, _mapManager, _filterManager);

            GeographyFilter = new AdvancedSearchItemViewModel(
                Language.Resources.Geographic_filter,
                "pack://application:,,,/Isogeo.Resources;component/Resources/map.png",
                "keyword:isogeo", _networkManager, _mapManager, _filterManager);

            LicenseFilter = new AdvancedSearchItemViewModel(
                Language.Resources.Licence,
                "pack://application:,,,/Isogeo.Resources;component/Resources/gavel.png",
                "license", _networkManager, _mapManager, _filterManager);

            CoordinateSystemFilter = new AdvancedSearchItemViewModel(
                Language.Resources.Coordinate_system_source,
                "pack://application:,,,/Isogeo.Resources;component/Resources/globe.png",
                "coordinate-system", _networkManager, _mapManager, _filterManager);

            OwnerMetadataFilter = new AdvancedSearchItemViewModel(
                Language.Resources.Metadata_Advanced_owner,
                "pack://application:,,,/Isogeo.Resources;component/Resources/users.png",
                "owner", _networkManager, _mapManager, _filterManager);

            ResourceTypeFilter = new AdvancedSearchItemViewModel(
                Language.Resources.Resource_type,
                "pack://application:,,,/Isogeo.Resources;component/Resources/asterisk.png",
                "type", _networkManager, _mapManager, _filterManager);
        }
    }
}
