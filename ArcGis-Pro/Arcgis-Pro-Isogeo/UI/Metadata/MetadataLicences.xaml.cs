using System.Collections.Generic;
using System.Windows;
using System.Windows.Forms;
using IsogeoLibrary;
using UserControl = System.Windows.Controls.UserControl;
using UI = Arcgis_Pro_Isogeo.UI;

namespace Arcgis_Pro_Isogeo.UI.Metadata
{
    /// <summary>
    /// Logique d'interaction pour MetadataLicences.xaml
    /// </summary>
    public partial class MetadataLicences : UserControl
    {
        private List<LicenceItem> _licenceItemsList;
        private List<LimitationItem> _limitationItemsList;

        public MetadataLicences()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            _licenceItemsList = new List<LicenceItem>();
            _limitationItemsList = new List<LimitationItem>();
            LvwLicenceItems.ItemsSource = _licenceItemsList;
            LvwLimitationItems.ItemsSource = _limitationItemsList;
        }

        public void setValues()
        {
            GrpLicences.Header = Variables.localisationManager.getValue(IsogeoLibrary.Localization.LocalizationItem.Metadata_Licences_licence);
            GrpLimitations.Header = Variables.localisationManager.getValue(IsogeoLibrary.Localization.LocalizationItem.Metadata_Licences_limitation);
            //-- CGUs ------------------------------------------------------------
            //Licences

            if (Variables.currentResult.conditions != null)
            {
                for (int i = Variables.currentResult.conditions.Count - 1; i >= 0; i--)
                {

                    LicenceItem licenceItem = new LicenceItem();
                    licenceItem.Init(Variables.currentResult.conditions[i]);
                    _licenceItemsList.Add(licenceItem);
                    // panel_licences.Controls.Add(resultItemSeparator);
                }
            }


            //# Limitations

            if (Variables.currentResult.limitations != null)
            {
                for (int i = Variables.currentResult.limitations.Count - 1; i >= 0; i--)
                {
                    LimitationItem limitationItem = new LimitationItem();
                    limitationItem.Init(Variables.currentResult.limitations[i]);
                    _limitationItemsList.Add(limitationItem);
                    // panel_limitations.Controls.Add(resultItemSeparator);
                }
            }

        }
    }
}
