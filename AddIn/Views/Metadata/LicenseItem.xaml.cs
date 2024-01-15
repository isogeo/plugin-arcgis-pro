using Condition = Isogeo.Models.API.Condition;

namespace Isogeo.AddIn.Views.Metadata
{
    public partial class LicenseItem
    {
        public LicenseItem()
        {
            InitializeComponent();
        }

        public void Init(Condition conditionInput)
        {
            LblLicense.Text = "";
            LblContent.Text = "";

            if (conditionInput.License != null)
            {
                LblLicense.Text = conditionInput.License.Name;
                LblContent.Text = conditionInput.License.Link;
            }
            LblDescription.Text = conditionInput.Description;
        }
    }
}
