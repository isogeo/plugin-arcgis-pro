using Condition = Isogeo.Models.API.Condition;

namespace Isogeo.AddIn.Views.Metadata
{
    public partial class LicenseItem
    {
        private Condition condition;

        public LicenseItem()
        {
            InitializeComponent();
        }

        public void Init(Condition conditionInput)
        {
            LblLicense.Text = "";
            LblContent.Text = "";

            condition = conditionInput;
            if (conditionInput.license != null)
            {
                LblLicense.Text = conditionInput.license.name;
                LblContent.Text = conditionInput.license.link;
            }
            /*else
            {
                LblLicense.Text = Isogeo.Language.Resources.No_licence;
            }*/
            LblDescription.Text = conditionInput.description;
        }
    }
}
