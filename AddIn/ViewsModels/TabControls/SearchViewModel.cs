using ArcGIS.Desktop.Framework.Contracts;
using Isogeo.AddIn.Models;
using Isogeo.AddIn.ViewsModels.Search.AdvancedSearch;
using Isogeo.AddIn.ViewsModels.Search.PrincipalSearch;
using Isogeo.AddIn.ViewsModels.Search.Results;
using Isogeo.Map.MapFunctions;
using Isogeo.Models.Network;
using Isogeo.Network;

namespace Isogeo.AddIn.ViewsModels.TabControls
{
    public class SearchViewModel : ViewModelBase
    {
        public AdvancedSearchViewModel AdvancedSearchViewModel { get; set; }
        public ResultsViewModel ResultsViewModel { get; set; }
        public ResultsToolBarViewModel ResultsToolBarViewModel { get; set; }
        public PrincipalSearchViewModel PrincipalSearchViewModel { get; set; }

        private readonly IMapFunctions _mapFunctions;
        private readonly INetworkManager _networkManager;
        private readonly FilterManager _filterManager;

        private void InitViewModel()
        {
            AdvancedSearchViewModel = new AdvancedSearchViewModel(_mapFunctions, _filterManager, _networkManager);
            ResultsViewModel = new ResultsViewModel(_mapFunctions, _networkManager, _filterManager);
            ResultsToolBarViewModel = new ResultsToolBarViewModel(_networkManager, _filterManager);
            PrincipalSearchViewModel = new PrincipalSearchViewModel(_filterManager, _networkManager, _mapFunctions);
        }

        public SearchViewModel(INetworkManager networkManager, FilterManager filterManager, IMapFunctions mapFunctions)
        {
            _filterManager = filterManager;
            _mapFunctions = mapFunctions;
            _networkManager = networkManager;
            InitViewModel();
        }
    }
}
