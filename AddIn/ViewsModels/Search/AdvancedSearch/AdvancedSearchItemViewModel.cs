using System.ComponentModel;
using Isogeo.Map.MapFunctions;
using Isogeo.Models;
using Isogeo.Models.Filters;
using MVVMPattern;
using MVVMPattern.MediatorPattern;

namespace Isogeo.AddIn.ViewsModels.Search.AdvancedSearch
{
    public class AdvancedSearchItemViewModel : ViewModelBase
    {
        public string DisplayName { get; set; }
        public string ImgPath { get; set; }
        private string _filterName;

        public bool IsCustomQuery { get; private set; }

        private Filters _filters;
        public Filters Filters
        {
            get => _filters;
            set
            {
                _filters = value;
                OnPropertyChanged("Filters");
            }
        }

        private void Filter_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged("Filters");
        }

        public AdvancedSearchItemViewModel(string displayName, string imageSearchPath, string apiFilterName)
        {
            DisplayName = displayName;
            ImgPath = imageSearchPath;
            Filters = new Filters(apiFilterName);
            Filters.PropertyChanged += Filter_PropertyChanged;
            Mediator.Register("isCustomQuery", IsCustomQueryEvent);
            Init(apiFilterName);
        }

        public void IsCustomQueryEvent(object obj)
        {
            IsCustomQuery = (bool)obj;
        }

        private void Init(string listName)
        {
            _filterName = listName;
            if (listName != "keyword:isogeo")
            {
                Variables.functionsSetlist.Add(SetList);
                Variables.listComboFilter.Add(Filters);
            }
            else
            {
                Variables.geographicFilter = Filters;
                SetGeographicOperator();
            }
        }

        private void SetList()
        {
            Variables.restFunctions.SetListCombo(Filters, _filterName);
        }

        private void ChangeBoxEvent(object box)
        {
            if ((string) box != null && (string) box == "")
                Filters.SelectItem("-");
            else
            {
                Filters.SelectItem(Language.Resources.Map_canvas);
                MapFunctions.SetMapExtent((string)box);
            }
        }

        private void SetGeographicOperator()
        {
            var mapCanvas = new FilterItem {Name = Language.Resources.Map_canvas};
            Filters.Items.Add(mapCanvas);
            Mediator.Register("ChangeBox", ChangeBoxEvent);
        }
    }
}
