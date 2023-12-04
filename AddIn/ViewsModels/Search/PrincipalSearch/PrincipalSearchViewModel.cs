using Isogeo.AddIn.Models;
using Isogeo.Map.MapFunctions;
using Isogeo.Network;
using MVVMPattern;

namespace Isogeo.AddIn.ViewsModels.Search.PrincipalSearch
{
    public class PrincipalSearchViewModel : ViewModelBase
    {
        public KeywordsViewModel KeywordsViewModel { get; set; }
        public QuickSearchViewModel QuickSearchViewModel { get; set; }
        public SearchBarViewModel SearchBarViewModel { get; set; }

        public PrincipalSearchViewModel(FilterManager filterManager, INetworkManager networkManager, IMapFunctions mapFunctions)
        {
            KeywordsViewModel = new KeywordsViewModel(filterManager, networkManager, mapFunctions);
            QuickSearchViewModel = new QuickSearchViewModel(networkManager, filterManager, mapFunctions);
            SearchBarViewModel = new SearchBarViewModel(networkManager, filterManager);
        }
    }
}
