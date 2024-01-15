using System.ComponentModel;
using System.Linq;
using Isogeo.AddIn.Models.FilterManager;
using Isogeo.AddIn.Models.Filters;
using Isogeo.AddIn.Models.Filters.Components;
using Isogeo.Map;
using Isogeo.Models;
using Isogeo.Network;
using Isogeo.Utils.Box;
using Isogeo.Utils.Configuration;
using Isogeo.Utils.ConfigurationManager;
using MVVMPattern;
using MVVMPattern.MediatorPattern;

namespace Isogeo.AddIn.ViewsModels.Search.PrincipalSearch
{
    public class QuickSearchViewModel : ViewModelBase
    {
        private readonly IConfigurationManager _configurationManager;

        public string ComponentName => Language.Resources.Quick_search;

        private readonly IFilterManager _filterManager;

        private QuickSearchFilters _quickSearchFilter;
        public QuickSearchFilters Filters
        {
            get => _quickSearchFilter;
            set
            {
                _quickSearchFilter = value;
                OnPropertyChanged(nameof(Filters));
            }
        }

        private void QuickSearch_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(nameof(Filters));
        }

        private void AddQuickSearchEvent(object newSearch)
        {
            Filters.AddItem((Utils.Configuration.Search)newSearch);
            Filters.SelectItem(((Utils.Configuration.Search)newSearch).Name);
        }

        private void InitializeQuickSearch()
        {
            var cmbName = Filters.SelectedItem?.Name;
            Filters.SetItems(_configurationManager.Config.Searchs.SearchDetails);
            Filters.SelectItem(Filters.Items.Any(s => s?.Name != null && 
                                                      !string.IsNullOrWhiteSpace(cmbName) &&
                                                      cmbName == s.Name)
                ? cmbName
                : "-");
        }

        private void ChangeQuickSearchEvent(object obj)
        {
            InitializeQuickSearch();
        }

        private static bool CheckEqualityBox(string box1, string box2, double precision)
        {
            if (string.IsNullOrWhiteSpace(box1) && string.IsNullOrWhiteSpace(box2))
                return true;
            if (string.IsNullOrWhiteSpace(box1) || string.IsNullOrWhiteSpace(box2))
                return false;
            return BoxUtils.BoxAreEquals(box1, box2, precision);
        }

        private void ChangeSelectedQuickSearchItemEvent(object queryItem)
        {
            var query = ((QueryItem) queryItem).Query;
            var box = _filterManager.GetBoxRequest();
            var filterItems = Filters.Items.Where(s =>
                s?.Id != null && !string.IsNullOrWhiteSpace(query) && s.Name != Language.Resources.Previous_search &&
                CheckEqualityBox(box, s.GeographicalOperator, 0.01) &&
                string.Concat(s.Id.Where(c => !char.IsWhiteSpace(c))).ToLower()
                    .Equals(string.Concat(query.Where(c => !char.IsWhiteSpace(c))).ToLower()));
            var enumerable = filterItems as FilterItem[] ?? filterItems.ToArray();
            if (enumerable.Any())
            {
                Filters.SelectItem(enumerable.First().Name, enumerable.First().Id,
                    enumerable.First().GeographicalOperator);
                return;
            }

            if (string.IsNullOrWhiteSpace(query))
            {
                filterItems = Filters.Items.Where(s => s != null &&
                                                       string.IsNullOrWhiteSpace(s.Id) && 
                                                       s.Name != Language.Resources.Previous_search &&
                                                       CheckEqualityBox(box, s.GeographicalOperator, 0.01));
                enumerable = filterItems as FilterItem[] ?? filterItems.ToArray();
                if (enumerable.Any())
                {
                    Filters.SelectItem(enumerable.First().Name, enumerable.First().Id,
                        enumerable.First().GeographicalOperator);
                    return;
                }
            }
            Filters.SelectItem("-");
        }

        public QuickSearchViewModel(INetworkManager networkManager, IFilterManager filterManager, IMapManager mapManager,
            IConfigurationManager configurationManager)
        {
            _configurationManager = configurationManager;
            _filterManager = filterManager;
            Filters = new QuickSearchFilters("QuickSearch", networkManager, filterManager, mapManager);
            Filters.PropertyChanged += QuickSearch_PropertyChanged;
            Filters.SetItems(_configurationManager.Config.Searchs.SearchDetails);
            Mediator.Register(MediatorEvent.AddNewQuickSearch, AddQuickSearchEvent);
            Mediator.Register(MediatorEvent.ChangeQuickSearch, ChangeQuickSearchEvent);
            Mediator.Register(MediatorEvent.ChangeQuery, ChangeSelectedQuickSearchItemEvent);
        }
    }
}
