using System.Windows;
using System.Windows.Forms;
using ArcMapAddinIsogeo;
using UserControl = System.Windows.Controls.UserControl;
using UI = Arcgis_Pro_Isogeo.UI;

namespace Arcgis_Pro_Isogeo.UI.Metadata
{
    /// <summary>
    /// Logique d'interaction pour MetadataLicences.xaml
    /// </summary>
    public partial class MetadataLicences : UserControl
    {
        public MetadataLicences()
        {
            InitializeComponent();
        }

        public void setValues()
        {
            GrpLicences.Header = Variables.localisationManager.getValue(ArcMapAddinIsogeo.Localization.LocalizationItem.Metadata_Licences_licence);
            GrpLimitations.Header = Variables.localisationManager.getValue(ArcMapAddinIsogeo.Localization.LocalizationItem.Metadata_Licences_limitation);
            //-- CGUs ------------------------------------------------------------
            //Licences

            if (Variables.currentResult.conditions != null)
            {
                for (int i = Variables.currentResult.conditions.Count - 1; i >= 0; i--)
                {

                    // TODO ADD gestion multiple licences
                    //LicenceItem licenceItem = new LicenceItem(Variables.currentResult.conditions[i]);
                    //licenceItem.Dock = DockStyle.Top;
                    //UI.Search.Results.ResultItemSeparator resultItemSeparator = new UI.Search.Results.ResultItemSeparator();
                    //resultItemSeparator.Dock = DockStyle.Top;
                    this.LicenceItem.Init(Variables.currentResult.conditions[i]);
                    // panel_licences.Controls.Add(resultItemSeparator);
                    // panel_licences.Controls.Add(licenceItem);
                }
            }


            //# Limitations

            if (Variables.currentResult.limitations != null)
            {
                for (int i = Variables.currentResult.limitations.Count - 1; i >= 0; i--)
                {
                    // TODO add gestion multiple limitations
                    this.LimitationItem.Init(Variables.currentResult.limitations[i]);
                    //LimitationItem limitationItem = new LimitationItem(Variables.currentResult.limitations[i]);
                    // limitationItem.Dock = DockStyle.Top;
                    //UI.Search.Results.ResultItemSeparator resultItemSeparator = new UI.Search.Results.ResultItemSeparator();
                    // resultItemSeparator.Dock = DockStyle.Top;
                    // panel_limitations.Controls.Add(resultItemSeparator);
                    // panel_limitations.Controls.Add(limitationItem);
                }
            }

        }
    }
}
