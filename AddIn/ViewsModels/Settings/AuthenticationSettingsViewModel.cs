using Isogeo.Models.Network.Authentication;
using MVVMPattern;
using System.Windows.Input;
using Isogeo.Network;
using MVVMPattern.RelayCommand;

namespace Isogeo.AddIn.ViewsModels.Settings
{
    public class AuthenticationSettingsViewModel : ViewModelBase
    {
        private readonly INetworkManager _networkManager;

        public AuthenticationSettingsViewModel(INetworkManager networkManager)
        {
            _networkManager = networkManager;
        }

        private ICommand _authenticateCommand;

        public ICommand AuthenticateCommand
        {
            get
            {
                return _authenticateCommand ??= new RelayCommand(
                    x => Authenticate(),
                    y => CanAuthenticate());
            }
        }

        private static bool CanAuthenticate()
        {
            return true;
        }

        private void Authenticate()
        {
            var frmAuthentication = new Authentication(_networkManager);
            frmAuthentication.ShowDialog();
        }
    }
}
