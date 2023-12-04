using Isogeo.Models.Network;
using System.Windows;

namespace Isogeo.AddIn.Views.Settings
{
    public partial class AuthenticationSettings
    {
        private readonly RestFunctions _restFunctions;

        public AuthenticationSettings(RestFunctions restFunctions)
        {
            InitializeComponent();
            _restFunctions = restFunctions;
        }

        private void BtnAuthentication_Click(object sender, RoutedEventArgs e)
        {
            var frmAuthentication = new Models.Network.Authentication.Authentication(_restFunctions);
            frmAuthentication.ShowDialog();
        }
    }
}
