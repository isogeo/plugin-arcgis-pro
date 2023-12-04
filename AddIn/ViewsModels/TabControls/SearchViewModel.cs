using ArcGIS.Desktop.Framework.Contracts;
using Isogeo.AddIn.Models.FilterManager;
using Isogeo.AddIn.ViewsModels.Search.AdvancedSearch;
using Isogeo.AddIn.ViewsModels.Search.PrincipalSearch;
using Isogeo.AddIn.ViewsModels.Search.Results;
using Isogeo.Map;
using Isogeo.Network;

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

        private void InitViewModel()
        {
            AdvancedSearchViewModel = new AdvancedSearchViewModel(_mapManager, _filterManager, _networkManager);
            ResultsViewModel = new ResultsViewModel(_mapManager, _networkManager, _filterManager);
            ResultsToolBarViewModel = new ResultsToolBarViewModel(_networkManager, _filterManager);
            PrincipalSearchViewModel = new PrincipalSearchViewModel(_filterManager, _networkManager, _mapManager);
        }

        public SearchViewModel(INetworkManager networkManager, IFilterManager filterManager, IMapManager mapManager)
        {
            _filterManager = filterManager;
            _mapManager = mapManager;
            _networkManager = networkManager;
            InitViewModel();
        }
    }
}
