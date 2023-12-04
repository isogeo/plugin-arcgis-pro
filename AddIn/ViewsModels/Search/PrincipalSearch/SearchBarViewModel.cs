using System.Threading.Tasks;
using Isogeo.AddIn.Models;
using Isogeo.Models.Network;
using Isogeo.Models;
using MVVMPattern;
using MVVMPattern.MediatorPattern;

namespace Isogeo.AddIn.ViewsModels.Search.PrincipalSearch
{
    public class SearchBarViewModel : ViewModelBase
    {
        private readonly RestFunctions _restFunctions;
        private readonly FilterManager _filterManager;

        public string SearchText
        {
            get => Variables.searchText;
            set
            {
                Variables.searchText = value;
                OnPropertyChanged(nameof(SearchText));
            }
        }

        private void ChangeSearchTextEvent(object obj)
        {
            SearchText = Variables.searchText;
        }

        public SearchBarViewModel(RestFunctions restFunctions, FilterManager filterManager)
        {
            _restFunctions = restFunctions;
            _filterManager = filterManager;
            Mediator.Register("ChangeQuery", ChangeSearchTextEvent);
        }

        public async Task Search()
        {
            var ob = _filterManager.GetOb();
            var od = _filterManager.GetOd();
            var query = _filterManager.GetQueryCombos();
            var box = _filterManager.GetBoxRequest();
            await _restFunctions.ReloadData(0, query, box, od, ob);
            _filterManager.SetSearchList(query);
        }
    }
}
