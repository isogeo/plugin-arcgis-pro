using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Isogeo.Models;
using Isogeo.Utils.ConfigurationManager;
using Isogeo.Utils.LogManager;
using Isogeo.Utils.ManageEncrypt;
using MVVMPattern;
using MVVMPattern.RelayCommand;

namespace Isogeo.AddIn.ViewsModels.Settings
{
    public class ProxySettingsViewModel : ViewModelBase
    {

        private readonly IConfigurationManager _configurationManager;

        public ProxySettingsViewModel(IConfigurationManager configurationManager)
        {
            _configurationManager = configurationManager;
        }

        private string _proxyUrl;
        public string ProxyUrl
        {
            get => _proxyUrl;
            set
            {
                _proxyUrl = value;
                OnPropertyChanged(nameof(ProxyUrl));
            }
        }

        private string _user;
        public string User
        {
            get => _user;
            set
            {
                _user = value;
                OnPropertyChanged(nameof(User));
            }
        }

        private ICommand _saveCommand;
        private ICommand _cancelCommand;

        public ICommand CancelCommand
        {
            get
            {
                return _cancelCommand ??= new RelayCommand(
                    x => Cancel(),
                    y => CanCancel());
            }
        }

        public ICommand SaveCommand
        {
            get
            {
                return _saveCommand ??= new RelayCommand(
                    x => Save(x),
                    y => CanSave());
            }
        }

        private static bool CanSave()
        {
            return true;
        }

        private void Save(object parameter)
        {
            _configurationManager.Config.Proxy.ProxyUrl = ProxyUrl;
            _configurationManager.Config.Proxy.ProxyUser = User;
            _configurationManager.Config.Proxy.ProxyPassword = "";

            var password = ((PasswordBox) parameter).Password;
            try
            {
                if (password != "")
                {
                    var encryptedString = RijndaelManagedEncryption.EncryptRijndael(password, Variables.EncryptCode);
                    _configurationManager.Config.Proxy.ProxyPassword = encryptedString;
                }
                _configurationManager.Save();
                MessageBox.Show(Language.Resources.Proxy_saved);
            }
            catch (Exception ex)
            {
                Log.Logger.Info(string.Concat(new object[]
                {
                    "Error proxySettings - saveClick -",
                    ex.Message
                }));
                MessageBox.Show(Language.Resources.Error_proxy);
            }
        }

        private void Cancel()
        {// todo
            ProxyUrl = _configurationManager.Config.Proxy.ProxyUrl;
            User = _configurationManager.Config.Proxy.ProxyUser;
        }

        private bool CanCancel()
        {
            return true;
        }
    }
}
