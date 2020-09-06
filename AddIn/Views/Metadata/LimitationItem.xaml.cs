using System.Windows.Controls;
using Isogeo.Models.API;

namespace Isogeo.AddIn.Views.Metadata
{
    public partial class LimitationItem : UserControl
    {
        public LimitationItem()
        {
            InitializeComponent();
        }

        public void Init(Limitation limitation)
        {
            LblLimitation.Text = limitation.type;
            LblDescription.Text = limitation.description;
            if (limitation.type == "legal")
            {
                LblDescription.Text += "\n" + limitation.restriction;
            }
        }
    }
}
