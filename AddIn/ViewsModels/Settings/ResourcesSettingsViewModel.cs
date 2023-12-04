using Isogeo.AddIn.Views.Settings;
using Isogeo.Utils.ConfigurationManager;
using Isogeo.Utils.LogManager;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using MVVMPattern.RelayCommand;

namespace Isogeo.AddIn.ViewsModels.Settings
{
    public class ResourcesSettingsViewModel
    {
        private readonly IConfigurationManager _configurationManager;

        public ResourcesSettingsViewModel(IConfigurationManager configurationManager)
        {
            _configurationManager = configurationManager;
        }

        private ICommand _btnHelpCommand;
        public ICommand BtnHelpCommand
        {
            get
            {
                return _btnHelpCommand ??= new RelayCommand(
                    x => BtnHelp_Click(),
                    y => true);
            }
        }

        private void BtnHelp_Click()
        {
            try
            {
                Process.Start(new ProcessStartInfo(_configurationManager.Config.UrlHelp) { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                MessageBox.Show(Isogeo.Language.Resources.Error_Open_External_Tool, "Isogeo");
                Log.Logger.Error(ex.Message);
            }
        }

        private ICommand _btnCreditsCommand;
        public ICommand BtnCreditsCommand
        {
            get
            {
                return _btnCreditsCommand ??= new RelayCommand(
                    x => BtnCredits_Click(),
                    y => true);
            }
        }
        private static void BtnCredits_Click()
        {
            var frmCredits = new Credits();
            frmCredits.Show();
        }

        private ICommand _btnContactSupportCommand;
        public ICommand BtnContactSupportCommand
        {
            get
            {
                return _btnContactSupportCommand ??= new RelayCommand(
                    x => BtnContactSupport_Click(),
                    y => true);
            }
        }

        private void BtnContactSupport_Click()
        {
            try
            {
                var mailto =
                    $"mailto:{_configurationManager.Config.EmailSupport}?Subject={_configurationManager.Config.EmailSubject}&Body={_configurationManager.Config.EmailBody.Replace("/n", "%0D%0A")}";
                Process.Start(new ProcessStartInfo(mailto) { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                MessageBox.Show(Isogeo.Language.Resources.Error_contact_support, "Isogeo");
                Log.Logger.Error(ex.Message);
            }
        }

        private ICommand _btnLogCommand;
        public ICommand BtnLogCommand
        {
            get
            {
                return _btnLogCommand ??= new RelayCommand(
                    x => BtnLog_Click(),
                    y => true);
            }
        }

        private void BtnLog_Click()
        {
            try
            {
                Process.Start(new ProcessStartInfo(Log.GetLogFilePath()) { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                MessageBox.Show(Isogeo.Language.Resources.Error_logs, "Isogeo");
                Log.Logger.Error(ex.Message);
            }
        }
    }
}
