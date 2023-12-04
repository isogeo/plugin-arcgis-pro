using Isogeo.Models.Network.Authentication;
using MVVMPattern;
using System.Windows.Input;
using Isogeo.Network;
using Isogeo.Utils.ConfigurationManager;
using MVVMPattern.RelayCommand;

namespace Isogeo.AddIn.ViewsModels.Settings
{
    public class AuthenticationSettingsViewModel : ViewModelBase
    {
        private readonly INetworkManager _networkManager;
        private IConfigurationManager _configurationManager;

        public AuthenticationSettingsViewModel(INetworkManager networkManager, IConfigurationManager configurationManager)
        {
            _configurationManager = configurationManager;
            _networkManager = networkManager;
        }

        private ICommand _authenticateCommand;

        public ICommand AuthenticateCommand
        {
            get
            {
                return _authenticateCommand ??= new RelayCommand(
                    x => Authenticate(_configurationManager),
                    y => CanAuthenticate());
            }
        }

        private static bool CanAuthenticate()
        {
            return true;
        }

        private void Authenticate(IConfigurationManager configurationManager)
        {
            _configurationManager = configurationManager;
            var frmAuthentication = new Authentication(_networkManager, configurationManager);
            frmAuthentication.ShowDialog();
        }
    }
}
