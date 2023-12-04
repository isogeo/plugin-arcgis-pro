using System.Collections.Generic;
using Isogeo.AddIn.Models.FilterManager;
using Isogeo.AddIn.Models.Filters.Components;
using Isogeo.Map;
using Isogeo.Models.Configuration;
using Isogeo.Network;

namespace Isogeo.AddIn.Models.Filters
{
    public class GeoGraphicalSettingsFilters : Components.Filters
    {
        private readonly ConfigurationManager _configurationManager;

        public GeoGraphicalSettingsFilters(string name, INetworkManager networkManager, IFilterManager filterManager, IMapManager mapManager,
            ConfigurationManager configurationManager) : base(name, networkManager, filterManager, mapManager)
        {
            _configurationManager = configurationManager;
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
            _configurationManager.config.GeographicalOperator = SelectedItem.Id;
            _configurationManager.Save();
            base.SelectionChanged();
        }

        public override void SetItems(List<FilterItem> items)
        {
            base.SetItems(items);
            RemoveFirstItem();
        }
    }
}
