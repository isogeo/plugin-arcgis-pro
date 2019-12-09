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
using System.Windows.Navigation;
using System.Windows.Shapes;
using ArcMapAddinIsogeo;
using API = ArcMapAddinIsogeo.API;

namespace Arcgis_Pro_Isogeo.UI.Metadata
{
    /// <summary>
    /// Logique d'interaction pour MetadataGeography.xaml
    /// </summary>
    public partial class MetadataGeography : UserControl
    {
        public MetadataGeography()
        {
            InitializeComponent();
        }

        public void setValues()
        {
            GrpTechnicalInformation.Header =
                Variables.localisationManager.getValue(ArcMapAddinIsogeo.Localization.LocalizationItem.Metadata_Geography_info);
            GrpSpecification.Header =
                Variables.localisationManager.getValue(ArcMapAddinIsogeo.Localization.LocalizationItem.Metadata_Geography_specification);
            GrpTypology.Content =
                Variables.localisationManager.getValue(ArcMapAddinIsogeo.Localization.LocalizationItem.Metadata_Geography_typology);
            TechnicalInformationItem.LblFormat.Content =
                Variables.localisationManager.getValue(ArcMapAddinIsogeo.Localization.LocalizationItem.Metadata_Geography_format);
            TechnicalInformationItem.LblFeatCount.Content =
                Variables.localisationManager.getValue(ArcMapAddinIsogeo.Localization.LocalizationItem.Metadata_Geography_nbobjects);
            TechnicalInformationItem.LblGeometryType.Content =
                Variables.localisationManager.getValue(ArcMapAddinIsogeo.Localization.LocalizationItem.Metadata_Geography_geometrytype);
            TechnicalInformationItem.LblSrs.Content = Variables.localisationManager.getValue(ArcMapAddinIsogeo.Localization.LocalizationItem.Metadata_Geography_srs);
            TechnicalInformationItem.LblScale.Content =
                Variables.localisationManager.getValue(ArcMapAddinIsogeo.Localization.LocalizationItem.Metadata_Geography_scale);
            TechnicalInformationItem.LblResolution.Content =
                Variables.localisationManager.getValue(ArcMapAddinIsogeo.Localization.LocalizationItem.Metadata_Geography_resolution);


            LblTypology.Content = "";
            //-- TECHNICAL -----------------------------------------------------
            // SRS
            TechnicalInformationItem.LblSrsValue.Content = "";
            if (Variables.currentResult.coordinate_system != null)
            {
                TechnicalInformationItem.LblSrsValue.Content = Variables.currentResult.coordinate_system.name + " (EPSG:" +
                               Variables.currentResult.coordinate_system.code + ")";
            }

            //Set the data format
            TechnicalInformationItem.LblFormat.Content = "";
            if (Variables.currentResult.tagsLists != null)
            {
                if (Variables.currentResult.tagsLists.formats.Count > 0)
                {
                    TechnicalInformationItem.LblFormat.Content = Variables.currentResult.tagsLists.formats[0];
                }
            }


            //feature info
            TechnicalInformationItem.LblFeatCountValue.Content = "";
            if (Variables.currentResult.features != null)
            {
                TechnicalInformationItem.LblFeatCountValue.Content = Variables.currentResult.features.ToString();
            }

            if (Variables.currentResult.geometry != null)
                TechnicalInformationItem.LblGeometryTypeValue.Content = Variables.currentResult.geometry;

            TechnicalInformationItem.LblResolutionValue.Content = "";
            if (Variables.currentResult.distance != null)
            {
                TechnicalInformationItem.LblResolutionValue.Content = Variables.currentResult.distance + " m";
            }

            if (Variables.currentResult.scale != null) TechnicalInformationItem.LblScaleValue.Content = Variables.currentResult.scale.ToString();


            //Quality
            if (Variables.currentResult.topologicalConsistency != null)
                LblTypology.Content = Variables.currentResult.topologicalConsistency;
            //Specifications

            LblSpecification.Content = "";
            if (Variables.currentResult.specifications != null)
            {
                foreach (API.Specification specification in Variables.currentResult.specifications)
                {
                    String conform = "Conforme";

                    if (specification.conformant == false)
                    {
                        conform = "Non conforme";
                    }

                    String dateSpec = formatDate(specification.specification.published);
                    if (LblSpecification.Content.ToString() != "") LblSpecification.Content += "/n";
                    LblSpecification.Content += specification.specification.name + "(" + dateSpec + ") : " + conform;
                }
            }



            //Geography
            if (Variables.currentResult.envelope != null)
            {
                ////display
                //self.complete_md.wid_bbox.setDisabled(0)
                ////get convex hull coordinates and create the polygon
                //md_lyr = self.envelope2layer(md.get("envelope"))
                ////add layers
                //QgsMapLayerRegistry.instance().addMapLayers([md_lyr,
                //                                             self.world_lyr],
                //                                            0)
                //map_canvas_layer_list = [QgsMapCanvasLayer(md_lyr),
                //                         QgsMapCanvasLayer(self.world_lyr)]
                //self.complete_md.wid_bbox.setLayerSet(map_canvas_layer_list)
                //self.complete_md.wid_bbox.setExtent(md_lyr.extent())
            }
            else
            {
                //self.complete_md.wid_bbox.setExtent(self.world_lyr.extent())
                //self.complete_md.grp_bbox.setDisabled(1)
            }

        }


        private String formatDate(String dateStr)
        {
            if (dateStr == null) return "";
            return dateStr.Split('T')[0];

        }
    }
}
