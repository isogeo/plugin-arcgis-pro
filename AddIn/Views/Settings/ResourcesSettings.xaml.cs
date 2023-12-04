using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using Isogeo.Utils.ConfigurationManager;
using Isogeo.Utils.LogManager;

namespace Isogeo.AddIn.Views.Settings
{
    public partial class ResourcesSettings : UserControl
    {
        private readonly IConfigurationManager _configurationManager;

        public ResourcesSettings(IConfigurationManager configurationManager)
        {
            InitializeComponent();
            _configurationManager = configurationManager;
            TbLogPath.Text = Log.GetLogFilePath();
        }

        private void BtnHelp_Click(object sender, RoutedEventArgs e)
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

        private void BtnCredits_Click(object sender, RoutedEventArgs e)
        {
            var frmCredits = new Credits();
            frmCredits.Show();
        }

        private void BtnContactSupport_Click(object sender, RoutedEventArgs e)
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

        private void BtnLog_Click(object sender, RoutedEventArgs e)
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
