using System.Windows;

namespace Isogeo.AddIn.Views.Settings
{
    public partial class AuthenticationSettings
    {
        public AuthenticationSettings()
        {
            InitializeComponent();
        }

        private void BtnAuthentication_Click(object sender, RoutedEventArgs e)
        {
            var frmAuthentication = new Models.Network.Authentication.Authentication();
            frmAuthentication.ShowDialog();
        }
    }
}
