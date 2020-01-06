using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using IsogeoLibrary;
using ToolTip = System.Windows.Controls.ToolTip;
using UserControl = System.Windows.Controls.UserControl;

namespace Arcgis_Pro_Isogeo.UI.Search.Results
{
    /// <summary>
    /// Logique d'interaction pour ResultsToolBar.xaml
    /// </summary>
    public partial class ResultsToolBar : UserControl
    {
        private Boolean isUpdateCombo = false;

        public ResultsToolBar()
        {
            InitializeComponent();
            Variables.functionsTranslate.Add(translate);
        }

        private void translate()
        {
            ToolTip toolTipSortingMethod = new ToolTip();
            toolTipSortingMethod.Content = Variables.localisationManager.getValue(IsogeoLibrary.Localization.LocalizationItem.Sorting_method);
            CmbSortingMethod.ToolTip = toolTipSortingMethod;

            ToolTip toolTipSortingDirection = new ToolTip();
            toolTipSortingDirection.Content = Variables.localisationManager.getValue(IsogeoLibrary.Localization.LocalizationItem.Sorting_direction);
            CmbSortingDirection.ToolTip = toolTipSortingDirection;

            ToolTip toolTipLastSearh = new ToolTip();
            toolTipLastSearh.Content = Variables.localisationManager.getValue(IsogeoLibrary.Localization.LocalizationItem.Reset_search);
            BtnLastSearch.ToolTip = toolTipLastSearh;

            setNbResults();
            setSortingMethod();
            setSortingDirection();

        }

        public void setSortingDefault()
        {
            isUpdateCombo = true;
            CmbSortingMethod.SelectedValue = Variables.configurationManager.config.sortMethode;
            CmbSortingDirection.SelectedValue = Variables.configurationManager.config.sortDirection;
            isUpdateCombo = false;
        }


        private void setSortingMethod()
        {
            isUpdateCombo = true;
            int valcmbIndex = CmbSortingMethod.SelectedIndex;
            if (valcmbIndex == -1) valcmbIndex = 0;

            CmbiRelevance.Content = Variables.localisationManager.getValue(IsogeoLibrary.Localization.LocalizationItem.Sorting_method_relevance);

            CmbiTitle.Content = Variables.localisationManager.getValue(IsogeoLibrary.Localization.LocalizationItem.Sorting_method_title);
            CmbiModified.Content = Variables.localisationManager.getValue(IsogeoLibrary.Localization.LocalizationItem.Sorting_method_modified);
            CmbiCreated.Content = Variables.localisationManager.getValue(IsogeoLibrary.Localization.LocalizationItem.Sorting_method_created);
            Cmbi_Created.Content = Variables.localisationManager.getValue(IsogeoLibrary.Localization.LocalizationItem.Sorting_method_metadata_modified);
            Cmbi_Created.Content = Variables.localisationManager.getValue(IsogeoLibrary.Localization.LocalizationItem.Sorting_method_metadata_created);

            CmbSortingMethod.SelectedIndex = valcmbIndex;
            isUpdateCombo = false;
        }

        private void setSortingDirection()
        {
            isUpdateCombo = true;
            int valcmbIndex = CmbSortingDirection.SelectedIndex;
            if (valcmbIndex == -1) valcmbIndex = 0;

            CmbiAsc.Content = Variables.localisationManager.getValue(IsogeoLibrary.Localization.LocalizationItem
                .Sorting_method_ascending);
            CmbiDesc.Content = Variables.localisationManager.getValue(IsogeoLibrary.Localization.LocalizationItem.Sorting_method_descending);

            CmbSortingDirection.SelectedIndex = valcmbIndex;
            isUpdateCombo = false;
        }

        public void setNbResults()
        {
            if (Variables.search != null)
            {
                BtnResults.Content = Variables.localisationManager.getValue(IsogeoLibrary.Localization.LocalizationItem.Results) + " (" + Variables.search.total + ")";
                if (Variables.search.total == 0)
                {
                    BtnResults.IsEnabled = false;
                }
                else
                {
                    BtnResults.IsEnabled = true;
                }

            }
            else
            {
                BtnResults.Content = Variables.localisationManager.getValue(IsogeoLibrary.Localization.LocalizationItem.Results) + " (0)";
                BtnResults.IsEnabled = false;
            }

        }


        private void CmbSortingMethod_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (isUpdateCombo == false)
            {
                Variables.restFunctions.reloadinfosAPI("", 0, true);
            }
        }

        private void CmbSortingDirection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (isUpdateCombo == false)
            {
                Variables.restFunctions.reloadinfosAPI("", 0, true);
            }
        }

        private void BtnResults_Click(object sender, RoutedEventArgs e)
        {
            Variables.restFunctions.reloadinfosAPI("", 0, true);
        }

        private void BtnLastSearch_Click(object sender, RoutedEventArgs e)
        {
            Variables.dockableWindowIsogeo.MenuSearchManagment.tsmi_new_Click(null, null);
        }
    }
}
