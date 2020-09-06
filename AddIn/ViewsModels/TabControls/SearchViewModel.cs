using ArcGIS.Desktop.Framework.Contracts;
using Isogeo.AddIn.ViewsModels.Search.AdvancedSearch;
using Isogeo.AddIn.ViewsModels.Search.PrincipalSearch;
using Isogeo.AddIn.ViewsModels.Search.Results;

namespace Isogeo.AddIn.ViewsModels.TabControls
{
    public class SearchViewModel : ViewModelBase
    {
        //private Database database;

        public AdvancedSearchViewModel AdvancedSearchViewModel { get; set; }
        public ResultsViewModel ResultsViewModel { get; set; }
        public ResultsToolBarViewModel ResultsToolBarViewModel { get; set; }
        public PrincipalSearchViewModel PrincipalSearchViewModel { get; set; }

        private void InitViewModel()
        {
            AdvancedSearchViewModel = new AdvancedSearchViewModel();
            ResultsViewModel = new ResultsViewModel();
            ResultsToolBarViewModel = new ResultsToolBarViewModel();
            PrincipalSearchViewModel = new PrincipalSearchViewModel();
        }

        public SearchViewModel()
        {
            InitViewModel();
        }
    }
}
