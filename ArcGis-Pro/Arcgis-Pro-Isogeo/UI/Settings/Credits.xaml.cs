using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using IsogeoLibrary;

namespace Arcgis_Pro_Isogeo.UI.Settings
{
    /// <summary>
    /// Logique d'interaction pour Credits.xaml
    /// </summary>
    public partial class Credits : Window
    {
        private List<string> _contributorsList;

        public Credits()
        {
            InitializeComponent();
            Init();
            translate();
        }

        private BitmapImage ReturnPicture(String imagePath)
        {
            return new BitmapImage(new Uri(imagePath, UriKind.Absolute));
        }

        public void Init()
        {
            _contributorsList = new List<string>();
            LstDevelopedBy.ItemsSource = _contributorsList;
            _contributorsList.Add("Vianney Doleans - Developer");
            ImgConception.Source = ReturnPicture(
                "pack://application:,,,/" + Variables._assemblyName + ";component/Images/btnIsogeo.png");
            ImgContent.Source = ReturnPicture(
                "pack://application:,,,/" + Variables._assemblyName + ";component/Images/btnIsogeo.png");
        }

        private void translate()
        {
            this.Title = Variables.localisationManager.getValue(IsogeoLibrary.Localization.LocalizationItem.Credit_title);
            GrpContent.Header = Variables.localisationManager.getValue(IsogeoLibrary.Localization.LocalizationItem.Credit_contenu);
            GrpConception.Header = Variables.localisationManager.getValue(IsogeoLibrary.Localization.LocalizationItem.Credit_conception);
            GrpDevelopment.Header = Variables.localisationManager.getValue(IsogeoLibrary.Localization.LocalizationItem.Credit_development);
            LblContent.Content = Variables.localisationManager.getValue(IsogeoLibrary.Localization.LocalizationItem.Credit_contenu_label);
            LblIsogeo.Content = Variables.localisationManager.getValue(IsogeoLibrary.Localization.LocalizationItem.Credit_isogeo);
            LblDevelopedBy.Content = Variables.localisationManager.getValue(IsogeoLibrary.Localization.LocalizationItem.Credit_developedby);
        }
    }
}
