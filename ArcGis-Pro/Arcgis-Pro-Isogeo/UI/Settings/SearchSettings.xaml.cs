using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using IsogeoLibrary;
using MessageBox = System.Windows.MessageBox;
using UserControl = System.Windows.Controls.UserControl;
using Localization = IsogeoLibrary.Localization;
using Objects = IsogeoLibrary.Objects;

namespace Arcgis_Pro_Isogeo.UI.Settings
{
    /// <summary>
    /// Logique d'interaction pour SearchSettings.xaml
    /// </summary>
    public partial class SearchSettings : UserControl
    {

        Boolean firstLoadOwner = true;
        public SearchSettings()
        {
            InitializeComponent();
            Variables.functionsTranslate.Add(translate);
            Variables.functionsSetlist.Add(setList);
            Variables.CmbLang = CmbLanguage;
        }

        private void translate()
    {
        GrpSearchSettings.Header = Variables.localisationManager.getValue(Localization.LocalizationItem.Search_settings);
        //TODO lbl_defaultsearch.Text = Variables.localisationManager.getValue(Localization.LocalizationItem.Default_search) + " :";
        //TODO lbl_action_type.Text = Variables.localisationManager.getValue(Localization.LocalizationItem.Action_type_default) + " :";
        LblGraphicOperator.Content = Variables.localisationManager.getValue(Localization.LocalizationItem.Geographical_operator_applied_to_the_filter) + " :";
        LblDefaultSorting.Content = Variables.localisationManager.getValue(Localization.LocalizationItem.Default_sort_order) + " :";
        LblLanguage.Content = Variables.localisationManager.getValue(Localization.LocalizationItem.Lang) + " :";
        LblOwnerProperty.Content = Variables.localisationManager.getValue(Localization.LocalizationItem.Metadata_owner_default) + " :";
        BtnSave.Content = Variables.localisationManager.getValue(Localization.LocalizationItem.Save);
        BtnCancel.Content = Variables.localisationManager.getValue(Localization.LocalizationItem.Cancel);
        LblSdeConnectionFile.Content = Variables.localisationManager.getValue(Localization.LocalizationItem.FileSDE);
        setLangList();
        setSortingMethod();
        setSortingDirection();
        // TODO setActionType();
        setGeographicOperator();
    }

    private void setList()
    {
        //Charge une seul fois la liste au chargement de l'application
        if (firstLoadOwner == true)
        {
            firstLoadOwner = false;
            Variables.restFunctions.setListCombo(CmbOwnerProperty, "owner");
        }

    }

    private void setLangList()
    {
        int valcmbIndex = CmbLanguage.SelectedIndex;
        if (valcmbIndex == -1) valcmbIndex = 0;
        List<Objects.comboItem> comboItems = new List<IsogeoLibrary.Objects.comboItem>();
        comboItems.Add(new Objects.comboItem("default", Variables.localisationManager.getValue(Localization.LocalizationItem.Lang_default)));
        comboItems.Add(new Objects.comboItem("en", Variables.localisationManager.getValue(Localization.LocalizationItem.en_EN)));
        comboItems.Add(new Objects.comboItem("fr", Variables.localisationManager.getValue(Localization.LocalizationItem.fr_FR)));
        CmbLanguage.SetBinding(ItemsControl.ItemsSourceProperty, new Binding("SelectedItem") { Source = comboItems }); 
        CmbLanguage.SelectedValuePath = "code";
        CmbLanguage.DisplayMemberPath = "value";
        CmbLanguage.SelectedIndex = valcmbIndex;
    }

    private void setSortingMethod()
    {
        int valcmbIndex = CmbSortingMethod.SelectedIndex;
        if (valcmbIndex == -1) valcmbIndex = 0;
        List<Objects.comboItem> comboItems = new List<Objects.comboItem>();
        comboItems.Add(new Objects.comboItem("relevance", Variables.localisationManager.getValue(Localization.LocalizationItem.Sorting_method_relevance)));
        comboItems.Add(new Objects.comboItem("title", Variables.localisationManager.getValue(Localization.LocalizationItem.Sorting_method_title)));
        comboItems.Add(new Objects.comboItem("modified", Variables.localisationManager.getValue(Localization.LocalizationItem.Sorting_method_modified)));
        comboItems.Add(new Objects.comboItem("created", Variables.localisationManager.getValue(Localization.LocalizationItem.Sorting_method_created)));
        comboItems.Add(new Objects.comboItem("_modified", Variables.localisationManager.getValue(Localization.LocalizationItem.Sorting_method_metadata_modified)));
        comboItems.Add(new Objects.comboItem("_created", Variables.localisationManager.getValue(Localization.LocalizationItem.Sorting_method_metadata_created)));

        CmbSortingMethod.SetBinding(ItemsControl.ItemsSourceProperty, new Binding("SelectedItem") { Source = comboItems });
        CmbSortingMethod.SelectedValuePath = "code";
        CmbSortingMethod.DisplayMemberPath = "value";
        CmbSortingMethod.SelectedIndex = valcmbIndex;
    }

    private void setSortingDirection()
    {
        int valcmbIndex = CmbSortingDirection.SelectedIndex;
        if (valcmbIndex == -1) valcmbIndex = 0;
        List<Objects.comboItem> comboItems = new List<Objects.comboItem>();
        comboItems.Add(new Objects.comboItem("asc", Variables.localisationManager.getValue(Localization.LocalizationItem.Sorting_method_ascending)));
        comboItems.Add(new Objects.comboItem("desc", Variables.localisationManager.getValue(Localization.LocalizationItem.Sorting_method_descending)));

        CmbSortingDirection.SetBinding(ItemsControl.ItemsSourceProperty, new Binding("SelectedItem") { Source = comboItems });
        CmbSortingDirection.SelectedValuePath = "code";
        CmbSortingDirection.DisplayMemberPath = "value";
        CmbSortingDirection.SelectedIndex = valcmbIndex;
        }


