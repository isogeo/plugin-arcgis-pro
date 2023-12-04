using System.Collections.Generic;
using Isogeo.AddIn.Models.Filters.Components;
using Isogeo.Map;
using Isogeo.Models;
using Isogeo.Network;

namespace Isogeo.AddIn.Models.Filters
{
    public class GeoGraphicalSettingsFilters : Components.Filters
    {
        public GeoGraphicalSettingsFilters(string name, INetworkManager networkManager, FilterManager filterManager, IMapManager mapManager) : base(name, networkManager, filterManager, mapManager)
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
                List.Selected = Items[0];
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
                List.Selected = value;
                SelectionChanged();
            }
        }

        protected override void SelectionChanged()
        {
            Variables.configurationManager.config.GeographicalOperator = SelectedItem.Id;
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
