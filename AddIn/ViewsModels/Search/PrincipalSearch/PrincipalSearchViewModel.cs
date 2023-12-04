using Isogeo.AddIn.Models.FilterManager;
using Isogeo.Map;
using Isogeo.Models.Configuration;
using Isogeo.Network;
using MVVMPattern;

namespace Isogeo.AddIn.ViewsModels.Search.PrincipalSearch
{
    public class PrincipalSearchViewModel : ViewModelBase
    {
        public KeywordsViewModel KeywordsViewModel { get; set; }
        public QuickSearchViewModel QuickSearchViewModel { get; set; }
        public SearchBarViewModel SearchBarViewModel { get; set; }

        public PrincipalSearchViewModel(IFilterManager filterManager, INetworkManager networkManager, IMapManager mapManager,
            ConfigurationManager configurationManager)
        {
            KeywordsViewModel = new KeywordsViewModel(filterManager, networkManager, mapManager);
            QuickSearchViewModel = new QuickSearchViewModel(networkManager, filterManager, mapManager, configurationManager);
            SearchBarViewModel = new SearchBarViewModel(networkManager, filterManager);
        }
    }
}
