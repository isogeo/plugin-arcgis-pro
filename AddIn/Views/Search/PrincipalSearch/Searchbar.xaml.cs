using System.Windows;
using Isogeo.Models;

namespace Isogeo.AddIn.Views.Search.PrincipalSearch
{
    public partial class SearchBar
    {
        public SearchBar()
        {
            InitializeComponent();
        }

        private void SearchTextBox_OnSearch(object sender, RoutedEventArgs e)
        {
            Variables.searchText = SearchTextBox.Text;
            Variables.restFunctions.ReloadData(0);
        }
    }
}
