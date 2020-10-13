﻿using System.Collections.Generic;
using Isogeo.Models.Configuration;

namespace Isogeo.Models.Filters
{
    public class QuickSearchSettings : QuickSearch
    {
        public QuickSearchSettings(string name) : base(name)
        {
        }

        protected override void SelectionChanged()
        {
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
                OnPropertyChanged("SelectedItem");
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
                comboItems.Add(new FilterItem {Id = items[i].query, Name = items[i].name, GeographicalOperator = items[i].box});
            }
            SetItems(comboItems);
        }
    }
}