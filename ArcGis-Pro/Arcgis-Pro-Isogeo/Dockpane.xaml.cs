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
using Arcgis_Pro_Isogeo.UI.Metadata;
using Arcgis_Pro_Isogeo.UI.Search.Results;


namespace Arcgis_Pro_Isogeo
{
    /// <summary>
    /// Interaction logic for DockpaneView.xaml
    /// </summary>
    public partial class DockpaneView : UserControl
    {
        public DockpaneView()
        {
            InitializeComponent();
            Metadata metadata = new Metadata();
            metadata.Show();
        }
    }
}
