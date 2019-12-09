using System;
using System.Collections.Generic;
using System.Windows.Controls;
using ArcMapAddinIsogeo;

namespace Arcgis_Pro_Isogeo.UI.Search.SearchManagment
{
    /// <summary>
    /// Logique d'interaction pour MenuSearchManagment.xaml
    /// </summary>
    public partial class MenuSearchManagment : UserControl
    {
        private String searchSelectedName = "";

        private List<MenuItem> lstTSMISearch = new List<MenuItem>();

        public MenuSearchManagment()
        {
            InitializeComponent();
            Variables.functionsTranslate.Add(translate);
        }

        private void translate()
        {
            this.MniSearchManagement.Header = Variables.localisationManager.getValue(ArcMapAddinIsogeo.Localization.LocalizationItem.Search_management);
            this.MniNewSearch.Header = Variables.localisationManager.getValue(ArcMapAddinIsogeo.Localization.LocalizationItem.New_search);
            this.MniPrecedentSearch.Header = Variables.localisationManager.getValue(ArcMapAddinIsogeo.Localization.LocalizationItem.Previous_search);
            //selectedSavedChange();

        }

        public void setValues()
        {
            int i = 0;
            foreach (ArcMapAddinIsogeo.Configuration.Search search in Variables.configurationManager.config.searchs.searchs)
            {
                if (search.name != "CurrentSearchSave")
                {
                    //MenuItem newToolStripMenuItem = new MenuItem(search.name, null, tsmi_saved_click);
                    //lstTSMISearch.Add(newToolStripMenuItem);
                    //tsmi_search_management.DropDownItems.Insert(i, newToolStripMenuItem);
                    i = i + 1;
                }
            }
            //lstSavedChange();

        }

        /*private void lstSavedChange()
        {
            if (lstTSMISearch.Count > 0)
            {
                tsmi_lst_save_separator.Visible = true;
            }
            else
            {
                tsmi_lst_save_separator.Visible = false;
            }
        }*/

        /*private void selectedSavedChange()
        {

            foreach (ToolStripMenuItem tsmi in lstTSMISearch)
            {
                if (tsmi.Text == searchSelectedName)
                {
                    tsmi.Checked = true;
                }
                else
                {
                    tsmi.Checked = false;
                }
            }

            if (searchSelectedName != "")
            {
                //tsmi_save.Visible = true;
                //tsmi_delete.Visible = true;
                //tsmi_rename.Visible = true;
                //tsmi_save.Text = Variables.localisationManager.getValue(ArcMapAddinIsogeo.Localization.LocalizationItem.Save) + " '" + searchSelectedName + "'";
                //tsmi_delete.Text = Variables.localisationManager.getValue(ArcMapAddinIsogeo.Localization.LocalizationItem.Delete) + " '" + searchSelectedName + "'";
                //tsmi_rename.Text = Variables.localisationManager.getValue(ArcMapAddinIsogeo.Localization.LocalizationItem.Rename) + " '" + searchSelectedName + "'";
                //tsmi_saveas.Text = Variables.localisationManager.getValue(ArcMapAddinIsogeo.Localization.LocalizationItem.Save) + " '" + searchSelectedName + "'" + Variables.localisationManager.getValue(Localization.LocalizationItem.Save_as);
            }
            else
            {
                //this.MniSaveAs.Visible = false;
                //tsmi_delete.Visible = false;
                //tsmi_rename.Visible = false;
                //tsmi_saveas.Text = Variables.localisationManager.getValue(ArcMapAddinIsogeo.Localization.LocalizationItem.Save) + " " + Variables.localisationManager.getValue(Localization.LocalizationItem.Save_as);
            }
        }*/

        private void tsmi_save_Click(object sender, EventArgs e)
        {
            foreach (ArcMapAddinIsogeo.Configuration.Search search in Variables.configurationManager.config.searchs.searchs)
            {
                if (search.name == searchSelectedName)
                {
                    search.query = Variables.restFunctions.getQueryCombos();
                    Variables.configurationManager.save();
                    break;
                }
            }

        }

        public void tsmi_new_Click(object sender, EventArgs e)
        {
            //Variables.dockableWindowIsogeo.ResultsToolBar.set.setSortingDefault();
            Variables.restFunctions.reloadinfosAPI(Variables.configurationManager.config.query + " ", 0, false);
            searchSelectedName = "";
            //selectedSavedChange();

        }


