﻿using System.ComponentModel;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using Isogeo.AddIn.Models.FilterManager;
using Isogeo.AddIn.Models.Filters.Components;
using Isogeo.Map;
using Isogeo.Models;
using Isogeo.Network;
using Isogeo.Utils.Box;
using MVVMPattern;
using MVVMPattern.MediatorPattern;

namespace Isogeo.AddIn.ViewsModels.Search.AdvancedSearch
{
    public class AdvancedSearchItemViewModel : ViewModelBase
    {
        public string DisplayName { get; set; }
        public string ImgPath { get; set; }
        private string _filterName;
        private readonly IMapManager _mapManager;

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

        public AdvancedSearchItemViewModel(string displayName, string imageSearchPath, string apiFilterName, INetworkManager networkManager,
            IMapManager mapManager, IFilterManager filterManager)
        {
            DisplayName = displayName;
            ImgPath = imageSearchPath;
            _mapManager = mapManager;
            _filterManager = filterManager;
            Filters = new Filters(apiFilterName, networkManager, filterManager, mapManager);
            Filters.PropertyChanged += Filter_PropertyChanged;
            Init(apiFilterName);
        }

        private void Init(string listName)
        {
            _filterName = listName;
            if (listName != "map")
            {
                _filterManager.AddFunctionToSetFilterList(SetList);
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

        private static bool CheckEqualityBox(string box1, string box2, double precision)
        {
            if (string.IsNullOrWhiteSpace(box1) && string.IsNullOrWhiteSpace(box2))
                return true;
            if (string.IsNullOrWhiteSpace(box1) || string.IsNullOrWhiteSpace(box2))
                return false;
            return BoxUtils.BoxAreEquals(box1, box2, precision);
        }

        private void ChangeBoxEvent(object box)
        {
            if ((string) box != null && (string) box == "")
                Filters.SelectItem("-");
            else
            {
                Filters.SelectItem(Language.Resources.Map_canvas);

                var mapExtent = _mapManager.GetMapExtent();
                if (!string.IsNullOrEmpty((string)box) && !string.IsNullOrEmpty(mapExtent) && 
                    !CheckEqualityBox((string)box, mapExtent, 0.01))
                {
                    QueuedTask.Run(() =>
                    {
                        _mapManager.SetMapExtent((string)box);
                    });
                }
            }
        }

        private void SetGeographicOperator()
        {
            var mapCanvas = new FilterItem("-1", Language.Resources.Map_canvas);
            Filters.Items.Add(mapCanvas);
            Mediator.Register(MediatorEvent.ChangeBox, ChangeBoxEvent);
        }
    }
}
