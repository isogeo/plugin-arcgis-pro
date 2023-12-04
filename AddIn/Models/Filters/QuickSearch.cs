using System.Collections;
using System.Collections.Generic;
using ArcGIS.Desktop.Framework.Threading.Tasks;
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

        protected override void SelectionChanged()
        {
            if (Variables.listLoading || SelectedItem == null || SelectedItem.Name == "-") 
                return;
            QueuedTask.Run(() =>
            {
                var ob = FilterManager.GetOb();
                var od = FilterManager.GetOd();
                var query = FilterManager.GetQueryCombos();
                FilterManager.SetSearchList(query);
                RestFunctions.LoadData(SelectedItem.Id, 0, SelectedItem.GeographicalOperator, od, ob);
            });
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
