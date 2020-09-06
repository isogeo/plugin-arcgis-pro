using System.Collections.Generic;
using Isogeo.Models.Configuration;

namespace Isogeo.Models.Filters
{
    public class QuickSearch : Filters
    {
        public QuickSearch(string name) : base(name)
        {
        }

        protected override void SelectionChanged()
        {
            if (Variables.listLoading || SelectedItem == null || SelectedItem.Name == "-") return;
            if (Variables.restFunctions != null)
            {
                Variables.restFunctions.LoadData(SelectedItem.Id, 0, SelectedItem.GeographicalOperator);
            }
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
