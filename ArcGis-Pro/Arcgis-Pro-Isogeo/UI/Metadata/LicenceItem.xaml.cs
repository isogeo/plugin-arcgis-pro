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
using  API = IsogeoLibrary.API;

namespace Arcgis_Pro_Isogeo.UI.Metadata
{
    /// <summary>
    /// Logique d'interaction pour LicenceItem.xaml
    /// </summary>
    public partial class LicenceItem : UserControl
    {
        private API.Condition condition;

        public LicenceItem()
        {
            InitializeComponent();
        }

        public void Init(API.Condition condition)
        {
            LblLicence.Content = "";
            LblContent.Content = "";

            this.condition = condition;
            if (condition.licence != null)
            {
                LblLicence.Content = condition.licence.name;
                LblContent.Content = condition.licence.content;
            }
            else
            {
                LblLicence.Content = "Pas de licence";
            }
            LblDescription.Content = condition.description;
        }
    }
}
