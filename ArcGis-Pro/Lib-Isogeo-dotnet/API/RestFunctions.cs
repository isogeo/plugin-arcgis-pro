using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using RestSharp;
using Newtonsoft.Json;

namespace ArcMapAddinIsogeo.API
{
    public class RestFunctions
    {
        private Boolean isCustomQuery=false;
        public RestFunctions()
        {
            
        }

        public void reloadinfosAPI(String query, int page, Boolean isResult)
        {
            Variables.haveResult = isResult;
           // saveLastSearch();

            if (query=="")
            {
                isCustomQuery = false;
            }
            else{
                isCustomQuery = true;
            }
            
            //Variables.dockableWindowIsogeo.Enabled = false;
            setConnexion();
            

            //Variables.search = new API.Search();
            if (Variables.token.access_token != null)
            {
               // sendRequestIsogeo(query,page,isResult);
                //Variables.dockableWindowIsogeo.Enabled = true;
            }
            else
            {
                //Variables.dockableWindowIsogeo.Enabled = true;
                //Ui.Authentification.FrmAuthentification frmAuthentification = new Ui.Authentification.FrmAuthentification();
                //frmAuthentification.ShowDialog();
            }
        }


        public void getDetails(String md_id)
        {

//            Variables.dockableWindowIsogeo.Enabled = false;
            setConnexion();


            if (Variables.token.access_token != null)
            {
                sendRequestIsogeoDetails(md_id);
//                Variables.dockableWindowIsogeo.Enabled = true;
            }
            else
            {
//                Variables.dockableWindowIsogeo.Enabled = true;
//                Ui.Authentification.FrmAuthentification frmAuthentification = new Ui.Authentification.FrmAuthentification();
//                frmAuthentification.ShowDialog();
            }
        }

        public void setConnexion()
        {
             Variables.token = new API.Token();

            String clientId = Variables.configurationManager.config.userAuthentification.id;  
            String clientSecret = "";
            if (Variables.configurationManager.config.userAuthentification.secret != "")
            {
                clientSecret = RijndaelManagedEncryption.DecryptRijndael(Variables.configurationManager.config.userAuthentification.secret, Variables.encryptCode);
            }


            if (clientSecret != "")
            {
                Variables.token = askForToken(clientId, clientSecret);
            }
            else
            {
                if (Variables.isFirstLoad == false)
                {
                    Variables.isFirstLoad = false;
//                    MessageBox.Show(Variables.dockableWindowIsogeo, Variables.localisationManager.getValue(Localization.LocalizationItem.Message_Query_authentification_ko_invalid) + "\n" + Variables.localisationManager.getValue(Localization.LocalizationItem.Message_contact_support), "Isogeo");
                }
                Variables.isFirstLoad = false;
                return;
            }
            switch (Variables.token.StatusResult)
            {
                case "NotFound":
//                    MessageBox.Show(Variables.dockableWindowIsogeo, Variables.localisationManager.getValue(Localization.LocalizationItem.Message_Query_authentification_ko_internet) + "\n" + Variables.localisationManager.getValue(Localization.LocalizationItem.Message_contact_support), "Isogeo");
                    break;
                case "OK":

                    break;
                case "BadRequest":
//                    MessageBox.Show(Variables.dockableWindowIsogeo, Variables.localisationManager.getValue(Localization.LocalizationItem.Message_Query_authentification_ko_invalid) + "\n" + Variables.localisationManager.getValue(Localization.LocalizationItem.Message_contact_support), "Isogeo");
                    break;
                case "0":
                    if (Variables.isFirstLoad == false)
                    {
                        Variables.isFirstLoad = false;
 //                       MessageBox.Show(Variables.dockableWindowIsogeo, Variables.localisationManager.getValue(Localization.LocalizationItem.Message_Query_authentification_ko_proxy) + "\n" + Variables.localisationManager.getValue(Localization.LocalizationItem.Message_contact_support), "Isogeo");
                    }
                    
                    break;
                default:
//                    MessageBox.Show(Variables.dockableWindowIsogeo, Variables.localisationManager.getValue(Localization.LocalizationItem.Message_Query_authentification_ko_internet) + "\n" + Variables.localisationManager.getValue(Localization.LocalizationItem.Message_contact_support), "Isogeo");
                    break;

            }
            Variables.isFirstLoad = false;
        }
        public API.Token askForToken(String clientId, String clientSecret)
        {
            
            API.Token token = new API.Token();
            token.StatusResult="";
            try
            {
                String encodedParam = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(clientId + ":" + clientSecret));
                String url = "https://id.api.isogeo.com/oauth/token";
                var client = new RestClient(url);
                //setProxy(client);
                var request2 = new RestRequest(Method.POST);
                request2.AddHeader("cache-control", "no-cache");
                request2.AddHeader("content-type", "application/x-www-form-urlencoded");
                request2.AddHeader("Authorization", "Basic " + encodedParam);
                request2.AddParameter("application/x-www-form-urlencoded", "grant_type=client_credentials", ParameterType.RequestBody);
                IRestResponse response = client.Execute(request2);
                String statusCode = response.StatusCode.ToString();//"NotFound" "OK" //"BadRequest"

                
                if (statusCode == "OK")
                {
                    token = JsonConvert.DeserializeObject<API.Token>(response.Content);

                }
                token.StatusResult = statusCode;

            }
            catch (Exception ex)
            {
/*                Utils.Log.DockableWindowLogger.Debug(string.Concat(new object[]
				{
					"Erreur ",
					ex.Message
				}));   */
            }
            return token;
        }
        
