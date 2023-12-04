using ArcGIS.Desktop.Framework.Threading.Tasks;
using Isogeo.AddIn.Models;
using Isogeo.Models;
using Isogeo.Models.Network;
using MVVMPattern;
using MVVMPattern.MediatorPattern;

namespace Isogeo.AddIn.ViewsModels.Search.PrincipalSearch
{
    public class PrincipalSearchViewModel : ViewModelBase
    {
        private readonly RestFunctions _restFunctions;

        public string SearchText
        {
            get => Variables.searchText;
            set
            {
                Variables.searchText = value; 
                OnPropertyChanged("SearchText");
            }
        }

        public KeywordsViewModel KeywordsViewModel { get; set; }
        public QuickSearchViewModel QuickSearchViewModel { get; set; }

        private void ChangeSearchTextEvent(object obj)
        {
            SearchText = Variables.searchText;
        }

        public PrincipalSearchViewModel(FilterManager filterManager, RestFunctions restFunctions)
        {
            _restFunctions = restFunctions;
            KeywordsViewModel = new KeywordsViewModel(filterManager, _restFunctions);
            QuickSearchViewModel = new QuickSearchViewModel();
            Mediator.Register("ChangeQuery", ChangeSearchTextEvent);
        }

        public void Search()
        {
            QueuedTask.Run(() =>
            {
                _restFunctions.ReloadData(0);
            });
        }
    }
}
