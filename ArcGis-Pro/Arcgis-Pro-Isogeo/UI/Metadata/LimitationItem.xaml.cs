using System.Windows.Controls;
using API = IsogeoLibrary.API;

namespace Arcgis_Pro_Isogeo.UI.Metadata
{
    /// <summary>
    /// Logique d'interaction pour LimitationItem.xaml
    /// </summary>
    public partial class LimitationItem : UserControl
    {
        public LimitationItem()
        {
            InitializeComponent();
        }

        public void Init(API.Limitation limitation)
        {
            LblLimitation.Content = limitation.type;
            LblDescription.Content = limitation.description;
            if (limitation.type == "legal")
            {
                LblDescription.Content += "\n" + limitation.restriction;
            }
        }
    }
}
