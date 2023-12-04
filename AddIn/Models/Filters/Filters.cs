using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using Isogeo.AddIn.Models;
using Isogeo.Map.MapFunctions;
using Isogeo.Models.Network;
using MVVMPattern;

namespace Isogeo.Models.Filters
{
    public class Filters : ViewModelBase
    {
        public string Name { get; }
        protected FilterItemList List { get; }

        protected RestFunctions RestFunctions { get; }

        protected FilterManager FilterManager { get; }

        protected IMapFunctions MapFunctions { get; }

        public ObservableCollection<FilterItem> Items
        {
            get => List.Items;
            set
            {
                List.Items = value;
                SelectedItem = value[0];
                OnPropertyChanged("Items");
            }
        }

        private void List_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged("Items");
        }

        public Filters(string name, RestFunctions restFunctions, FilterManager filterManager, IMapFunctions mapFunctions)
        {
            RestFunctions = restFunctions;
            FilterManager = filterManager;
            MapFunctions = mapFunctions;
            Name = name;
            List = new FilterItemList();
            List.PropertyChanged += List_PropertyChanged;
            var item = new FilterItem { Name = "-" };
            List.Items.Add(item);
            List.Selected = item;
        }

        public virtual FilterItem SelectedItem
        {
            get => List.Selected;
            set
            {
                OnPropertyChanged("SelectedItem");
                if (value == null || (List.Selected != null && value.Name == List.Selected.Name))
                    return;
                var query = FilterManager.GetQueryCombos();
                var box = FilterManager.GetBoxRequest();
                RestFunctions.SaveSearch(box, query);
                List.Selected = value;
                SelectionChanged();
            }
        }

        protected virtual void SelectionChanged()
        {
            if (Variables.listLoading) 
                return;
            QueuedTask.Run(() =>
            {
                var query = FilterManager.GetQueryCombos();
                var box = FilterManager.GetBoxRequest();

                var ob = FilterManager.GetOb();
                var od = FilterManager.GetOd();
                FilterManager.SetSearchList(query);
                RestFunctions.ReloadData(0, query, box, od, ob);
            });
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
            OnPropertyChanged("SelectedItem");
        }

        public virtual void AddItem(FilterItem item)
        {
            while (Items.Any(search => search.Name == item.Name))
                item.Name += " - " + Language.Resources.Copy.ToLower();
            if (!string.IsNullOrWhiteSpace(item.Name))
                List.Items.Add(item);
        }

        public virtual void SetItems(List<FilterItem> items)
        {
            List.Items.Clear();
            AddItem(new FilterItem {Id = "", Name = "-"});
            List.Selected = null;

            for (var i = 0; i < items.Count; i += 1)
            {
                AddItem(items[i]);
            }

            if (List.Items.Count > 0)
            {
                List.Selected = List.Items[0];
                OnPropertyChanged("SelectedItem");
            }
        }
    }
}
