using System.ComponentModel;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using Isogeo.AddIn.Models;
using Isogeo.AddIn.Models.Filters.Components;
using Isogeo.Map.MapFunctions;
using Isogeo.Models;
using Isogeo.Models.Network;
using MVVMPattern;
using MVVMPattern.MediatorPattern;

namespace Isogeo.AddIn.ViewsModels.Search.AdvancedSearch
{
    public class AdvancedSearchItemViewModel : ViewModelBase
    {
        public string DisplayName { get; set; }
        public string ImgPath { get; set; }
        private string _filterName;
        private readonly IMapFunctions _mapFunctions;

        private readonly FilterManager _filterManager;

        public bool IsCustomQuery { get; private set; }

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

        public AdvancedSearchItemViewModel(string displayName, string imageSearchPath, string apiFilterName, RestFunctions restFunctions,
            IMapFunctions mapFunctions, FilterManager filterManager)
        {
            DisplayName = displayName;
            ImgPath = imageSearchPath;
            _mapFunctions = mapFunctions;
            _filterManager = filterManager;
            Filters = new Filters(apiFilterName, restFunctions, filterManager, mapFunctions);
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
                _filterManager.AddFilters(Filters);
            }
            else
            {
                _filterManager.SetGeographicFilter(Filters);
                SetGeographicOperator();
            }
        }

        private void SetList()
        {
            _filterManager.SetListCombo(Filters, _filterName);
        }

        private void ChangeBoxEvent(object box)
        {
            if ((string) box != null && (string) box == "")
                Filters.SelectItem("-");
            else
            {
                Filters.SelectItem(Language.Resources.Map_canvas);
                QueuedTask.Run(() =>
                {
                    _mapFunctions.SetMapExtent((string)box);
                });
            }
        }

        private void SetGeographicOperator()
        {
            var mapCanvas = new FilterItem("-1", Language.Resources.Map_canvas);
            Filters.Items.Add(mapCanvas);
            Mediator.Register("ChangeBox", ChangeBoxEvent);
        }
    }
}
