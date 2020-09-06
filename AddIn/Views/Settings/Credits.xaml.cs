using System;
using System.Diagnostics;
using System.Windows.Input;
using System.Windows.Media.Imaging;

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
            Process.Start(new ProcessStartInfo("https://github.com/VianneyDoleans"));
            e.Handled = true;
        }

        private void LinkedinMouseUp(object sender, MouseButtonEventArgs e)
        {
            Process.Start(new ProcessStartInfo("https://www.linkedin.com/in/vianneydoleans"));
            e.Handled = true;
        }
    }
}
