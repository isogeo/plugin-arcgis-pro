using System.ComponentModel;
using Isogeo.Models;
using Isogeo.Models.Filters;
using MVVMPattern;

namespace Isogeo.AddIn.ViewsModels.Search.PrincipalSearch
{
    public class KeywordsViewModel : ViewModelBase
    {
        public string ComponentName => Language.Resources.Keywords;

        private Filters _filters;
        public Filters Filters
        {
            get => _filters;
            set
            {
                _filters = value;
                OnPropertyChanged("Filters");
            }
        }

        private void Filter_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged("Filters");
        }

        public KeywordsViewModel()
        {
            Filters = new Filters("keyword:isogeo");
            Filters.PropertyChanged += Filter_PropertyChanged;
            Variables.functionsSetlist.Add(SetList);
            Variables.listComboFilter.Add(Filters);
        }

        private void SetList()
        {
            Variables.restFunctions.SetListCombo(Filters, "keyword:isogeo");
        }
    }
}
