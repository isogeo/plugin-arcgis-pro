using System.Collections.Generic;
using Isogeo.AddIn.Models.FilterManager;
using Isogeo.AddIn.Models.Filters.Components;
using Isogeo.Map;
using Isogeo.Network;
using Isogeo.Utils.Configuration;

namespace Isogeo.AddIn.Models.Filters
{
    public class QuickSearchFilters : Components.Filters
    {
        public QuickSearchFilters(string name, INetworkManager networkManager, IFilterManager filterManager, IMapManager mapManager) : base(name, networkManager, filterManager, mapManager)
        {
        }

        protected override async void SelectionChanged()
        {
            if (FilterManager.FilterListsLoading || SelectedItem == null || SelectedItem.Name == "-")
                return;
            var ob = FilterManager.GetOb();
            var od = FilterManager.GetOd();
            await NetworkManager.LoadData(SelectedItem.Id, 0, SelectedItem.GeographicalOperator, od, ob);
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
            base.AddItem(new FilterItem(item.Query, item.Name) { GeographicalOperator = item.Box});
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