        public void sendRequestIsogeoDetails(String md_id)
        {
            //nbresult = 99;
            
            try
            {

                

                String url = "https://v1.api.isogeo.com/resources/" + md_id;
                //url = "https://v1.api.isogeo.com/resources/db3fa300ebd44daf95b6ffd64f7cbe4e/";

            
                var client = new RestClient(url);
                setProxy(client);
                var request = new RestRequest(Method.GET);

                request.AddHeader("Authorization", "Bearer " + Variables.token.access_token);
                
                request.AddParameter("_include", "conditions");
                request.AddParameter("_include", "contacts");
                request.AddParameter("_include", "coordinate-system");
                request.AddParameter("_include", "events");
                request.AddParameter("_include", "feature-attributes");
                request.AddParameter("_include", "limitations");
                request.AddParameter("_include", "keywords");
                request.AddParameter("_include", "specifications");
                //IRestResponse<API.Search> reponse = client.Execute<API.Search>(request);

                IRestResponse response = client.Execute(request);
                Result result = JsonConvert.DeserializeObject<Result>(response.Content);

                

                Variables.currentResult = result;
                Variables.currentResult.tagsLists = new Objects.Tags(result);
            }
            catch (Exception ex)
            {
 /*               Utils.Log.DockableWindowLogger.Debug(string.Concat(new object[]
				{
					"Erreur ",
					ex.Message
				}));*/
            }
            
        }