        private void tsmi_saved_click(object sender, EventArgs e)
        {
            searchSelectedName = ((MenuItem)sender).Name;
            foreach (ArcMapAddinIsogeo.Configuration.Search search in Variables.configurationManager.config.searchs.searchs)
            {
                if (search.name == searchSelectedName)
                {
                    if (search.query.IndexOf("action:view") == -1)
                    {
                        search.query += " action:view";
                    }
                    Variables.restFunctions.reloadinfosAPI(search.query, 0, false);
                    break;
                }
            }
            //selectedSavedChange();
        }

        public void tsmi_previous_Click(object sender, EventArgs e)
        {
            foreach (ArcMapAddinIsogeo.Configuration.Search search in Variables.configurationManager.config.searchs.searchs)
            {
                if (search.name == "CurrentSearchSave")
                {
                    Variables.restFunctions.reloadinfosAPI(search.query, 0, false);
                    break;
                }
            }
            searchSelectedName = "";
            //selectedSavedChange();
        }

        private void tsmi_rename_Click(object sender, EventArgs e)
        {

            //UI.Search.SearchManagment.AddSearchManagment frm = new UI.Search.SearchManagment.AddSearchManagment(true, searchSelectedName);
 //           frm.ShowDialog();
 //           if (frm.isSave == false) return;

//            String new_name = frmtxt_name.Text;
            foreach (MenuItem tsmi in lstTSMISearch)
            {
                if (tsmi.Name == searchSelectedName)
                {
  //                  tsmi.Name = new_name;
                    break;
                }
            }

            foreach (ArcMapAddinIsogeo.Configuration.Search search in Variables.configurationManager.config.searchs.searchs)
            {
                if (search.name == searchSelectedName)
                {
   //                 search.name = new_name;
                    Variables.configurationManager.save();
                    break;
                }
            }

 //           searchSelectedName = new_name;
           // selectedSavedChange();
        }

        private void tsmi_saveas_Click(object sender, EventArgs e)
        {
            //Appel du formulaire
            //UI.Search.SearchManagment.AddSearchManagment frm = new UI.Search.SearchManagment.AddSearchManagment(false, "");
            //frm.ShowDialog();

            //Si cancel
            //if (frm.isSave == false) return;


            //String new_name = frm.txt_name.Text;

            //Ajout dans configuration
            ArcMapAddinIsogeo.Configuration.Search newsearch = new ArcMapAddinIsogeo.Configuration.Search();
            //newsearch.name = new_name;
            newsearch.query = Variables.restFunctions.getQueryCombos();
            Variables.configurationManager.config.searchs.searchs.Add(newsearch);

            Variables.configurationManager.save();

            //Ajout dans le menu
            MenuItem newToolStripMenuItem = new MenuItem();
            //newToolStripMenuItem.Header = new_name;
            //newToolStripMenuItem. = tsmi_saved_click;
            //(new_name, null, tsmi_saved_click);
            lstTSMISearch.Add(newToolStripMenuItem);
            //this.MniSearchManagement
            //tsmi_search_management.DropDownItems.Insert(lstTSMISearch.Count - 1, newToolStripMenuItem);

            //selecton de la nouvelle recherche
            //searchSelectedName = new_name;
         //   selectedSavedChange();


        }

        private void tsmi_delete_Click(object sender, EventArgs e)
        {
            //Suppression du menu
            foreach (MenuItem tsmi in lstTSMISearch)
            {
                if (tsmi.Name == searchSelectedName)
                {
                    //tsmi_search_management.DropDownItems.Remove(tsmi);
                    lstTSMISearch.Remove(tsmi);
                    break;
                }
            }

            //Suppression dans la configuration
            foreach (ArcMapAddinIsogeo.Configuration.Search search in Variables.configurationManager.config.searchs.searchs)
            {
                if (search.name == searchSelectedName)
                {
                    Variables.configurationManager.config.searchs.searchs.Remove(search);
                    break;
                }
            }
            searchSelectedName = "";
            Variables.configurationManager.save();

            //mise à jour du menu
         //   selectedSavedChange();
        }
    }
}
