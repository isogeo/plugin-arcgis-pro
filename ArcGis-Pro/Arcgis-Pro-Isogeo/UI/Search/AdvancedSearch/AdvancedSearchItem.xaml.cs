using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using ArcMapAddinIsogeo;
using Binding = System.Windows.Data.Binding;
using  Objects = ArcMapAddinIsogeo.Objects;
using UserControl = System.Windows.Controls.UserControl;

namespace Arcgis_Pro_Isogeo.UI.Search.AdvancedSearch
{
    /// <summary>
    /// Logique d'interaction pour AdvancedSearchItem.xaml
    /// </summary>
    public partial class AdvancedSearchItem : UserControl
    {
        private String translateName;
        private String listName;

        public AdvancedSearchItem()
        {
            InitializeComponent();
        }

        public void Init(String searchName, String imageSearchPath, String listName)
        {
            LblFilterName.Content = searchName;
            // TODO set picture
            ImgAdvancedSearch.Source = new BitmapImage(new Uri(imageSearchPath, UriKind.Absolute)); //imageSearchPath;
            translateName = searchName;
            this.listName = listName;
            Variables.functionsTranslate.Add(translate);
            if (listName != "geographicFilter")
            {

                Variables.functionsSetlist.Add(setList);
                Variables.listComboFilter.Add(this.CmbAdvancedSearchFilter);
            }
            else
            {
                Variables.advancedSearchItem_geographicFilter = this;
            }
        }

        private void translate()
        {
            this.LblFilterName.Content = Variables.localisationManager.getValue(translateName) + " :";
            if (listName == "geographicFilter")
            {
                setGeographicOperator();
            }

        }

        private void setList()
        {
            Variables.restFunctions.setListCombo(CmbAdvancedSearchFilter, listName);
        }

        public void setGeographicOperator()
        {
            Objects.comboItem valcmbValue = null;
            if (CmbAdvancedSearchFilter.SelectedIndex > -1)
            {
                valcmbValue = (Objects.comboItem) CmbAdvancedSearchFilter.SelectedItem;
            }


            List<Objects.comboItem> comboItems = new List<Objects.comboItem>();
            comboItems.Add(new Objects.comboItem("-", "-"));
            comboItems.Add(new Objects.comboItem("mapcanvas",
                Variables.localisationManager.getValue(ArcMapAddinIsogeo.Localization.LocalizationItem.Map_canvas)));

            CmbAdvancedSearchFilter.SetBinding(ItemsControl.ItemsSourceProperty, new Binding("SelectedItem") {  Source = comboItems });
            CmbAdvancedSearchFilter.SelectedValuePath = "code";
            CmbAdvancedSearchFilter.DisplayMemberPath = "value";
            if (valcmbValue != null)
            {
                try
                {
                    CmbAdvancedSearchFilter.SelectedItem = valcmbValue;
                }
                catch
                {
                }
            }
            if (CmbAdvancedSearchFilter.SelectedIndex == -1)
            {
                CmbAdvancedSearchFilter.SelectedIndex = 0;
            }
        }

        private void CmbAdvancedSearchFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Variables.ListLoading == true) return;
            if (Variables.restFunctions != null) Variables.restFunctions.reloadinfosAPI("", 0, false);
        }
    }
}
