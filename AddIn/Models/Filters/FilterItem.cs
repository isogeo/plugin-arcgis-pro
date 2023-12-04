using MVVMPattern;

namespace Isogeo.Models.Filters
{
    public class FilterItem : ViewModelBase
    {
        private string _geographicalOperator;

        public string Name { get; }

        public string Id { get; }

        public FilterItem(string id, string name)
        {
            Id = id;
            Name = name;
        }

        public string GeographicalOperator
        {
            get => _geographicalOperator;
            set
            {
                _geographicalOperator = value;
                OnPropertyChanged(nameof(GeographicalOperator));
            }
        }
    }
}
