using System.Collections.Generic;
using Isogeo.AddIn.Models.FilterManager;
using Isogeo.AddIn.Models.Filters.Components;
using Isogeo.Map;
using Isogeo.Models.Configuration;
using Isogeo.Network;

namespace Isogeo.AddIn.Models.Filters
{
    public class QuickSearchSettingsFilters : QuickSearchFilters
    {
        public QuickSearchSettingsFilters(string name, INetworkManager networkManager, IFilterManager filterManager, IMapManager mapManager) : base(name, networkManager, filterManager, mapManager)
        {
        }

        protected override void SelectionChanged()
        {
            // Method intentionally left empty.
        }

        public override void AddItem(FilterItem item)
        {
            if (item?.Name == Language.Resources.Previous_search || item?.Name == "-") return;
            base.AddItem(item);
        }

        public override void AddItem(Search item)
        {
            if (item?.Name == Language.Resources.Previous_search || item?.Name == "-") return;
            base.AddItem(item);
        }

        public override FilterItem SelectedItem
        {
            get => List.Selected;
            set
            {
                OnPropertyChanged(nameof(SelectedItem));
                if (value == null || (List.Selected != null && value.Name == List.Selected.Name))
                    return;
                List.Selected = value;
                SelectionChanged();
            }
        }

        public override void SetItems(List<Search> items)
        {
            var comboItems = new List<FilterItem>();

            for (var i = 0; i < items.Count; i += 1)
            {
                if (items[i].Name == Language.Resources.Previous_search) continue;
                comboItems.Add(new FilterItem (items[i].Query, items[i].Name) { GeographicalOperator = items[i].Box});
            }
            SetItems(comboItems);
        }
    }
}
