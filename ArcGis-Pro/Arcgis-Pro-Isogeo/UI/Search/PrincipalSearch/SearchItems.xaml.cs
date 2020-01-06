using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using IsogeoLibrary;

namespace Arcgis_Pro_Isogeo.UI.Search.PrincipalSearch
{
    /// <summary>
    /// Logique d'interaction pour SearchItems.xaml
    /// </summary>
    public partial class SearchItems : UserControl
    {
        public SearchItems()
        {
            InitializeComponent();
            Variables.functionsTranslate.Add(translate);
        }

        private void translate()
        {
            LblSearchTerms.Content = Variables.localisationManager.getValue(IsogeoLibrary.Localization.LocalizationItem.Search_terms) + " :";
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        { 
            Variables.restFunctions.reloadinfosAPI("", 0, false);
        }

        private void TxtSearch_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            { 
                Variables.restFunctions.reloadinfosAPI("", 0, false);
            }
        }
    }
}
