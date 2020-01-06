using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using IsogeoLibrary;
using UserControl = System.Windows.Controls.UserControl;
using API = IsogeoLibrary.API;
using UI = Arcgis_Pro_Isogeo.UI;

namespace Arcgis_Pro_Isogeo.UI.Metadata
{
    /// <summary>
    /// Logique d'interaction pour MetadataContacts.xaml
    /// </summary>
    public partial class MetadataContacts : UserControl
    {
        private List<ContactItem> _contactItemsList;
        private List<ContactItem> _otherContactItemsList;

        public MetadataContacts()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            _contactItemsList = new List<ContactItem>();
            _otherContactItemsList = new List<ContactItem>();
            LvwContactItems.ItemsSource = _contactItemsList;
            LvwOtherContactItems.ItemsSource = _otherContactItemsList;
        }

        public void setValues()
        {
            GrpContactPoint.Header = Variables.localisationManager.getValue(IsogeoLibrary.Localization
                .LocalizationItem.Metadata_Contacts_points_contacts);
            GrpOthersContact.Header = Variables.localisationManager.getValue(IsogeoLibrary.Localization
                .LocalizationItem.Metadata_Contacts_others_contacts);

            for (int i = Variables.currentResult.contacts.Count - 1; i >= 0; i--)
            {
                API.Contact contact = Variables.currentResult.contacts[i].contact;
                ContactItem contactItem = new ContactItem();
                contactItem.Init(contact);
                if (Variables.currentResult.contacts[i].role == "pointOfContact")
                {
                    _contactItemsList.Add(contactItem);
                    // panel_contact.Controls.Add(resultItemSeparator);
                }
                else
                {
                    _otherContactItemsList.Add(contactItem);
                    //panel_others_contact.Controls.Add(resultItemSeparator);
                }

            }
        }
    }
}