    /*TODO private void setActionType()
    {
        int valcmbIndex = cmb_action_type.SelectedIndex;
        if (valcmbIndex == -1) valcmbIndex = 0;
        List<Objects.comboItem> comboItems = new List<Objects.comboItem>();
        comboItems.Add(new Objects.comboItem("view", Variables.localisationManager.getValue(Localization.LocalizationItem.Action_type_view)));
        comboItems.Add(new Objects.comboItem("download", Variables.localisationManager.getValue(Localization.LocalizationItem.Action_type_download)));
        cmb_action_type.DataSource = new BindingSource(comboItems, null);
        cmb_action_type.ValueMember = "code";
        cmb_action_type.DisplayMember = "value";
        cmb_action_type.SelectedIndex = valcmbIndex;
    }*/

    private void setGeographicOperator()
    {
        int valcmbIndex = CmbGraphicOperator.SelectedIndex;
        if (valcmbIndex == -1) valcmbIndex = 0;
        List<Objects.comboItem> comboItems = new List<Objects.comboItem>();
        comboItems.Add(new Objects.comboItem("intersects", Variables.localisationManager.getValue(Localization.LocalizationItem.Geopraphic_type_intersects)));
        comboItems.Add(new Objects.comboItem("within", Variables.localisationManager.getValue(Localization.LocalizationItem.Geopraphic_type_within)));
        comboItems.Add(new Objects.comboItem("contains", Variables.localisationManager.getValue(Localization.LocalizationItem.Geopraphic_type_contains)));

        CmbGraphicOperator.SetBinding(ItemsControl.ItemsSourceProperty, new Binding("SelectedItem") { Source = comboItems });
        CmbGraphicOperator.SelectedValuePath = "code";
        CmbGraphicOperator.DisplayMemberPath = "value";
        CmbGraphicOperator.SelectedIndex = valcmbIndex;
        }

    private void cmb_lang_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Variables.TranslateInProgress == true) return;
        if (Variables.restFunctions != null) Variables.restFunctions.reloadinfosAPI("", 0, false);
        Variables.localisationManager.openLocalizationFile();
        Variables.localisationManager.translatesAll();
    }

    private void btn_save_Click(object sender, EventArgs e)
    {
        //TODO rename Grapic operator by geographic operator
        Variables.configurationManager.config.language = (String)CmbLanguage.SelectedValue;
        //TODO Variables.configurationManager.config.defaultSearch = (String)cmb_default_search.SelectedValue;
        //TODO Variables.configurationManager.config.actionType = (String)cmb_action_type.SelectedValue;
        Variables.configurationManager.config.owner = (String)CmbOwnerProperty.SelectedValue;
        if (Variables.configurationManager.config.owner == "-") Variables.configurationManager.config.owner = "";
        Variables.configurationManager.config.geographicalOperator = (String)CmbGraphicOperator.SelectedValue;
        Variables.configurationManager.config.sortMethode = (String)CmbSortingMethod.SelectedValue;
        Variables.configurationManager.config.sortDirection = (String)CmbSortingDirection.SelectedValue;

        Variables.configurationManager.config.query = " ";
        Variables.configurationManager.config.query += Variables.configurationManager.config.owner;

        Variables.configurationManager.config.fileSDE = TxtSdeConnectionFile.Text;


        Variables.configurationManager.save();

        // TODO MessageBox.Show(Variables.dockableWindowIsogeo, Variables.localisationManager.getValue(Localization.LocalizationItem.Message_Settings_save));

    }

    public void setValues()
    {
        try
        {
            CmbLanguage.SelectedValue = Variables.configurationManager.config.language;
        }
        catch (Exception ex)
        {
            IsogeoLibrary.Utils.Log.DockableWindowLogger.Debug(string.Concat(new object[]
            {
                    "Erreur ",
                    ex.Message
            }));
        }

        //TODO cmb_default_search.SelectedValue = Variables.configurationManager.config.defaultSearch;
        // TODO cmb_action_type.SelectedValue = Variables.configurationManager.config.actionType;
        CmbOwnerProperty.SelectedValue = Variables.configurationManager.config.owner;
        CmbGraphicOperator.SelectedValue = Variables.configurationManager.config.geographicalOperator;
        CmbSortingMethod.SelectedValue = Variables.configurationManager.config.sortMethode;
        CmbSortingDirection.SelectedValue = Variables.configurationManager.config.sortDirection;
        TxtSdeConnectionFile.Text = Variables.configurationManager.config.fileSDE;
    }
    private void btn_cancel_Click(object sender, EventArgs e)
    {
        setValues();
    }

    private void cmb_owner_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    private void cmb_action_type_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    private void cmb_geographic_operator_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    private void cmb_sorting_method_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    private void cmb_sorting_direction_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    // TODO 
    /*private void btn_sde_Click(object sender, EventArgs e)
    {
        // Create an instance of the open file dialog box.
        OpenFileDialog openFileDialog1 = new OpenFileDialog();

        openFileDialog1.FileName = "";
        // Set filter options and filter index.
        openFileDialog1.Filter = "SDE Files (.sde)|*.sde";
        openFileDialog1.FilterIndex = 1;

        openFileDialog1.Multiselect = true;

        // Call the ShowDialog method to show the dialog box.
        System.Windows.Forms.DialogResult dialogResult = openFileDialog1.ShowDialog();

        // Process input if the user clicked OK.
        if (dialogResult == DialogResult.OK)
        {
            txt_sde.Text = openFileDialog1.FileName;
        }*/
    }
}
