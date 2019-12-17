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
using System.Windows.Shapes;

namespace Arcgis_Pro_Isogeo.UI.Metadata
{
    /// <summary>
    /// Logique d'interaction pour Metadata.xaml
    /// </summary>
    public partial class Metadata : Window
    {
        public Metadata()
        {
            InitializeComponent();
            this.TabsMetadata.MetadataLicences.setValues();
            this.TabsMetadata.MetadataAdvanced.setValues();
            this.TabsMetadata.MetadataGeneral.setValues();
            this.TabsMetadata.MetadataHistory.setValues();
            this.TabsMetadata.MetadataGeography.setValues();
            this.TabsMetadata.MetadataContacts.setValues();
        }
    }
}
