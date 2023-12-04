using System.ComponentModel;
using Isogeo.AddIn.Models.FilterManager;
using Isogeo.AddIn.Models.Filters.Components;
using Isogeo.Map;
using Isogeo.Network;
using MVVMPattern;

namespace Isogeo.AddIn.ViewsModels.Search.PrincipalSearch
{
    public class KeywordsViewModel : ViewModelBase
    {
        public string ComponentName => Language.Resources.Keywords;

        private readonly IFilterManager _filterManager;

        private Filters _filters;
        public Filters Filters
        {
            get => _filters;
            set
            {
                _filters = value;
                OnPropertyChanged(nameof(Filters));
            }
        }

        private void Filter_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(nameof(Filters));
        }

        public KeywordsViewModel(IFilterManager filterManager, INetworkManager networkManager, IMapManager mapManager)
        {
            _filterManager = filterManager;
            Filters = new Filters("keyword:isogeo", networkManager, filterManager, mapManager);
            Filters.PropertyChanged += Filter_PropertyChanged;
            _filterManager.AddFunctionToSetFilterList(SetList);
            _filterManager.AddFilters(Filters);
        }

        private void SetList()
        {
            _filterManager.SetListCombo(Filters, "keyword:isogeo");
        }
    }
}
