using System;
using System.Windows;
using System.Windows.Controls;
using ArcMapAddinIsogeo;

namespace Arcgis_Pro_Isogeo.UI.Search.PrincipalSearch
{
    /// <summary>
    /// Logique d'interaction pour Keywords.xaml
    /// </summary>
    public partial class Keywords : UserControl
    {
        public Keywords()
        {
            InitializeComponent();
            Variables.functionsTranslate.Add(translate);
            Variables.functionsSetlist.Add(setList);
            Variables.listComboFilter.Add(CmbKeywords);
        }

        private void translate()
        {
            LblKeywords.Content = Variables.localisationManager.getValue(
                                      ArcMapAddinIsogeo.Localization.LocalizationItem.Keywords) + " :";
        }

        private void setList()
        {
            Variables.restFunctions.setListCombo(CmbKeywords, "keyword:isogeo");
        }

        private void CmbKeywords_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Variables.ListLoading) return;
            if (Variables.restFunctions != null) Variables.restFunctions.reloadinfosAPI("", 0, false);
        }
    }
}
