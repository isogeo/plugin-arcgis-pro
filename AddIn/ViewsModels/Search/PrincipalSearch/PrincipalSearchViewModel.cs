using ArcGIS.Desktop.Framework.Threading.Tasks;
using Isogeo.AddIn.Models;
using Isogeo.Map.MapFunctions;
using Isogeo.Models;
using Isogeo.Models.Network;
using MVVMPattern;
using MVVMPattern.MediatorPattern;

namespace Isogeo.AddIn.ViewsModels.Search.PrincipalSearch
{
    public class PrincipalSearchViewModel : ViewModelBase
    {
        private readonly RestFunctions _restFunctions;
        private readonly FilterManager _filterManager;


        public KeywordsViewModel KeywordsViewModel { get; set; }
        public QuickSearchViewModel QuickSearchViewModel { get; set; }
        public SearchBarViewModel SearchBarViewModel { get; set; }


        public PrincipalSearchViewModel(FilterManager filterManager, RestFunctions restFunctions, IMapFunctions mapFunctions)
        {
            _restFunctions = restFunctions;
            _filterManager = filterManager;
            KeywordsViewModel = new KeywordsViewModel(filterManager, restFunctions, mapFunctions);
            QuickSearchViewModel = new QuickSearchViewModel(_restFunctions, filterManager, mapFunctions);
            SearchBarViewModel = new SearchBarViewModel(_restFunctions, filterManager);
        }
    }
}
