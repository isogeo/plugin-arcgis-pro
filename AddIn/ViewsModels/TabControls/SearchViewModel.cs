using ArcGIS.Desktop.Framework.Contracts;
using Isogeo.AddIn.Models;
using Isogeo.AddIn.ViewsModels.Search.AdvancedSearch;
using Isogeo.AddIn.ViewsModels.Search.PrincipalSearch;
using Isogeo.AddIn.ViewsModels.Search.Results;
using Isogeo.Map.MapFunctions;
using Isogeo.Models.Network;

namespace Isogeo.AddIn.ViewsModels.TabControls
{
    public class SearchViewModel : ViewModelBase
    {
        //private Database database;

        public AdvancedSearchViewModel AdvancedSearchViewModel { get; set; }
        public ResultsViewModel ResultsViewModel { get; set; }
        public ResultsToolBarViewModel ResultsToolBarViewModel { get; set; }
        public PrincipalSearchViewModel PrincipalSearchViewModel { get; set; }

        private readonly IMapFunctions _mapFunctions;
        private readonly RestFunctions _restFunctions;
        private readonly FilterManager _filterManager;

        private void InitViewModel()
        {
            AdvancedSearchViewModel = new AdvancedSearchViewModel(_mapFunctions);
            ResultsViewModel = new ResultsViewModel(_mapFunctions, _restFunctions);
            ResultsToolBarViewModel = new ResultsToolBarViewModel();
            PrincipalSearchViewModel = new PrincipalSearchViewModel(_filterManager, _restFunctions);
        }

        public SearchViewModel()
        {
            _mapFunctions = new MapFunctions();
            _restFunctions = new RestFunctions(_mapFunctions);
            _filterManager = new FilterManager();
            InitViewModel();
        }
    }
}
