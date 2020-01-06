using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using IsogeoLibrary;

namespace Arcgis_Pro_Isogeo.UI.Search.SearchManagment
{

    /// <summary>
    /// Logique d'interaction pour AddSearchManagment.xaml
    /// </summary>
    public partial class AddSearchManagment : Window
    {
        public Boolean isRename = false;
        public Boolean isSave = false;
        public String oldName;

        public AddSearchManagment()
        {
            InitializeComponent();
        }

        public void Init(Boolean isRename, String oldName)
        {
            this.isRename = isRename;
            this.oldName = oldName;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            isSave = false;
            this.Close();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (TxtQuickSearch.Text == "")
            {
                MessageBox.Show(this, Variables.localisationManager.getValue(IsogeoLibrary.Localization.LocalizationItem.Quicksearch_name_mandatory), this.Title);
                return;
            }


            foreach (IsogeoLibrary.Configuration.Search search in Variables.configurationManager.config.searchs.searchs)
            {
                if (search.name == TxtQuickSearch.Text && search.name != oldName)
                {
                    MessageBox.Show(this,
                        Variables.localisationManager.getValue(IsogeoLibrary.Localization.LocalizationItem.Quicksearch_already_exist), this.Title);
                    return;
                }
            }

            isSave = true;
            this.Close();
        }

        private void TxtQuickSearch_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                BtnSave_Click(null, null);
            }
        }
    }
}
