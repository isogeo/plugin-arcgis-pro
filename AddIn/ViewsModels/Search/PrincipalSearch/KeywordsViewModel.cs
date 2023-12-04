using System.ComponentModel;
using Isogeo.AddIn.Models;
using Isogeo.AddIn.Models.Filters.Components;
using Isogeo.Map.MapFunctions;
using Isogeo.Models;
using Isogeo.Models.Network;
using Isogeo.Network;
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

        public KeywordsViewModel(FilterManager filterManager, IRestFunctions restFunctions, IMapFunctions mapFunctions)
        {
            _filterManager = filterManager;
            Filters = new Filters("keyword:isogeo", restFunctions, filterManager, mapFunctions);
            Filters.PropertyChanged += Filter_PropertyChanged;
            Variables.FunctionsSetlist.Add(SetList);
            _filterManager.AddFilters(Filters);
        }

        private void SetList()
        {
            _filterManager.SetListCombo(Filters, "keyword:isogeo");
        }
    }
}
