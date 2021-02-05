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
            LblLimitation.Text = "Type :" + limitation.type;
            LblDescription.Text = "Description : " + limitation.description;
            LblDirectiveName.Text = (limitation.directive != null && limitation.directive.name != null) ? "Directive : " + limitation.directive.name : Isogeo.Language.Resources.NotReported;
            LblDirectiveDescription.Text = (limitation.directive != null && limitation.directive.description != null) ? limitation.directive.description : Isogeo.Language.Resources.NotReported;


            {
                LblDescription.Text += "\n" + limitation.restriction;
            }
        }
    }
}
