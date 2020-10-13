﻿using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Isogeo.AddIn.Views.Search.Results;
using Isogeo.Models;
using Isogeo.Models.Filters;
using MVVMPattern;
using MVVMPattern.MediatorPattern;
using MVVMPattern.RelayCommand;

namespace Isogeo.AddIn.ViewsModels.Search.Results
{
    public class ResultsViewModel : ViewModelBase
    {

        private ICommand _nextCommand;
        private ICommand _previousCommand;

        public ObservableCollection<ResultItem> ResultsList { get; set; }

        public FilterItem CurrentPage
        {
            get => ListNumberPage.Selected;
            set
            {
                OnPropertyChanged("CurrentPage");
                if (value == null || (ListNumberPage.Selected != null && value.Name == ListNumberPage.Selected.Name))
                    return;
                ListNumberPage.Selected = value;
                SelectionChanged((int.Parse(value.Name) - 1) * Variables.nbResult);
            }
        }

        private static void SelectionChanged(int offset)
        {
            if (Variables.restFunctions != null) 
                Variables.restFunctions.ReloadData(offset);
        }

        private string _lblPage;
        public string LblNbPage
        {
            get => _lblPage;
            set
            {
                _lblPage = value;
                OnPropertyChanged("LblNbPage");
            }
        }

        private bool _lstResultIsEnabled;
        public bool LstResultIsEnabled
        {
            get => _lstResultIsEnabled;
            set
            {
                _lstResultIsEnabled = value;
                OnPropertyChanged("LstResultIsEnabled");
            }

        }

        private FilterItemList _listNumberPage;
        public FilterItemList ListNumberPage
        {
            get => _listNumberPage;
            set
            {
                _listNumberPage = value;
                OnPropertyChanged("ListNumberPage");
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
            OnPropertyChanged("ListNumberPage");
        }

        private void ClearResults()
        {
            ResultsList.Clear();
            ListNumberPage.Items.Clear();
            ListNumberPage.Items.Add(new FilterItem { Name = "1" });
            LblNbPage = "/ 1";
            /*_currentPage*/ ListNumberPage.Selected = ListNumberPage.Items[0];
            OnPropertyChanged("CurrentPage");
            OnPropertyChanged("ListNumberPage");
            OnPropertyChanged("LblNbPage");
            OnPropertyChanged("CurrentPage");
        }

        private void ClearResultsEvent(object obj)
        {
            ClearResults();
        }

        public ResultsViewModel()
        {
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
                    var resultItem = new ResultItem();
                    resultItem.Init(result);
                    ResultsList.Insert(0, resultItem);
                }
            }
            SetPages(offset);
            LstResultIsEnabled = true;
        }

        private void SetCurrentPageWithoutTriggerReloadApi(int offset)
        {
            var index = (offset / Variables.nbResult + 1) - 1;
            if (offset < 0 || Variables.nbResult <= 0 && ListNumberPage.Items.Count <= index) 
                ListNumberPage.Selected = ListNumberPage.Items[0];
            else
                ListNumberPage.Selected = ListNumberPage.Items[index];
            OnPropertyChanged("CurrentPage");
        }

        private void SetPages(int offset)
        {
            var nbPage = GetNbPage();
            ListNumberPage.Items.Clear();
            for (var i = 0; i < nbPage; i++) 
                ListNumberPage.Items.Add(new FilterItem { Name = (i + 1).ToString()});
            LblNbPage = "/ " + nbPage;
            SetCurrentPageWithoutTriggerReloadApi(offset);
        }

        public ICommand NextCommand
        {
            get
            {
                return _nextCommand ?? (_nextCommand = new RelayCommand(
                    x => GoNext(),
                    y => CanGoNext()));
            }
        }

        public ICommand PreviousCommand
        {
            get
            {
                return _previousCommand ?? (_previousCommand = new RelayCommand(
                    x => GoPrevious(),
                    y => CanGoPrevious()));
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
            return ListNumberPage.GetIndex(CurrentPage.Name) < ListNumberPage.Items.Count - 1;
        }

        private bool CanGoPrevious()
        {
            return ListNumberPage.GetIndex(CurrentPage.Name) > 0;
        }
    }
}