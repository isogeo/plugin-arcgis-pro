using Isogeo.AddIn.Models;
using Isogeo.Map.MapFunctions;
using Isogeo.Models.Network;
using Isogeo.Network;
using MVVMPattern;

namespace Isogeo.AddIn.ViewsModels.Search.PrincipalSearch
{
    public class PrincipalSearchViewModel : ViewModelBase
    {
        public KeywordsViewModel KeywordsViewModel { get; set; }
        public QuickSearchViewModel QuickSearchViewModel { get; set; }
        public SearchBarViewModel SearchBarViewModel { get; set; }

        public PrincipalSearchViewModel(FilterManager filterManager, IRestFunctions restFunctions, IMapFunctions mapFunctions)
        {
            KeywordsViewModel = new KeywordsViewModel(filterManager, restFunctions, mapFunctions);
            QuickSearchViewModel = new QuickSearchViewModel(restFunctions, filterManager, mapFunctions);
            SearchBarViewModel = new SearchBarViewModel(restFunctions, filterManager);
        }
    }
}
