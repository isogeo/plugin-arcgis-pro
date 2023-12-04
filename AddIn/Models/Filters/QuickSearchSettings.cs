using System.Collections.Generic;
using Isogeo.AddIn.Models;
using Isogeo.Map.MapFunctions;
using Isogeo.Models.Configuration;
using Isogeo.Models.Network;

namespace Isogeo.Models.Filters
{
    public class QuickSearchSettings : QuickSearch
    {
        public QuickSearchSettings(string name, RestFunctions restFunctions, FilterManager filterManager, IMapFunctions mapFunctions) : base(name, restFunctions, filterManager, mapFunctions)
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
            if (item?.name == Language.Resources.Previous_search || item?.name == "-") return;
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
                if (items[i].name == Language.Resources.Previous_search) continue;
                comboItems.Add(new FilterItem (items[i].query, items[i].name) { GeographicalOperator = items[i].box});
            }
            SetItems(comboItems);
        }
    }
}
