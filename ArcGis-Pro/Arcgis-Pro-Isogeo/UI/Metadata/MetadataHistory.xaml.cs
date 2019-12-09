using System;
using System.Collections.Generic;
using System.Windows.Data;
using System.Windows.Controls;
using ArcMapAddinIsogeo;
using UserControl = System.Windows.Controls.UserControl;
using API = ArcMapAddinIsogeo.API;

namespace Arcgis_Pro_Isogeo.UI.Metadata
{
    /// <summary>
    /// Logique d'interaction pour MetadataHistory.xaml
    /// </summary>
    public partial class MetadataHistory : UserControl
    {
        public MetadataHistory()
        {
            InitializeComponent();
        }

        public void setValues()
        {
            GrpDataHistory.Header = Variables.localisationManager.getValue(ArcMapAddinIsogeo.Localization.LocalizationItem.Metadata_History_history);
            GrpCollectContext.Header = Variables.localisationManager.getValue(ArcMapAddinIsogeo.Localization.LocalizationItem.Metadata_History_context);
            GrpCollectMethod.Header = Variables.localisationManager.getValue(ArcMapAddinIsogeo.Localization.LocalizationItem.Metadata_History_method);
            GrpLastModification.Header = Variables.localisationManager.getValue(ArcMapAddinIsogeo.Localization.LocalizationItem.Metadata_History_lastupdatedata);

            HistoryDataItem.LblDataCreation.Content = Variables.localisationManager.getValue(ArcMapAddinIsogeo.Localization.LocalizationItem.Metadata_History_datecreate);
            HistoryDataItem.LblDataUpdate.Content = Variables.localisationManager.getValue(ArcMapAddinIsogeo.Localization.LocalizationItem.Metadata_History_lastupdate);
            HistoryDataItem.LblFrequency.Content = Variables.localisationManager.getValue(ArcMapAddinIsogeo.Localization.LocalizationItem.Metadata_History_updateevery);
            HistoryDataItem.LblValidStart.Content = Variables.localisationManager.getValue(ArcMapAddinIsogeo.Localization.LocalizationItem.Metadata_History_startvalid);
            HistoryDataItem.LblValidEnd.Content = Variables.localisationManager.getValue(ArcMapAddinIsogeo.Localization.LocalizationItem.Metadata_History_endvalid);
            HistoryDataItem.LblValidComment.Content = Variables.localisationManager.getValue(ArcMapAddinIsogeo.Localization.LocalizationItem.Metadata_History_comment);


            //HISTORY ---------------------------------------------------------
            //Data creation and last update dates
            HistoryDataItem.LblDataCreation.Content = formatDate(Variables.currentResult._created);
            HistoryDataItem.LblDataUpdate.Content = formatDate(Variables.currentResult._modified);
            //custom_tools.handle_date
            //Update frequency information
            if (Variables.currentResult.updateFrequency != null)
            {
                //freq = md.get("updateFrequency")
                //frequency_info = "{}{} {}"\
                //                 .format(isogeo_tr.tr(None, "frequencyUpdateHelp"),
                //                         ''.join(i for i in freq if i.isdigit()),
                //                         isogeo_tr.tr("frequencyShortTypes",
                //                                      freq[-1]))
                //self.complete_md.val_frequency.setText(frequency_info)
                HistoryDataItem.LblFrequencyValue.Content = "?";
            }
            else
            { 
                HistoryDataItem.LblFrequencyValue.Content = "NR";
            }

            //Validity
            HistoryDataItem.LblValidStartValue.Content = formatDate(Variables.currentResult.validFrom);
            HistoryDataItem.LblValidEndValue.Content = formatDate(Variables.currentResult.validTo);
            HistoryDataItem.LblValidCommentValue.Content = formatDate(Variables.currentResult.validityComment);

            //Collect information
            LblMethod.Content = Variables.currentResult.collectionMethod;
            LblContext.Content = Variables.currentResult.collectionContext;

            if (Variables.currentResult.events != null)
            {
                var events = new List<API.Event>();

                foreach (API.Event eventObj in Variables.currentResult.events)
                {
                    if (eventObj.kind == "update")
                    {
                        // TODO this is not the good way to do, better to bind a list to the DataGrid, and specify in parameter which methods are the good values
                        events.Add(eventObj);
//                        LastModificationMetaDataItem.DgLastModifications.Items.Add( DataItem())
                        //DataGridRow row = new DataGridRow();
                        //row.C CreateCells(LastModificationMetaDataItem.DgLastModifications);
                        //row.Cells[0].Value = eventObj.date;
                        //row.Cells[1].Value = eventObj.description;
                        //tbl_events.Rows.Add(row);
                    }
                }
                LastModificationMetaDataItem.DgLastModifications.Items.Add(events);
            }
        }

        private String formatDate(String dateStr)
        {
            if (dateStr == null) return "";
            return dateStr.Split('T')[0];

        }
    }
}
