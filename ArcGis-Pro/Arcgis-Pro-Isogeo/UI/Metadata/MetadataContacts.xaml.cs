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
using ArcMapAddinIsogeo;
using UserControl = System.Windows.Controls.UserControl;
using API = ArcMapAddinIsogeo.API;
using UI = Arcgis_Pro_Isogeo.UI;

namespace Arcgis_Pro_Isogeo.UI.Metadata
{
    /// <summary>
    /// Logique d'interaction pour MetadataContacts.xaml
    /// </summary>
    public partial class MetadataContacts : UserControl
    {
        public MetadataContacts()
        {
            InitializeComponent();
        }

        public void setValues()
        {
            GrpContactPoint.Header = Variables.localisationManager.getValue(ArcMapAddinIsogeo.Localization.LocalizationItem.Metadata_Contacts_points_contacts);
            GrpOthersContact.Header = Variables.localisationManager.getValue(ArcMapAddinIsogeo.Localization.LocalizationItem.Metadata_Contacts_others_contacts);

            // TODO : error here, can have multiple users, not only one, so have to make a "add" fonction, not "init"
            for (int i = Variables.currentResult.contacts.Count - 1; i >= 0; i--)
            {
                API.Contact contact = Variables.currentResult.contacts[i].contact;
                //ContactItem contactItem = new ContactItem(contact);
                //contactItem.Dock = DockStyle.Top;
                //UI.Search.Results.ResultItemSeparator resultItemSeparator = new UI.Search.Results.ResultItemSeparator();
                //resultItemSeparator.Dock = DockStyle.Top;
                if (Variables.currentResult.contacts[i].role == "pointOfContact")
                {
                    // TODO error here
                    this.ContactItemPoint.Init(contact);
                    // panel_contact.Controls.Add(resultItemSeparator);
                }
                else
                {
                    // TODO error here
                    this.ContactItemPoint.Init(contact);
                    //panel_others_contact.Controls.Add(resultItemSeparator);
                    //panel_others_contact.Controls.Add(contactItem);
                }

            }
        }
}
