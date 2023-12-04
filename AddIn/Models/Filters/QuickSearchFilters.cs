using System.Collections.Generic;
using Isogeo.AddIn.Models.Filters.Components;
using Isogeo.Map.MapFunctions;
using Isogeo.Models;
using Isogeo.Models.Configuration;
using Isogeo.Network;

namespace Isogeo.AddIn.Models.Filters
{
    public class QuickSearchFilters : Components.Filters
    {
        public QuickSearchFilters(string name, INetworkManager networkManager, FilterManager filterManager, IMapFunctions mapFunctions) : base(name, networkManager, filterManager, mapFunctions)
        {
        }

        protected override async void SelectionChanged()
        {
            if (Variables.listLoading || SelectedItem == null || SelectedItem.Name == "-")
                return;
            var ob = FilterManager.GetOb();
            var od = FilterManager.GetOd();
            await NetworkManager.LoadData(SelectedItem.Id, 0, SelectedItem.GeographicalOperator, od, ob);
            FilterManager.SetSearchList(SelectedItem.Id);
        }

        public void SelectItem(string name, string id, string box)
        {
            foreach (var item in List.Items)
            {
                if (item.Name == name && item.Id == id && item.GeographicalOperator == box)
                {
                    List.Selected = item;
                }
            }
            OnPropertyChanged(nameof(SelectedItem));
        }

        public virtual void AddItem(Search item)
        {
            base.AddItem(new FilterItem(item.Query, item.Name));
        }

        public virtual void SetItems(List<Search> items)
        {
            var comboItems = new List<FilterItem>();

            for (var i = 0; i < items.Count; i += 1)
            {
                comboItems.Add(new FilterItem(items[i].Query, items[i].Name) { GeographicalOperator = items[i].Box});
            }
            base.SetItems(comboItems);
        }
    }
}
