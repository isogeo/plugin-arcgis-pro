using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Isogeo.AddIn.Models;
using Isogeo.AddIn.Models.Filters.Components;
using Isogeo.AddIn.Views.Search.Results;
using Isogeo.Map.MapFunctions;
using Isogeo.Models;
using Isogeo.Models.Network;
using MVVMPattern;
using MVVMPattern.MediatorPattern;
using MVVMPattern.RelayCommand;

namespace Isogeo.AddIn.ViewsModels.Search.Results
{
    public class ResultsViewModel : ViewModelBase
    {

        private ICommand _nextCommand;
        private ICommand _previousCommand;
        private readonly IMapFunctions _mapFunctions;
        private readonly RestFunctions _restFunctions;
        private readonly FilterManager _filterManager;

        public ObservableCollection<ResultItem> ResultsList { get; set; }

        private ResultItem _selectedItem;
        public ResultItem SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                OnPropertyChanged(nameof(SelectedItem));
            }
        }

        public FilterItem CurrentPage
        {
            get => ListNumberPage.Selected;
            set
            {
                if (value == null || (ListNumberPage.Selected != null && value.Name == ListNumberPage.Selected.Name))
                    return;
                _listNumberPage.SetSelected(value);
                OnPropertyChanged(nameof(CurrentPage));
                Task.Run(() => Application.Current.Dispatcher.Invoke(async () => await SelectionChanged((int.Parse(value.Name) - 1) * Variables.nbResult)));
            }
        }

        private async Task SelectionChanged(int offset)
        {
            var ob = _filterManager.GetOb();
            var od = _filterManager.GetOd();
            var query = _filterManager.GetQueryCombos();
            var box = _filterManager.GetBoxRequest();

            await _restFunctions.ReloadData(offset, query, box, od, ob);
            _filterManager.SetSearchList(query);
        }

        private string _lblPage;
        public string LblNbPage
        {
            get => _lblPage;
            set
            {
                _lblPage = value;
                OnPropertyChanged(nameof(LblNbPage));
            }
        }

        private bool _lstResultIsEnabled;
        public bool LstResultIsEnabled
        {
            get => _lstResultIsEnabled;
            set
            {
                _lstResultIsEnabled = value;
                OnPropertyChanged(nameof(LstResultIsEnabled));
            }

        }

        private FilterItemList _listNumberPage;
        public FilterItemList ListNumberPage
        {
            get => _listNumberPage;
            set
            {
                _listNumberPage = value;
                OnPropertyChanged(nameof(ListNumberPage));
            }
        }

        private static int GetNbPage()
        {
            var nbPage = 1;
            if (Variables.search != null && !Variables.search.total.Equals(0))
                nbPage = Convert.ToInt32(Math.Ceiling(Variables.search.total / Variables.nbResult));
            return nbPage;
        }

        private void ChangePageEvent(object offset)
        {
            Refresh((int)offset);
        }

        private void Filter_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(nameof(ListNumberPage));
        }

        private void ClearResults()
        {
            ResultsList.Clear();
            ListNumberPage.Items.Clear();
            ListNumberPage.Items.Add(new FilterItem("1", "1" ));
            LblNbPage = "/ 1";
            ListNumberPage.SetSelected(ListNumberPage.Items[0]);
            OnPropertyChanged(nameof(CurrentPage));
            OnPropertyChanged(nameof(ListNumberPage));
            OnPropertyChanged(nameof(LblNbPage));
            OnPropertyChanged(nameof(CurrentPage));
        }

        private void ClearResultsEvent(object obj)
        {
            ClearResults();
        }

        public ResultsViewModel(IMapFunctions mapFunctions, RestFunctions restFunctions, FilterManager filterManager)
        {
            _mapFunctions = mapFunctions;
            _restFunctions = restFunctions;
            _filterManager = filterManager;
            ResultsList = new ObservableCollection<ResultItem>();
            ListNumberPage = new FilterItemList();
            ListNumberPage.PropertyChanged += Filter_PropertyChanged;
            Refresh(0);
            Mediator.Register("ChangeOffset", ChangePageEvent);
            Mediator.Register("ClearResults", ClearResultsEvent);
        }

        public void Refresh(int offset)
        {
            LstResultIsEnabled = false;
            ResultsList.Clear();
            if (Variables.search != null && Variables.search.results != null)
            {
                for (var i = Variables.search.results.Count - 1; i >= 0; i--)
                {
                    var result = Variables.search.results[i];
                    var resultItem = new ResultItem(_mapFunctions, _restFunctions);
                    resultItem.Init(result);
                    ResultsList.Insert(0, resultItem);
                }
            }
            SetPages(offset);
            LstResultIsEnabled = true;
            if (ResultsList.Count > 0) 
                SelectedItem = ResultsList[0];
        }

        private void SetCurrentPageWithoutTriggerReloadApi(int offset)
        {
            var index = (offset / Variables.nbResult + 1) - 1;
            if (offset < 0 || Variables.nbResult <= 0 && ListNumberPage.Items.Count <= index) 
                ListNumberPage.SetSelected(ListNumberPage.Items[0]);
            else
                ListNumberPage.SetSelected(ListNumberPage.Items[index]);
            OnPropertyChanged(nameof(CurrentPage));
        }

        private void SetPages(int offset)
        {
            var nbPage = GetNbPage();
            ListNumberPage.Items.Clear();
            for (var i = 0; i < nbPage; i++) 
                ListNumberPage.Items.Add(new FilterItem((i + 1).ToString(), (i + 1).ToString()));
            LblNbPage = "/ " + nbPage;
            SetCurrentPageWithoutTriggerReloadApi(offset);
        }

        public ICommand NextCommand
        {
            get
            {
                return _nextCommand ??= new RelayCommand(
                    x => GoNext(),
                    y => CanGoNext());
            }
        }

        public ICommand PreviousCommand
        {
            get
            {
                return _previousCommand ??= new RelayCommand(
                    x => GoPrevious(),
                    y => CanGoPrevious());
            }
        }

        private void GoNext()
        {
            CurrentPage = ListNumberPage.Items[ListNumberPage.GetIndex(CurrentPage.Name) + 1];
        }

        private void GoPrevious()
        {
            CurrentPage = ListNumberPage.Items[ListNumberPage.GetIndex(CurrentPage.Name) - 1];
        }

        private bool CanGoNext()
        {
            if (CurrentPage?.Name == null)
                return false;
            return ListNumberPage.GetIndex(CurrentPage.Name) < ListNumberPage.Items.Count - 1;
        }

        private bool CanGoPrevious()
        {
            if (CurrentPage?.Name == null)
                return false;
            return ListNumberPage.GetIndex(CurrentPage.Name) > 0;
        }
    }
}
