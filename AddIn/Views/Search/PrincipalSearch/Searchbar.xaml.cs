using Isogeo.AddIn.ViewsModels.Search.PrincipalSearch;
using System.Windows;

namespace Isogeo.AddIn.Views.Search.PrincipalSearch
{
    public partial class SearchBar
    {
        public SearchBar()
        {
            InitializeComponent();
        }

        private async void SearchTextBox_OnSearch(object sender, RoutedEventArgs e)
        {
            // todo Not the best way to do it but didn't find a better solution
            // need to call Search() when Search event is triggered (click on loop, Enter key, history click,..., by user) on controls:SearchTextBox
            // but SearchTextBox from Esri doesn't have a Command (MVVM pattern)
            // so did this : 
            // https://stackoverflow.com/questions/24847062/how-can-i-access-my-viewmodel-from-code-behind
            // why it is not great : "In MVVM you shouldn't be accessing your view model from code behind, the view model and view are ignorant of each other"
            var vm = (SearchBarViewModel)DataContext;
            await vm.Search();
        }
    }
}
