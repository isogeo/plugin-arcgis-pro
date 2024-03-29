﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using ActiproSoftware.Windows.Extensions;
using Isogeo.AddIn.Models.FilterManager;
using Isogeo.Map;
using Isogeo.Network;
using MVVMPattern;

namespace Isogeo.AddIn.Models.Filters.Components
{
    public class Filters : ViewModelBase
    {
        public string Name { get; }
        protected FilterItemList List { get; }

        protected INetworkManager NetworkManager { get; }

        protected IFilterManager FilterManager { get; }

        protected IMapManager MapManager { get; }

        public ObservableCollection<FilterItem> Items => List.Items;

        private void List_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(nameof(Items));
        }

        public Filters(string name, INetworkManager networkManager, IFilterManager filterManager, IMapManager mapManager)
        {
            NetworkManager = networkManager;
            FilterManager = filterManager;
            MapManager = mapManager;
            Name = name;
            List = new FilterItemList();
            List.PropertyChanged += List_PropertyChanged;
            var item = new FilterItem("-1", "-");
            List.Items.Add(item);
            List.Selected = item;
        }

        public virtual FilterItem SelectedItem
        {
            get => List.Selected;
            set
            {
                OnPropertyChanged(nameof(SelectedItem));
                if (value == null || List.Selected != null && value.Name == List.Selected.Name)
                    return;
                var query = FilterManager.GetQueryCombos();
                var box = FilterManager.GetBoxRequest();
                NetworkManager.SaveSearch(box, query);
                List.Selected = value;
                SelectionChanged();
            }
        }

        protected virtual async void SelectionChanged()
        {
            if (FilterManager.FilterListsLoading)
                return;
            var query = FilterManager.GetQueryCombos();
            var box = FilterManager.GetBoxRequest();

            var ob = FilterManager.GetOb();
            var od = FilterManager.GetOd();
            await NetworkManager.LoadData(query, 0, box, od, ob);
        }

        public virtual void SelectItem(string name = null, string id = null)
        {
            if (id != null && name != null)
            {
                List.SelectById(id);
            }
            else
            {
                if (name == SelectedItem?.Name)
                    return;
                List.SelectByName(name);
            }
            OnPropertyChanged(nameof(SelectedItem));
        }

        public virtual void AddItem(FilterItem item)
        {
            var itemNameToAdd = item.Name;
            while (Items.Any(search => search.Name == itemNameToAdd))
                itemNameToAdd += " - " + Language.Resources.Copy.ToLower();
            if (!string.IsNullOrWhiteSpace(itemNameToAdd))
                List.Items.Add(new FilterItem(item.Id, itemNameToAdd) { GeographicalOperator = item.GeographicalOperator});
        }

        private static void AddItem(FilterItem item, ICollection<FilterItem> list)
        {
            var itemNameToAdd = item.Name;
            while (list.Any(search => search.Name == itemNameToAdd))
                itemNameToAdd += " - " + Language.Resources.Copy.ToLower();
            if (!string.IsNullOrWhiteSpace(itemNameToAdd))
                list.Add(new FilterItem(item.Id, itemNameToAdd) { GeographicalOperator = item.GeographicalOperator });
        }

        public virtual void SetItems(List<FilterItem> items)
        {
            // performance: List.Items is an ObservableCollection, by filling a temporary simple list then use addRange() at the end,
            // we don't trigger the UI for each item added (only once triggered at the end of AddRange())
            var temporaryFilterItemList = new List<FilterItem>();

            List.Items.Clear();
            AddItem(new FilterItem("", "-"), temporaryFilterItemList);
            List.Selected = null;

            for (var i = 0; i < items.Count; i += 1)
                AddItem(items[i], temporaryFilterItemList);
            List.Items.AddRange(temporaryFilterItemList);

            if (List.Items.Count > 0)
            {
                List.Selected = List.Items[0];
                OnPropertyChanged(nameof(SelectedItem));
            }
        }
    }
}
