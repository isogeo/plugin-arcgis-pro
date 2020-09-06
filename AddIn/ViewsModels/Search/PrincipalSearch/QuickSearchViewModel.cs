using System.ComponentModel;
using System.Linq;
using Isogeo.Models;
using Isogeo.Models.Configuration;
using Isogeo.Models.Filters;
using Isogeo.Utils.Box;
using MVVMPattern;
using MVVMPattern.MediatorPattern;

namespace Isogeo.AddIn.ViewsModels.Search.PrincipalSearch
{
    public class QuickSearchViewModel : ViewModelBase
    {
        public string ComponentName => Language.Resources.Quick_search;

        private QuickSearch _quickSearch;
        public QuickSearch Filters
        {
            get => _quickSearch;
            set
            {
                _quickSearch = value;
                OnPropertyChanged("Filters");
            }
        }

        private void QuickSearch_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged("Filters");
        }

        private void AddQuickSearchEvent(object newSearch)
        {
            Filters.AddItem((Models.Configuration.Search)newSearch);
            Filters.SelectItem(((Models.Configuration.Search)newSearch).name);
        }

        private void InitializeQuickSearch()
        {
            var cmbName = Filters.SelectedItem?.Name;
            Filters.SetItems(Variables.configurationManager.config.searchs.searchs);
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
            var query = ((QueryItem) queryItem).query;
            var box = ((QueryItem) queryItem).box;
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

        public QuickSearchViewModel()
        {
            Filters = new QuickSearch("QuickSearch");
            Filters.PropertyChanged += QuickSearch_PropertyChanged;
            Filters.SetItems(Variables.configurationManager.config.searchs.searchs);
            Mediator.Register("AddNewQuickSearch", AddQuickSearchEvent);
            Mediator.Register("ChangeQuickSearch", ChangeQuickSearchEvent);
            Mediator.Register("ChangeQuery", ChangeSelectedQuickSearchItemEvent);
        }
    }
}
