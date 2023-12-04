using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;
using ArcGIS.Desktop.Framework;
using Isogeo.Resources;
using Isogeo.Utils.LogManager;
using Isogeo.Utils.ManageEncrypt;
using Microsoft.Win32;
using MessageBox = ArcGIS.Desktop.Framework.Dialogs.MessageBox;

namespace Isogeo.Models.Network.Authentication
{
    public partial class Authentication
    {

        private void InitResources()
        {
            Dummy.DummyCode();
            var resources = Resources;
            resources.BeginInit();


            if (FrameworkApplication.ApplicationTheme == ApplicationTheme.Dark ||
                FrameworkApplication.ApplicationTheme == ApplicationTheme.HighContrast)
            {
                resources.MergedDictionaries.Add(
                    new ResourceDictionary
                    {
                        Source = new Uri(@"pack://application:,,,/Isogeo.Resources;component/Themes/DarkTheme.xaml")
                    });
            }
            else
            {
                resources.MergedDictionaries.Add(
                    new ResourceDictionary
                    {
                        Source = new Uri(@"pack://application:,,,/Isogeo.Resources;component/Themes/LightTheme.xaml")
                    });
            }
            resources.EndInit();
        }

        internal bool IsInDesignMode =>
            (bool)DependencyPropertyDescriptor.FromProperty(
                DesignerProperties.IsInDesignModeProperty, typeof(DependencyObject)).Metadata.DefaultValue;

        public Authentication()
        {
            InitializeComponent();
            if (!IsInDesignMode)
                InitResources();
            GetAuthentication();
        }

        private void Expander_Expanded(object sender, RoutedEventArgs e)
        {
            if (ExpLogin == sender) 
            {
                if (ExpAltLogin != null) 
                    ExpAltLogin.IsExpanded = false;
            }
            else
            {
                if (ExpLogin != null)
                    ExpLogin.IsExpanded = false;
            }
        }

        private void GetAuthentication()
        {
            try
            {
                TxtId.Text = Variables.configurationManager.config.userAuthentication.id;
                var secretValue = Variables.configurationManager.config.userAuthentication.secret;
                if (!string.IsNullOrWhiteSpace(secretValue))
                {
                    TxtSecret.Password = RijndaelManagedEncryption.DecryptRijndael(secretValue, Variables.encryptCode);
                }

            }
            catch (Exception ex)
            {
                Log.Logger.Error(string.Concat(new object[]
                {
                    "Error Authentication : ",
                    ex.Message
                }));
            }
        }

        private bool CheckValidInputsFormatAuthentication(string id, string password)
        {
            if (string.IsNullOrEmpty(id))
            {
                MessageBox.Show(this, Isogeo.Language.Resources.Message_Query_authentication_id_mandatory, "Isogeo");
                return false;
            }

            if (string.IsNullOrEmpty(password) || password.Length != 64)
            {
                MessageBox.Show(this, Isogeo.Language.Resources.Message_Query_authentication_secret_mandatory, "Isogeo");
                return false;
            }
            return true;
        }

        private async void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (CheckValidInputsFormatAuthentication(TxtId.Text, TxtSecret.Password)) 
                await Authenticate(TxtId.Text, TxtSecret.Password);
        }

        private async Task Authenticate(string username, string password)
        {
            var token = await Variables.restFunctions.SetConnection(username, password);

            if (token?.StatusResult == "OK")
            {
                Variables.configurationManager.config.userAuthentication.id = username;
                try
                {
                    var encryptedstring = RijndaelManagedEncryption.EncryptRijndael(password, Variables.encryptCode);
                    Variables.configurationManager.config.userAuthentication.secret = encryptedstring;
                    Variables.configurationManager.Save();
                    Variables.restFunctions.ResetData();
                    Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    Log.Logger.Error("Error Authenticate - " + ex.Message);
                }
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        public class Web
        {
            [JsonPropertyName("client_id")]
            public string ClientId { get; set; }

            [JsonPropertyName("client_secret")]
            public string ClientSecret { get; set; }
        }

        public class Response
        {
            [JsonPropertyName("web")]
            public Web Web { get; set; }
        }

        private async void BtnSave_Click_From_File(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(TxtPathFile.Text))
            {
                MessageBox.Show(this, Isogeo.Language.Resources.Message_Query_authentication_ko_invalid, "Isogeo");
                return;
            }
            try
            {
                var jsonText = File.ReadAllText(TxtPathFile.Text);
                var response = JsonSerializer.Deserialize<Response>(jsonText);
                if (CheckValidInputsFormatAuthentication(response.Web.ClientId, response.Web.ClientSecret)) 
                    await Authenticate(response.Web.ClientId, response.Web.ClientSecret);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, Isogeo.Language.Resources.Message_Query_authentication_ko_invalid, "Isogeo");
                Log.Logger.Error(string.Concat(new object[]
                {
                    "Error Authentication BtnSave_Click_From_File - ",
                    ex.Message
                }));
            }
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            try
            {
                Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri) { UseShellExecute = true });
                e.Handled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(Isogeo.Language.Resources.Error_Open_External_Tool, "Isogeo");
                Log.Logger.Error(ex.Message);
            }
        }

        private void BtnLoadFile(object sender, RoutedEventArgs e)
        {
            try
            {
                var fileDialog = new OpenFileDialog();
                if (fileDialog.ShowDialog() == true)
                    TxtPathFile.Text = fileDialog.FileName;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, Isogeo.Language.Resources.Message_Query_authentication_ko_invalid, "Isogeo");
                Log.Logger.Error(string.Concat(new object[]
                {
                    "Error BtnLoadFile - ",
                    ex.Message
                }));
            }
        }
    }
}
