using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Isogeo.AddIn.Models;
using Isogeo.Map.MapFunctions;
using Isogeo.Models.Configuration;
using Isogeo.Models.Network;

namespace Isogeo.Models.Filters
{
    public class QuickSearch : Filters
    {
        public QuickSearch(string name, RestFunctions restFunctions, FilterManager filterManager, IMapFunctions mapFunctions) : base(name, restFunctions, filterManager, mapFunctions)
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
                    List.Selected = item;
                }
            }
            OnPropertyChanged("SelectedItem");
        }

        public virtual void AddItem(Search item)
        {
            base.AddItem(new FilterItem {Id = item.query, Name = item.name});
        }

        public virtual void SetItems(List<Search> items)
        {
            var comboItems = new List<FilterItem>();

            for (var i = 0; i < items.Count; i += 1)
            {
                comboItems.Add(new FilterItem {Id = items[i].query, Name = items[i].name, GeographicalOperator = items[i].box});
            }
            base.SetItems(comboItems);
        }
    }
}
