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
        private Boolean isCmbLoad = false;

        public Results()
        {
            InitializeComponent();
        }

        public void setData()
        {
            //if (LstResults.Items.Count > 0) 
            //    LstResults.Items.Clear();
            LstResults.IsEnabled = false;
            var resultsList = new List<ResultItem>();
            for (int i = Variables.search.results.Count - 1; i >= 0; i--)
            {
                API.Result result = Variables.search.results[i];
                ResultItem resultItem = new ResultItem();
                resultItem.Init(result);
                // resultsList.Add(resultItem);
                resultsList.Insert(0, resultItem);
                //ResultItemSeparator resultItemSeparator = new ResultItemSeparator();
            }
            LstResults.SetBinding(ItemsControl.ItemsSourceProperty, new Binding { Source = resultsList });
            LstResults.IsEnabled = true;
        }

        public void setCombo(int nbPage)
        {
            isCmbLoad = true;
            CmbNoPage.Items.Clear();
            for (int i = 0; i < nbPage; i++)
            {
                CmbNoPage.Items.Add(i + 1);
            }
            CmbNoPage.SelectedIndex = Variables.currentPage - 1;
            LblNbPage.Content = "/" + nbPage;
            isCmbLoad = false;
        }

        public void clearPages()
        {
            var resultsList = new List<ResultItem>();
            LstResults.SetBinding(ItemsControl.ItemsSourceProperty, new Binding { Source = resultsList });
            LstResults.IsEnabled = true;
            //LstResults.Items.Clear();
            CmbNoPage.Items.Clear();
            LblNbPage.Content = "";
        }

        private void BtnPrevious_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (Variables.currentPage != 1)
            {
                Variables.currentPage += -1;
            }
            Variables.restFunctions.reloadinfosAPI("", Variables.currentPage, true);
        }

        private void BtnNext_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (Variables.currentPage != CmbNoPage.Items.Count)
            {
                Variables.currentPage += 1;
            }
            Variables.restFunctions.reloadinfosAPI("", Variables.currentPage, true);
        }

        private void CmbNoPage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Variables.currentPage = CmbNoPage.SelectedIndex + 1;
            BtnPrevious.IsEnabled = true;
            BtnNext.IsEnabled = true;
            if (Variables.currentPage == 1) BtnPrevious.IsEnabled = false;
            if (Variables.currentPage == CmbNoPage.Items.Count) BtnNext.IsEnabled = false;

            if (isCmbLoad == true) return;

            Variables.restFunctions.reloadinfosAPI("", Variables.currentPage, true);
        }
    }
}