        public void sendRequestIsogeo(String query,int page,Boolean isResult)
        {
            //nbresult = 99;
            
            try
            {
                int nbResult = 0;
                int offset = 0;
                int nbpage = 0;
                if (isResult == true)
                {
                   // nbResult = Convert.ToInt32(Math.Floor(Convert.ToDecimal((Variables.dockableWindowIsogeo.reultsPanel1.lst_results.Height - 10) / 35)));
                    nbpage = Convert.ToInt32(Math.Ceiling(Variables.search.total / nbResult));
                    if (Variables.currentPage > nbpage) Variables.currentPage = nbpage;
                    offset = (Variables.currentPage - 1) * nbResult;
                    

                }
                else
                {
                //    Variables.dockableWindowIsogeo.reultsPanel1.clearPages();
                }


                Variables.search = new Search();
                //Variables.currentPage
                //Variables.search
                
                

                    

                String url = "https://v1.api.isogeo.com/resources/search"; //?&_limit=0
                var client = new RestClient(url);
                setProxy(client);
                var request = new RestRequest(Method.GET);
                /*if ((string)Variables.combo_lang.SelectedValue == "default")
                {
                }
                else
                {
                }*/

                //request.AddParameter("_include", "links");
                request.AddParameter("_include", "layers");
                request.AddParameter("_include", "serviceLayers");
                
                //request.AddParameter("_lang", Utils.Util.getLocale());
                request.AddParameter("_limit", nbResult);

//                request.AddParameter("ob", Variables.dockableWindowIsogeo.resultsToolBar1.cmb_sorting_method.SelectedValue);
//                request.AddParameter("od", Variables.dockableWindowIsogeo.resultsToolBar1.cmb_sorting_direction.SelectedValue);

                //request.AddParameter("_include", "conditions");
                //request.AddParameter("_include", "contacts");
                //request.AddParameter("_include", "coordinate-system");
                //request.AddParameter("_include", "events");
                //request.AddParameter("_include", "feature-attributes");
                //request.AddParameter("_include", "limitations");
                //request.AddParameter("_include", "keywords");
                //request.AddParameter("_include", "specifications");

                if (isResult == true)
                {
                    request.AddParameter("_offset", offset);
                }
                
                if (query == "")
                {
                    query=getQueryCombos();
                }

                if (query.IndexOf("action:view") == -1)
                {
                    query += " action:view";
                }

                //temp test                
                request.AddParameter("q", query.Replace("action:view", ""));

/*                if (Variables.advancedSreachItem_geographicFilter.cmb_search.SelectedIndex == 1)
                {
                    //request.AddParameter("coord", Utils.MapFunctions.getMapExtent());

                    //request.AddParameter("box", "-4.970, 30.69418, 8.258, 51.237");
                    request.AddParameter("box", Utils.MapFunctions.getMapExtent());
                    
                    //request.AddParameter("extent", Utils.MapFunctions.getMapExtent());
                    //request.AddParameter("epsg", "4326");
                    request.AddParameter("rel", Variables.configurationManager.config.geographicalOperator);
                }
                
                if (Variables.advancedSreachItem_geographicFilter.cmb_search.SelectedIndex > 1)
                {
                    request.AddParameter("box", Utils.MapFunctions.getLayerExtent(Variables.layersVisible[Variables.advancedSreachItem_geographicFilter.cmb_search.SelectedIndex - 2]));
                    request.AddParameter("rel", Variables.configurationManager.config.geographicalOperator);
                }
                //if (layer.Visible == true)
                //{
                //    ESRI.ArcGIS.Geometry.IEnvelope env = layer.AreaOfInterest.Envelope;
                //}
                //request.AddParameter("q", "format:shp type:vector-dataset"); 
                //client.Proxy=new WebProxy(ff, false);
                request.AddHeader("Authorization", "Bearer " + Variables.token.access_token);

                //IRestResponse<API.Search> reponse = client.Execute<API.Search>(request);

                IRestResponse response = client.Execute(request);
                response.Content.Replace("\"coordinate-system\": {", "\"coordinate_system\": {");
                Variables.search = JsonConvert.DeserializeObject<API.Search>(response.Content);

                
                

                setSearchList(query);

                Variables.dockableWindowIsogeo.resultsToolBar1.setNbResults();
                if (isResult==true)
                {
                    Variables.dockableWindowIsogeo.reultsPanel1.setData();
                    Variables.dockableWindowIsogeo.reultsPanel1.setCombo(nbpage);

                }
                */

            }
            catch (Exception ex)
            {
                /*Utils.Log.DockableWindowLogger.Debug(string.Concat(new object[]
				{
					"Erreur ",
					ex.Message
				}));*/
            }
            
        }
        public void setProxy(RestClient client)
        {
            if (!String.IsNullOrEmpty(Variables.configurationManager.config.proxy.proxyUrl))
            {
                WebProxy myproxy = new WebProxy(Variables.configurationManager.config.proxy.proxyUrl, false);

                if (!String.IsNullOrEmpty(Variables.configurationManager.config.proxy.proxyUser) && !String.IsNullOrEmpty(Variables.configurationManager.config.proxy.proxyPassword))
                {
                    myproxy.Credentials = new NetworkCredential(Variables.configurationManager.config.proxy.proxyUser, Variables.configurationManager.config.proxy.proxyPassword);
                }

                client.Proxy = myproxy;
            }
            
        }

