using Isogeo.Models;
using MVVMPattern;
using MVVMPattern.MediatorPattern;

namespace Isogeo.AddIn.ViewsModels.Search.PrincipalSearch
{
    public class PrincipalSearchViewModel : ViewModelBase
    {

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

        public PrincipalSearchViewModel()
        {
            KeywordsViewModel = new KeywordsViewModel();
            QuickSearchViewModel = new QuickSearchViewModel();
            Mediator.Register("ChangeQuery", ChangeSearchTextEvent);
        }

        public void Search()
        {
            Variables.restFunctions.ReloadData(0);
        }
    }
}
