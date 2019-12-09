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
using API = ArcMapAddinIsogeo.API;

namespace Arcgis_Pro_Isogeo.UI
{
    /// <summary>
    /// Logique d'interaction pour ContactItem.xaml
    /// </summary>
    public partial class ContactItem : UserControl
    {
        public ContactItem()
        {
            InitializeComponent();
        }

        public void Init(API.Contact contact)
        {
            LblName.Content = contact.name;

            if (contact.organization != null)
            {
                if (contact.organization != "")
                {
                    LblName.Content += " ( " + contact.organization + ")";
                }
            }
            LblEmail.Content = contact.email;
            LblPhone.Content = contact.phone;
            LblAddress.Content = contact.addressLine1 + " " + contact.addressLine2;
            LblCity.Content = contact.zipCode + " " + contact.city + " " + contact.country;
        }

        private void LblEmail_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (LblEmail.Content != "")
            {
                string mailto =
                    string.Format(
                        "mailto:{0}?Subject={1}&Body={2}",
                        LblEmail.Content, "Isogeo", "");
                System.Diagnostics.Process.Start(mailto);
            }
        }
    }
}
