using System.Collections.ObjectModel;
using System.Linq;
using MVVMPattern;

namespace Isogeo.Models.Filters
{
    public class FilterItemList : ViewModelBase
    {
        private ObservableCollection<FilterItem> _items;

        public ObservableCollection<FilterItem> Items
        {
            get => _items;
            set
            {
                _items = value;
                OnPropertyChanged(nameof(Items));
            }
        }

        private FilterItem _selected;

        public FilterItem Selected
        {
            get => _selected;
            set
            {
                _selected = value;
                OnPropertyChanged(nameof(Selected));
            }
        }

        public FilterItemList()
        {
            Items = new ObservableCollection<FilterItem>();
        }

        internal void SelectByName(string p)
        {
            Selected = Items.FirstOrDefault(s => s?.Name != null && s.Name.Equals(p));
        }

        /// <summary>
        /// Select Item by checking Id without spaces and upper cases
        /// </summary>
        /// <param name="id"></param>
        internal void SelectById(string id)
        {
            Selected = Items.FirstOrDefault(s => s?.Id != null && id != null && s.Name != Language.Resources.Previous_search &&
                                                 string.Concat(s.Id.Where(c => !char.IsWhiteSpace(c))).ToLower()
                                                     .Equals(string.Concat(id.Where(c => !char.IsWhiteSpace(c))).ToLower()));
        }

        public int GetIndex(string p)
        {
            for (var i = 0; i < Items.Count; i += 1)
            {
                if (Items[i].Name == p)
                    return i;
            }
            return -1;
        }
    }

}
