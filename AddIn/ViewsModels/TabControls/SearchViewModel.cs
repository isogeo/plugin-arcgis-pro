using ArcGIS.Desktop.Framework.Contracts;
using Isogeo.AddIn.Models.FilterManager;
using Isogeo.AddIn.ViewsModels.Search.AdvancedSearch;
using Isogeo.AddIn.ViewsModels.Search.PrincipalSearch;
using Isogeo.AddIn.ViewsModels.Search.Results;
using Isogeo.Map;
using Isogeo.Network;
using Isogeo.Utils.ConfigurationManager;

namespace Isogeo.AddIn.ViewsModels.TabControls
{
    public class SearchViewModel : ViewModelBase
    {
        public AdvancedSearchViewModel AdvancedSearchViewModel { get; set; }
        public ResultsViewModel ResultsViewModel { get; set; }
        public ResultsToolBarViewModel ResultsToolBarViewModel { get; set; }
        public PrincipalSearchViewModel PrincipalSearchViewModel { get; set; }

        private readonly IMapManager _mapManager;
        private readonly INetworkManager _networkManager;
        private readonly IFilterManager _filterManager;
        private readonly IConfigurationManager _configurationManager;

        private void InitViewModel()
        {
            AdvancedSearchViewModel = new AdvancedSearchViewModel(_mapManager, _filterManager, _networkManager);
            ResultsViewModel = new ResultsViewModel(_mapManager, _networkManager, _filterManager, _configurationManager);
            ResultsToolBarViewModel = new ResultsToolBarViewModel(_networkManager, _filterManager, _configurationManager);
            PrincipalSearchViewModel = new PrincipalSearchViewModel(_filterManager, _networkManager, _mapManager, _configurationManager);
        }

        public SearchViewModel(INetworkManager networkManager, IFilterManager filterManager, IMapManager mapManager,
            IConfigurationManager configurationManager)
        {
            _filterManager = filterManager;
            _mapManager = mapManager;
            _networkManager = networkManager;
            _configurationManager = configurationManager;
            InitViewModel();
        }
    }
}
