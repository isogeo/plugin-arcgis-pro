using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ActiproSoftware.Windows.Extensions;
using Isogeo.AddIn.Models.FilterManager;
using Isogeo.AddIn.Models.Filters.Components;
using Isogeo.Map;
using Isogeo.Models;
using Isogeo.Network;
using Isogeo.Utils.ConfigurationManager;
using MVVMPattern;
using MVVMPattern.MediatorPattern;
using MVVMPattern.RelayCommand;

namespace Isogeo.AddIn.ViewsModels.Search.Results
{
    public class ResultsViewModel : ViewModelBase
    {

        private ICommand _nextCommand;
        private ICommand _previousCommand;
        private readonly IMapManager _mapManager;
        private readonly INetworkManager _networkManager;
        private readonly IFilterManager _filterManager;
        private readonly IConfigurationManager _configurationManager;

        public ObservableCollection<ResultItemViewModel> ResultsList { get; set; }

        public ResultsViewModel(IMapManager mapManager, INetworkManager networkManager, IFilterManager filterManager,
            IConfigurationManager configurationManager)
        {
            _configurationManager = configurationManager;
            _mapManager = mapManager;
            _networkManager = networkManager;
            _filterManager = filterManager;
            ResultsList = new ObservableCollection<ResultItemViewModel>();
            ListNumberPage = new FilterItemList();
            ListNumberPage.PropertyChanged += Filter_PropertyChanged;
            Refresh(0);
            Mediator.Register(MediatorEvent.ChangeOffset, ChangePageEvent);
            Mediator.Register(MediatorEvent.ClearResults, ClearResultsEvent);
        }

        private ResultItemViewModel _selectedItem;
        public ResultItemViewModel SelectedItem
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
                _listNumberPage.Selected = value;
                OnPropertyChanged(nameof(CurrentPage));
                Task.Run(() => Application.Current.Dispatcher.Invoke(async () => 
                await SelectionChanged((int.Parse(value.Name) - 1) * _configurationManager.GlobalSoftwareSettings.NbResult)));
            }
        }

        private async Task SelectionChanged(int offset)
        {
            var ob = _filterManager.GetOb();
            var od = _filterManager.GetOd();
            var query = _filterManager.GetQueryCombos();
            var box = _filterManager.GetBoxRequest();

            await _networkManager.LoadData(query, offset, box, od, ob);
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

        private int GetNbPage()
        {
            var nbPage = 1;
            if (Variables.search != null && !Variables.search.Total.Equals(0))
                nbPage = Convert.ToInt32(Math.Ceiling(Variables.search.Total /
                                                      _configurationManager.GlobalSoftwareSettings.NbResult));
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
            ListNumberPage.Selected = ListNumberPage.Items[0];
            OnPropertyChanged(nameof(CurrentPage));
            OnPropertyChanged(nameof(ListNumberPage));
            OnPropertyChanged(nameof(LblNbPage));
            OnPropertyChanged(nameof(CurrentPage));
        }

        private void ClearResultsEvent(object obj)
        {
            ClearResults();
        }

        public void Refresh(int offset)
        {
            var temporaryItems = new List<ResultItemViewModel>();

            LstResultIsEnabled = false;
            if (Variables.search != null && Variables.search.Results != null)
            {
                for (var i = Variables.search.Results.Count - 1; i >= 0; i--)
                {
                    var result = Variables.search.Results[i];
                    var resultItem = new ResultItemViewModel(_mapManager, _networkManager, _configurationManager);
                    resultItem.Init(result);
                    temporaryItems.Add(resultItem);
                }
            }
            ResultsList.Clear();
            ResultsList.AddRange(temporaryItems);
            SetPages(offset);
            LstResultIsEnabled = true;
            if (ResultsList.Count > 0) 
                SelectedItem = ResultsList[0];
        }

        private void SetCurrentPageWithoutTriggerReloadApi(int offset)
        {
            var index = (offset / _configurationManager.GlobalSoftwareSettings.NbResult + 1) - 1;
            if (offset < 0 || _configurationManager.GlobalSoftwareSettings.NbResult <= 0 && ListNumberPage.Items.Count <= index) 
                ListNumberPage.Selected = ListNumberPage.Items[0];
            else
                ListNumberPage.Selected = ListNumberPage.Items[index];
            OnPropertyChanged(nameof(CurrentPage));
        }

        private void SetPages(int offset)
        {
            var temporaryList = new List<FilterItem>();

            var nbPage = GetNbPage();
            for (var i = 0; i < nbPage; i++)
                temporaryList.Add(new FilterItem((i + 1).ToString(), (i + 1).ToString()));
            ListNumberPage.Items.Clear();
            ListNumberPage.Items.AddRange(temporaryList);
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
