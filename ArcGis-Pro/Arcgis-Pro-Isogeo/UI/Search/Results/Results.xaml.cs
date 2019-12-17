using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Data;
using ArcMapAddinIsogeo;
using UserControl = System.Windows.Controls.UserControl;
using API = ArcMapAddinIsogeo.API;

namespace Arcgis_Pro_Isogeo.UI.Search.Results
{
    /// <summary>
    /// Logique d'interaction pour Results.xaml
    /// </summary>
    public partial class Results : UserControl
    {
        private int _currentPage;
        public int CurrentPage
        {
            get => _currentPage;
            set => _currentPage = value;
        }

        private Boolean isCmbLoad = false;
        private int nbPage = 1;
        private List<int> _listNumberPage;

        public Results()
        {
            InitializeComponent();
            CurrentPage = 1;
            _listNumberPage = new List<int>();
            Refresh(0);
        }

        public void Refresh(int offset)
        {
            LstResults.IsEnabled = false;
            var resultsList = new List<ResultItem>();
            if (Variables.search != null && Variables.search.results != null)
            {
                // TODO dont know why variable.searc.result.count and not variables.nbResult
                for (int i = Variables.search.results.Count - 1; i >= 0; i--)
                {
                    API.Result result = Variables.search.results[i];
                    ResultItem resultItem = new ResultItem();
                    resultItem.Init(result);
                    resultsList.Insert(0, resultItem);
                } 
                LstResults.SetBinding(ItemsControl.ItemsSourceProperty, new Binding { Source = resultsList });
            }
            setCombo(offset);
            LstResults.IsEnabled = true;
        }


        private void setCombo(int offset)
        {
            if (Variables.search != null && Variables.search.total != 0) 
                nbPage = Convert.ToInt32(Math.Ceiling(Variables.search.total / Variables.nbResult));
            else
                nbPage = 1;
            isCmbLoad = true;
            _listNumberPage.Clear();
            for (int i = 0; i < nbPage; i++)
            {
                _listNumberPage.Add(i + 1);
            }
            CmbNoPage.SetBinding(ItemsControl.ItemsSourceProperty, new Binding { Source = _listNumberPage });
            CmbNoPage.SelectedIndex = CurrentPage - 1;
            LblNbPage.Content = "/" + nbPage;
            isCmbLoad = false;
            CheckButtonEnabled();
        }

        private void BtnPrevious_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (CurrentPage != 1)
            {
                CurrentPage += -1;
            }
            if (CurrentPage > nbPage) CurrentPage = nbPage;
            int offset = (Variables.dockableWindowIsogeo.Results.CurrentPage - 1) *
                          Variables.nbResult;
            Variables.restFunctions.reloadinfosAPI("", offset, true);
        }

        void CheckButtonEnabled()
        {
            BtnPrevious.IsEnabled = true;
            BtnNext.IsEnabled = true;
            if (CurrentPage == 1) BtnPrevious.IsEnabled = false;
            if (CurrentPage == _listNumberPage.Count) BtnNext.IsEnabled = false;
        }

        private void BtnNext_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (CurrentPage != _listNumberPage.Count)
            {
                CurrentPage += 1;
            }
            if (CurrentPage > nbPage) CurrentPage = nbPage;
            int offset = (Variables.dockableWindowIsogeo.Results.CurrentPage - 1) *
                          Variables.nbResult;
            Variables.restFunctions.reloadinfosAPI("", offset, true);
        }

        private void CmbNoPage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CurrentPage = CmbNoPage.SelectedIndex + 1;

            if (isCmbLoad == true) return;

            int offset = (Variables.dockableWindowIsogeo.Results.CurrentPage - 1) *
                         Variables.nbResult;
            Variables.restFunctions.reloadinfosAPI("", offset, true);
        }
    }
}
