using System;
using System.Windows;
using System.Windows.Controls;
using Isogeo.Models;
using Isogeo.Utils.LogManager;

namespace Isogeo.AddIn.Views.Settings
{
    public partial class ResourcesSettings : UserControl
    {
        public ResourcesSettings()
        {
            InitializeComponent();
            TbLogPath.Text = Log.GetLogFilePath();
        }

        private void BtnHelp_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(Variables.configurationManager.config.urlHelp);
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
                    $"mailto:{Variables.configurationManager.config.emailSupport}?Subject={Variables.configurationManager.config.emailSubject}&Body={Variables.configurationManager.config.emailBody.Replace("/n", "%0D%0A")}";
                System.Diagnostics.Process.Start(mailto);
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
                System.Diagnostics.Process.Start(Log.GetLogFilePath());
            }
            catch (Exception ex)
            {
                MessageBox.Show(Isogeo.Language.Resources.Error_logs, "Isogeo");
                Log.Logger.Error(ex.Message);
            }
        }
    }
}
