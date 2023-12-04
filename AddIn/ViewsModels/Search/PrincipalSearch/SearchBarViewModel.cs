using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Isogeo.AddIn.Models;
using Isogeo.Models.Network;
using Isogeo.Models;
using System.Windows;
using MVVMPattern;
using System.Windows;
using Isogeo.AddIn.Models;
using Isogeo.Models;
using Isogeo.Models.Network;
using Isogeo.Models.Filters;
using MVVMPattern.MediatorPattern;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using System.Windows.Input;
using MVVMPattern.RelayCommand;

namespace Isogeo.AddIn.ViewsModels.Search.PrincipalSearch
{
    public class SearchBarViewModel : ViewModelBase
    {
        private readonly RestFunctions _restFunctions;
        private readonly FilterManager _filterManager;

        public string SearchText
        {
            get => Variables.searchText;
            set
            {
                Variables.searchText = value;
                OnPropertyChanged("SearchText");
            }
        }

        private void ChangeSearchTextEvent(object obj)
        {
            SearchText = Variables.searchText;
        }

        public SearchBarViewModel(RestFunctions restFunctions, FilterManager filterManager)
        {
            _restFunctions = restFunctions;
            _filterManager = filterManager;
            Mediator.Register("ChangeQuery", ChangeSearchTextEvent);
        }

        private void SearchTextBox_OnSearch(object sender, RoutedEventArgs e)
        {
            Search();
            //var ob = _filterManager.GetOb();
            //var od = _filterManager.GetOd();
            //var query = _filterManager.GetQueryCombos();
            //var box = _filterManager.GetBoxRequest();
            //_restFunctions.ReloadData(0, query, box, od, ob);
        }

        //private ICommand _searchCommand;
        //public ICommand SearchCommand
        //{
        //    get
        //    {
        //        return _searchCommand ??= new RelayCommand(
        //            x => Search(),
        //            y => CanSearch());
        //    }
        //}

        //public bool CanSearch()
        //{
        //    return !Variables.listLoading;
        //}

        public void Search()
        {
            QueuedTask.Run(() =>
            {
                var ob = _filterManager.GetOb();
                var od = _filterManager.GetOd();
                var query = _filterManager.GetQueryCombos();
                var box = _filterManager.GetBoxRequest();
                _restFunctions.ReloadData(0, query, box, od, ob);
            });
        }
    }
}
