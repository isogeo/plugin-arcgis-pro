using Isogeo.AddIn.Models;
using Isogeo.Map.MapFunctions;
using Isogeo.Models.Network;
using System.Collections.Generic;

namespace Isogeo.Models.Filters
{
    public class GeoGraphicalSettings : Filters
    {
        public GeoGraphicalSettings(string name, RestFunctions restFunctions, FilterManager filterManager, IMapFunctions mapFunctions) : base(name, restFunctions, filterManager, mapFunctions)
        {
            var items = new List<FilterItem>
            {
                new("intersects", Language.Resources.Geographic_type_intersects),
                new("within", Language.Resources.Geographic_type_within),
                new("contains", Language.Resources.Geographic_type_contains)
            };
            base.SetItems(items);
            RemoveFirstItem();
        }

        private void RemoveFirstItem()
        {
            if (Items.Count > 1)
            {
                Items.Remove(Items[0]);
                List.SetSelected(Items[0]);
            }
        }

        public override FilterItem SelectedItem
        {
            get => List.Selected;
            set
            {
                OnPropertyChanged(nameof(SelectedItem));
                if (value == null || (List.Selected != null && value.Name == List.Selected.Name))
                    return;
                List.SetSelected(value);
                SelectionChanged();
            }
        }

        protected override void SelectionChanged()
        {
            Variables.configurationManager.config.geographicalOperator = SelectedItem.Id;
            Variables.configurationManager.Save();
            base.SelectionChanged();
        }

        public override void SetItems(List<FilterItem> items)
        {
            base.SetItems(items);
            RemoveFirstItem();
        }
    }
}