        public void setSearchList(String query)
        {
            String textInput = "";
            Variables.searchLists=new API.SearchLists();

            Variables.searchLists.list.Add(new SearchList("type",true));
            Variables.searchLists.list.Add(new SearchList("keyword:inspire-theme", true));
            Variables.searchLists.list.Add(new SearchList("keyword:isogeo", true));
            Variables.searchLists.list.Add(new SearchList("format", true));
            Variables.searchLists.list.Add(new SearchList("coordinate-system", true));
            Variables.searchLists.list.Add(new SearchList("owner", true));
            Variables.searchLists.list.Add(new SearchList("action", true));
            Variables.searchLists.list.Add(new SearchList("contact", true));
            Variables.searchLists.list.Add(new SearchList("license", true));

            
            //foreach (SearchList item in Variables.searchLists.list)
            //{
            //    item.lstItem=new List<Objects.comboItem>;
            //    item.lstItem.Add(new Objects.comboItem("","-");
            //}

            
            

            if (Variables.search.tags == null) return;
            foreach (var item in Variables.search.tags)
            {
                String key = item.Key;
                String val = item.Value;
                foreach (SearchList lst in Variables.searchLists.list)
                {
                    if (key.IndexOf(lst.filter) == 0)
                    {
                        lst.lstItem.Add(new Objects.comboItem(key, val));//.Substring(lst.filter.Length+1)                        
                        break;
                    }
                }
            }

            if (query != "")
            {
                string[] queryItems = query.Split(' ');

                foreach (SearchList lst in Variables.searchLists.list)
                {
                    lst.query = "-";
                }
                
                foreach (string queryItem in queryItems)
                {
                    Boolean find = false;
                    if (queryItem.Trim() != "")
                    {
                        foreach (SearchList lst in Variables.searchLists.list)
                        {

                            if (queryItem.IndexOf(lst.filter+ ":") == 0)
                            {
                                lst.query = queryItem;
                                find = true;
                                break;
                            }
                        }
                    }
                    
                    if (find == false)
                    {
                        if (textInput != "") textInput += " ";
                        textInput += queryItem;
                    }

                    //string[] queryItemsKeyValue = queryItem.Split(':');
                    //if (queryItemsKeyValue.Length == 2 || queryItemsKeyValue.Length == 3)
                    //{
                    //    String keyQuery = queryItemsKeyValue[0];

                    //    foreach (SearchList lst in Variables.searchLists.list)
                    //    {
                    //        if (keyQuery.IndexOf(lst.filter) == 0)
                    //        {
                    //            lst.query = queryItem;
                    //            break;
                    //        }
                    //    }
                    //}
                    //else
                    //{
                    //    if (textInput != "") textInput += " ";
                    //    textInput += queryItem;
                    //}
                }
                
            }
            

            

            foreach (SearchList lst in Variables.searchLists.list)
            {
                lst.lstItem = lst.lstItem.OrderBy(x => x.value).ToList();
                lst.lstItem.Insert(0,new Objects.comboItem("-","-"));

            }

            Variables.ListLoading = true;
            foreach (Action func in Variables.functionsSetlist)
            {
                func();
            }

            if (query != "")
            {
            //    Variables.dockableWindowIsogeo.principalSearchPanel1.searchItems1.txt_search.Text = textInput;
            }
            
            Variables.ListLoading = false;
        }


        public String getQueryCombos()
        {
            String filter="";

            /*foreach (ComboBox cmb in Variables.listComboFilter)
            {
                if (cmb.SelectedValue != null) 
                {
                    String valcmbValue = (String)cmb.SelectedValue;
                    if (valcmbValue != "-")
                    {
                        if (filter != "") filter = filter + " ";
                        filter = filter + valcmbValue;
                    }
                }
            }*/

            if (filter.IndexOf("action:view") == -1)
            {
                filter += " action:view";
            }
            
           // filter += " " +Variables.dockableWindowIsogeo.principalSearchPanel1.searchItems1.txt_search.Text;
         
   
            return filter;
        }

        private void saveLastSearch()
        {
            ArcMapAddinIsogeo.Configuration.Search currentSearch=null;
            foreach (ArcMapAddinIsogeo.Configuration.Search search in Variables.configurationManager.config.searchs.searchs)
            {
                if (search.name == "CurrentSearchSave")
                {
                    currentSearch = search;
                    break;
                }
            }
            if (currentSearch==null) {
            currentSearch=new ArcMapAddinIsogeo.Configuration.Search();
            currentSearch.name="CurrentSearchSave";            
            Variables.configurationManager.config.searchs.searchs.Add(currentSearch);
            }
            
           // currentSearch.query = Variables.restFunctions.getQueryCombos();
            Variables.configurationManager.save();


        }
       /* public void setListCombo(ComboBox cmb,String listName)
        {
            foreach (API.SearchList lst in Variables.searchLists.list)
            {
                if (lst.filter == listName)
                {

                    try
                    {
                        String valcmbValue = null;
                        if (cmb.SelectedValue != null) valcmbValue = (String)cmb.SelectedValue;
                        if (isCustomQuery)
                        {
                            cmb.SelectedValue = "-";
                        }
                        if (lst.query != "")
                        {
                            valcmbValue = lst.query;
                        }
                        cmb.DataSource = new BindingSource(lst.lstItem, null);
                        cmb.ValueMember = "code";
                        cmb.DisplayMember = "value";

                        if (valcmbValue != null)
                        {
                            cmb.SelectedValue = valcmbValue;
                        }

                    }
                    catch (Exception ex)
                    {
                        Utils.Log.DockableWindowLogger.Debug(string.Concat(new object[]
				        {
					        "Erreur ",
					        ex.Message
				        }));
                    }

                }
            }
        }*/
    }
}
