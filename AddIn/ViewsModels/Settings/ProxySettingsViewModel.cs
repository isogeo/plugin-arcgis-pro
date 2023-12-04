using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Isogeo.Models;
using Isogeo.Utils.LogManager;
using Isogeo.Utils.ManageEncrypt;
using MVVMPattern;
using MVVMPattern.RelayCommand;

namespace Isogeo.AddIn.ViewsModels.Settings
{
    public class ProxySettingsViewModel : ViewModelBase
    {

        private string _proxyUrl;
        public string ProxyUrl
        {
            get => _proxyUrl;
            set
            {
                _proxyUrl = value;
                OnPropertyChanged("ProxyUrl");
            }
        }

        private string _user;
        public string User
        {
            get => _user;
            set
            {
                _user = value;
                OnPropertyChanged("User");
            }
        }

        private ICommand _saveCommand;
        private ICommand _cancelCommand;

        public ICommand CancelCommand
        {
            get
            {
                return _cancelCommand ?? (_cancelCommand = new RelayCommand(
                    x => Cancel(),
                    y => CanCancel()));
            }
        }

        public ICommand SaveCommand
        {
            get
            {
                return _saveCommand ?? (_saveCommand = new RelayCommand(
                    x => Save(x),
                    y => CanSave()));
            }
        }

        private static bool CanSave()
        {
            return true;
        }

        private void Save(object parameter)
        {
            Variables.configurationManager.config.proxy.proxyUrl = ProxyUrl;
            Variables.configurationManager.config.proxy.proxyUser = User;
            Variables.configurationManager.config.proxy.proxyPassword = "";

            var password = ((PasswordBox) parameter).Password;
            try
            {
                if (password != "")
                {
                    var encryptedString = RijndaelManagedEncryption.EncryptRijndael(password, Variables.encryptCode);
                    Variables.configurationManager.config.proxy.proxyPassword = encryptedString;
                }
                Variables.configurationManager.Save();
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
            ProxyUrl = Variables.configurationManager.config.proxy.proxyUrl;
            User = Variables.configurationManager.config.proxy.proxyUser;
        }

        private bool CanCancel()
        {
            return true;
        }
    }
}
