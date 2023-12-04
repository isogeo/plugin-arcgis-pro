using System.Collections.Generic;
using Isogeo.AddIn.Models.Filters.Components;
using Isogeo.Map.MapFunctions;
using Isogeo.Models;
using Isogeo.Models.Configuration;
using Isogeo.Models.Network;

namespace Isogeo.AddIn.Models.Filters
{
    public class QuickSearchFilters : Components.Filters
    {
        public QuickSearchFilters(string name, RestFunctions restFunctions, FilterManager filterManager, IMapFunctions mapFunctions) : base(name, restFunctions, filterManager, mapFunctions)
        {
        }

        protected override async void SelectionChanged()
        {
            if (Variables.listLoading || SelectedItem == null || SelectedItem.Name == "-")
                return;
            var ob = FilterManager.GetOb();
            var od = FilterManager.GetOd();
            await RestFunctions.LoadData(SelectedItem.Id, 0, SelectedItem.GeographicalOperator, od, ob);
            FilterManager.SetSearchList(SelectedItem.Id);
        }

        public void SelectItem(string name, string id, string box)
        {
            foreach (var item in List.Items)
            {
                if (item.Name == name && item.Id == id && item.GeographicalOperator == box)
                {
                    List.SetSelected(item);
                }
            }
            OnPropertyChanged(nameof(SelectedItem));
        }

        public virtual void AddItem(Search item)
        {
            base.AddItem(new FilterItem(item.query, item.name));
        }

        public virtual void SetItems(List<Search> items)
        {
            var comboItems = new List<FilterItem>();

            for (var i = 0; i < items.Count; i += 1)
            {
                comboItems.Add(new FilterItem(items[i].query, items[i].name) { GeographicalOperator = items[i].box});
            }
            base.SetItems(comboItems);
        }
    }
}
