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
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            ArcMapAddinIsogeo.Variables.restFunctions.reloadinfosAPI("", 0, false);
        }

        private void TxtSearch_OnMouseEnter(object sender, MouseEventArgs e)
        {
            ArcMapAddinIsogeo.Variables.restFunctions.reloadinfosAPI("", 0, false);
        }
    }
}
