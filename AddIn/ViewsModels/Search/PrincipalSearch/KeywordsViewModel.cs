using System.ComponentModel;
using Isogeo.AddIn.Models;
using Isogeo.Map.MapFunctions;
using Isogeo.Models;
using Isogeo.Models.Filters;
using Isogeo.Models.Network;
using MVVMPattern;

namespace Isogeo.AddIn.ViewsModels.Search.PrincipalSearch
{
    public class KeywordsViewModel : ViewModelBase
    {
        public string ComponentName => Language.Resources.Keywords;

        private readonly FilterManager _filterManager;

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

        public KeywordsViewModel(FilterManager filterManager, RestFunctions restFunctions, IMapFunctions mapFunctions)
        {
            _filterManager = filterManager;
            Filters = new Filters("keyword:isogeo", restFunctions, filterManager, mapFunctions);
            Filters.PropertyChanged += Filter_PropertyChanged;
            Variables.functionsSetlist.Add(SetList);
            _filterManager.AddFilters(Filters);
        }

        private void SetList()
        {
            _filterManager.SetListCombo(Filters, "keyword:isogeo");
        }
    }
}
