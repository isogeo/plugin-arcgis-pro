using System;
using System.Diagnostics;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using ArcGIS.Desktop.Framework.Dialogs;
using Isogeo.Utils.LogManager;

namespace Isogeo.AddIn.Views.Settings
{
    public partial class Credits
    {
        public Credits()
        {
            InitializeComponent();
            Init();
        }

        private static BitmapImage ReturnPicture(string imagePath)
        {
            return new BitmapImage(new Uri(imagePath, UriKind.Absolute));
        }

        public void Init()
        {
            ImgConception.Source = ReturnPicture(
                "pack://application:,,,/Isogeo.Resources;component/Resources/lgpl-logo.png");
            ImgGithub.Source = ReturnPicture(
                "pack://application:,,,/Isogeo.Resources;component/Resources/github-logo.png");
            ImgLinkedin.Source = ReturnPicture(
                "pack://application:,,,/Isogeo.Resources;component/Resources/Linkedin-logo.png");
        }

        private void GithubMouseUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                Process.Start(new ProcessStartInfo("https://github.com/VianneyDoleans") { UseShellExecute = true });
                e.Handled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(Isogeo.Language.Resources.Error_Open_External_Tool, "Isogeo");
                Log.Logger.Error(ex.Message);
            }
        }

        private void LinkedinMouseUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                Process.Start(new ProcessStartInfo("https://www.linkedin.com/in/vianneydoleans") { UseShellExecute = true });
                e.Handled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(Isogeo.Language.Resources.Error_Open_External_Tool, "Isogeo");
                Log.Logger.Error(ex.Message);
            }
        }
    }
}
