using MVVMPattern;

namespace Isogeo.Models.Filters
{
    public class FilterItem : ViewModelBase
    {
        private string _name;
        private string _id;
        private string _geographicalOperator;

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }

        public string Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged("Id");
            }
        }

        public string GeographicalOperator
        {
            get => _geographicalOperator;
            set
            {
                _geographicalOperator = value;
                OnPropertyChanged("GeographicalOperator");
            }
        }
    }
}
