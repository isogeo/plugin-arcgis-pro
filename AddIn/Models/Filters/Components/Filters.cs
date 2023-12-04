using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Isogeo.AddIn.Models;
using Isogeo.Map.MapFunctions;
using Isogeo.Models;
using Isogeo.Models.Network;
using MVVMPattern;

namespace Isogeo.AddIn.Models.Filters.Components
{
    public class Filters : ViewModelBase
    {
        public string Name { get; }
        protected FilterItemList List { get; }

        protected RestFunctions RestFunctions { get; }

        protected FilterManager FilterManager { get; }

        protected IMapFunctions MapFunctions { get; }

        public ObservableCollection<FilterItem> Items => List.Items;

        private void List_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(nameof(Items));
        }

        public Filters(string name, RestFunctions restFunctions, FilterManager filterManager, IMapFunctions mapFunctions)
        {
            RestFunctions = restFunctions;
            FilterManager = filterManager;
            MapFunctions = mapFunctions;
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
                RestFunctions.SaveSearch(box, query);
                List.Selected = value;
                SelectionChanged();
            }
        }

        protected virtual async void SelectionChanged()
        {
            if (Variables.listLoading)
                return;
            var query = FilterManager.GetQueryCombos();
            var box = FilterManager.GetBoxRequest();

            var ob = FilterManager.GetOb();
            var od = FilterManager.GetOd();
            await RestFunctions.ReloadData(0, query, box, od, ob);
            FilterManager.SetSearchList(query);
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
                List.Items.Add(new FilterItem(item.Id, itemNameToAdd));
        }

        public virtual void SetItems(List<FilterItem> items)
        {
            List.Items.Clear();
            AddItem(new FilterItem("", "-"));
            List.Selected = null;

            for (var i = 0; i < items.Count; i += 1)
            {
                AddItem(items[i]);
            }

            if (List.Items.Count > 0)
            {
                List.Selected = List.Items[0];
                OnPropertyChanged(nameof(SelectedItem));
            }
        }
    }
}
