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
            if (conditionInput.licence != null)
            {
                LblLicense.Text = conditionInput.licence.name;
                LblContent.Text = conditionInput.licence.content;
            }
            /*else
            {
                LblLicense.Text = Isogeo.Language.Resources.No_licence;
            }*/
            LblDescription.Text = conditionInput.description;
        }
    }
}
