using Isogeo.AddIn.Models;
using Isogeo.Map;
using Isogeo.Network;
using MVVMPattern;

namespace Isogeo.AddIn.ViewsModels.Search.PrincipalSearch
{
    public class PrincipalSearchViewModel : ViewModelBase
    {
        public KeywordsViewModel KeywordsViewModel { get; set; }
        public QuickSearchViewModel QuickSearchViewModel { get; set; }
        public SearchBarViewModel SearchBarViewModel { get; set; }

        public PrincipalSearchViewModel(FilterManager filterManager, INetworkManager networkManager, IMapManager mapManager)
        {
            KeywordsViewModel = new KeywordsViewModel(filterManager, networkManager, mapManager);
            QuickSearchViewModel = new QuickSearchViewModel(networkManager, filterManager, mapManager);
            SearchBarViewModel = new SearchBarViewModel(networkManager, filterManager);
        }
    }
}
