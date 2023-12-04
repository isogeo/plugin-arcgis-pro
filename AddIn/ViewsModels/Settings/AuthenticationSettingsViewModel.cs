using Isogeo.Models.Network.Authentication;
using Isogeo.Models.Network;
using MVVMPattern;
using System.Windows.Input;
using MVVMPattern.RelayCommand;

namespace Isogeo.AddIn.ViewsModels.Settings
{
    public class AuthenticationSettingsViewModel : ViewModelBase
    {
        private readonly RestFunctions _restFunctions;

        public AuthenticationSettingsViewModel(RestFunctions restFunctions)
        {
            _restFunctions = restFunctions;
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
            var frmAuthentication = new Authentication(_restFunctions);
            frmAuthentication.ShowDialog();
        }
    }
}
