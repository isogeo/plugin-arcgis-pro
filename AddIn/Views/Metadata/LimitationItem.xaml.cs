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
            LblLimitation.Text = "Type : " + limitation.Type;
            LblDescription.Text = "Description : " + limitation.Description;
            LblDirectiveName.Text = (limitation.Directive != null && limitation.Directive.Name != null) ? "Directive : " + limitation.Directive.Name : Isogeo.Language.Resources.NotReported;
            LblDirectiveDescription.Text = (limitation.Directive != null && limitation.Directive.Description != null) ? "Restriction : " +limitation.Directive.Description : Isogeo.Language.Resources.NotReported;


            {
                LblDescription.Text += "\n" + limitation.Restriction;
            }
        }
    }
}
