using System;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using ArcGIS.Desktop.Framework.Dialogs;
using Isogeo.Map.MapFunctions;
using Isogeo.Models.API;
using Isogeo.Models.Configuration;
using Isogeo.Models.Filters;
using Isogeo.Utils.LogManager;
using Isogeo.Utils.ManageEncrypt;
using MVVMPattern.MediatorPattern;
using Newtonsoft.Json;
using RestSharp;
using Resource = Isogeo.Language.Resources;
using Search = Isogeo.Models.API.Search;

namespace Isogeo.Models.Network
{
    public class RestFunctions
    {
        private Authentication.Authentication _frmAuthentication;

        private bool AuthenticationPopUpIsOpen => _frmAuthentication != null && _frmAuthentication.IsLoaded;

        public static bool isFirstLoad = true;
        private static bool _isFirstUserRequest = true;

        public class ApiParameters
        {
            internal string ob;
            internal string od;
            internal int offset;
            internal string lang;
            internal string rel;
            internal Token token;
            internal string query;
            internal string box;

            public ApiParameters(
                Token token,
                string query = "",
                string ob = "",
                string od = "",
                int offset = 0,
                string rel = "",
                string box = "")
            {
                this.query = query;
                this.token = token;
                this.ob = ob;
                this.od = od;
                this.rel = rel;
                this.offset = offset;
                lang = Thread.CurrentThread.CurrentCulture.Name;
                this.box = box;
            }
        }

        public Token SetConnection(string clientId, string clientSecret)
        {
            Token newToken;

            Log.Logger.Info("Set Connection : " + clientId);
            if (!string.IsNullOrEmpty(clientSecret))
            {
                newToken = AskForToken(clientId, clientSecret);
                if (newToken == null)
                {
                    MessageBox.Show(Resource.Message_Query_authentication_ko_invalid + "\n" +
                                    Resource.Message_contact_support, "Isogeo");
                    Log.Logger.Info(Resource.Message_Query_authentication_ko_invalid + "\n" +
                    Resource.Message_contact_support);
                    return null;
                }
            }
            else
            {
                if (isFirstLoad == false)
                {
                    MessageBox.Show(Resource.Message_Query_authentication_ko_invalid + "\n" +
                                    Resource.Message_contact_support, "Isogeo");
                    Log.Logger.Info(Resource.Message_Query_authentication_ko_invalid + "\n" +
                                    Resource.Message_contact_support);
                }
                isFirstLoad = false;
                return null;
            }
            switch (newToken.StatusResult)
            {
                case "NotFound":
                    MessageBox.Show(Resource.Message_Query_authentication_ko_internet + "\n" +
                        Resource.Message_contact_support, "Isogeo");
                    Log.Logger.Info(Resource.Message_Query_authentication_ko_internet + "\n" +
                                    Resource.Message_contact_support);
                    break;
                case "OK":

                    break;
                case "BadRequest":
                    MessageBox.Show(Resource.Message_Query_authentication_ko_invalid + "\n" +
                        Resource.Message_contact_support, "Isogeo");
                    Log.Logger.Info(Resource.Message_Query_authentication_ko_invalid + "\n" +
                                    Resource.Message_contact_support);
                    break;
                case "0":
                    if (isFirstLoad == false)
                    {
                        MessageBox.Show(Resource.Message_Query_authentication_ko_proxy + "\n" +
                                        Resource.Message_contact_support, "Isogeo");
                        Log.Logger.Info(Resource.Message_Query_authentication_ko_proxy + "\n" +
                                        Resource.Message_contact_support);
                    }

                    break;
                default:
                    MessageBox.Show(Resource.Message_Query_authentication_ko_internet + "\n" +
                        Resource.Message_contact_support, "Isogeo");
                    Log.Logger.Info(Resource.Message_Query_authentication_ko_internet + "\n" +
                                    Resource.Message_contact_support);
                    break;

            }
            isFirstLoad = false;
            return newToken;
        }

        public void OpenAuthenticationPopUp()
        {
            if (AuthenticationPopUpIsOpen) return;
            _frmAuthentication = new Authentication.Authentication();
            _frmAuthentication.ShowDialog();
        }

        private bool CheckFirstRequestThenTokenThenSearchRequest(ref string query, int offset)
        {
            if (_isFirstUserRequest)
            {
                _isFirstUserRequest = false;
                query = Variables.configurationManager.config.defaultSearch;
                var state = TokenThenSearchRequest(query, offset, Variables.nbResult);
                if (!state)
                    _isFirstUserRequest = true;
                return state;
            }
            return (TokenThenSearchRequest(query, offset, Variables.nbResult));
        }

        public void ResetData()
        {
            Log.Logger.Debug("Execute Reset Data");
            var query = "";
            Mediator.NotifyColleagues("ChangeBox", "");
            var state = CheckFirstRequestThenTokenThenSearchRequest(ref query, 0);
            Mediator.NotifyColleagues("ChangeQuery", new QueryItem { query = query, box = GetBoxRequest()});
            Mediator.NotifyColleagues("ClearResults", null);
            if (!state)
                OpenAuthenticationPopUp();
        }

        public void LoadData(string query, int offset, string box = null)
        {
            Log.Logger.Debug("Execute Load Data");
            if (!string.IsNullOrWhiteSpace(box))
                Mediator.NotifyColleagues("ChangeBox", box);
            var state = CheckFirstRequestThenTokenThenSearchRequest(ref query, offset);
            Mediator.NotifyColleagues("ChangeQuery", new QueryItem { query = query, box = GetBoxRequest() });
            if (state)
                Mediator.NotifyColleagues("ChangeOffset", offset);
            else
            {
                Mediator.NotifyColleagues("ClearResults", offset);
                OpenAuthenticationPopUp();
            }
        }

        public void ReloadData(int offset)
        {
            Log.Logger.Debug("Execute Reload Data");
            var query = GetQueryCombos();
            var state = CheckFirstRequestThenTokenThenSearchRequest(ref query, offset);
            Mediator.NotifyColleagues("ChangeQuery", new QueryItem { query = query, box = GetBoxRequest() });
            if (state)
                Mediator.NotifyColleagues("ChangeOffset", offset);
            else
            {
                Mediator.NotifyColleagues("ClearResults", offset);
                OpenAuthenticationPopUp();
            }
        }

        /// <summary>
        ///  Make request to API with request inside Query.
        ///  If query parameter is empty, query will be chosen from UI combobox.
        /// </summary>
        /// <param name="query"></param>
        /// <param name="offset"></param>
        /// <param name="nbResult"></param>
        private bool TokenThenSearchRequest(string query, int offset, int nbResult)
        {
            var state = true;

            Mediator.NotifyColleagues("EnableDockableWindowIsogeo", false);
            try
            {
                var newToken = SetConnection(Variables.configurationManager.config.userAuthentication.id,
                    RijndaelManagedEncryption.DecryptRijndael(
                        Variables.configurationManager.config.userAuthentication.secret, Variables.encryptCode));
                if (!(string.IsNullOrEmpty(newToken?.access_token) || newToken.StatusResult != "OK"))
                {
                    Variables.search =
                        SearchRequest(new ApiParameters(newToken, query, offset: offset, ob: Variables.cmbSortingMethodSelectedItem?.Id, 
                            od: Variables.cmbSortingDirectionSelectedItem?.Id, box: GetBoxRequest(), rel: GetRelRequest()), nbResult);
                    SetSearchList(query);
                }
                else
                    state = false;
            }
            catch (Exception ex)
            {
                Log.Logger.Error("Error Authentication : " + ex.Message);
                state = false;
            }
            finally
            {
                Mediator.NotifyColleagues("EnableDockableWindowIsogeo", true);
            }

            return state;
        }

        /// <summary>
        /// Get Metadata details from API, Window is locked during the process. If token is invalid, authentication pop-up is opened
        /// </summary>
        /// <param name="mdId">Metadata id from API</param>
        /// <returns></returns>
        public Result GetDetails(string mdId)
        {
            Log.Logger.Info("GetDetails - md_id : " + mdId);

            Mediator.NotifyColleagues("EnableDockableWindowIsogeo", false);
            Result result = null;
            try
            {
                var newToken = SetConnection(Variables.configurationManager.config.userAuthentication.id,
                    RijndaelManagedEncryption.DecryptRijndael(
                        Variables.configurationManager.config.userAuthentication.secret, Variables.encryptCode));
                if (!(string.IsNullOrEmpty(newToken?.access_token) || newToken.StatusResult != "OK"))
                { 
                    result = ApiDetailsResourceRequest(mdId, new ApiParameters(newToken));
                }
            }
            catch (Exception ex)
            {
                Log.Logger.Error("Error Authentication : " + ex.Message);
            }
            finally
            {
                Mediator.NotifyColleagues("EnableDockableWindowIsogeo", true);
            }
            Log.Logger.Debug("END GetDetails");
            return result;
        }

        /// <summary>
        /// Ask API for Token with client_id and client_secret and return it
        /// </summary>
        /// <param name="clientId">client_id API</param>
        /// <param name="clientSecret">client_secret API</param>
        /// <returns></returns>
        private Token AskForToken(string clientId, string clientSecret)
        {
            Log.Logger.Debug("Ask for Token - cliendId : " + clientId);
            Token newToken = null;
            try
            {
                var encodedParam = Convert.ToBase64String(Encoding.UTF8.GetBytes(clientId + ":" + clientSecret));
                var url = Variables.configurationManager.config.apiIdUrl + "oauth/token";
                var client = new RestClient(url);
                SetProxy(client);
                var request = new RestRequest(Method.POST);
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("content-type", "application/x-www-form-urlencoded");
                request.AddHeader("Authorization", "Basic " + encodedParam);
                request.AddParameter("application/x-www-form-urlencoded", "grant_type=client_credentials", ParameterType.RequestBody);
                var response = client.Execute(request);
                var statusCode = response.StatusCode.ToString();//"NotFound" "OK" //"BadRequest"

                
                if (statusCode == "OK")
                {
                    newToken = JsonConvert.DeserializeObject<Token>(response.Content);

                }

                if (newToken == null)
                    newToken = new Token();
                newToken.StatusResult = statusCode;
            }
            catch (Exception ex)
            {
                Log.Logger.Error("Error - " + ex.Message);
            }
            return newToken;
        }

        /// <summary>
        /// Ask API more details for one metadata and return it
        /// </summary>
        /// <param name="mdId">Id of one Isogeo API's metadata</param>
        /// <param name="parameters">Token</param>
        /// <returns></returns>
        private Result ApiDetailsResourceRequest(string mdId, ApiParameters parameters)
        {
            Log.Logger.Info("Execution DetailsResourceRequest - ID : " + mdId);
            try
            {
                var url = Variables.configurationManager.config.apiUrl + "resources/" + mdId;

                var client = new RestClient(url);
                SetProxy(client);
                var request = new RestRequest(Method.GET);

                request.AddHeader("Authorization", "Bearer " + parameters.token.access_token);
                
                request.AddParameter("_include", "conditions");
                request.AddParameter("_include", "contacts");
                request.AddParameter("_include", "coordinate-system");
                request.AddParameter("_include", "events");
                request.AddParameter("_include", "feature-attributes");
                request.AddParameter("_include", "limitations");
                request.AddParameter("_include", "keywords");
                request.AddParameter("_include", "specifications");
                request.AddParameter("_lang", CultureInfo.InstalledUICulture.TwoLetterISOLanguageName);

                var response = client.Execute(request);
                var result = JsonConvert.DeserializeObject<Result>(response.Content);

                result.tagsLists = new Tags(result);
                return result;
            }
            catch (Exception ex)
            {
                Log.Logger.Error("Error - " + ex.Message);
            }
            return null;
        }

        private static string GetRelRequest()
        {
            return !string.IsNullOrWhiteSpace(Variables.configurationManager.config.geographicalOperator) ? Variables.configurationManager.config.geographicalOperator : "";
        }

        public string GetBoxRequest()
        {
            var box = "";
            if (Variables.geographicFilter.SelectedItem.Name != "-")
                box = MapFunctions.GetMapExtent();
            return box;
        }

        private void DebugRequest(RestRequest request)
        {
            var line = "";
            foreach (var parameters in request.Parameters)
            {
                if (parameters.Name == "Authorization") continue;
                line += parameters.Name + " " + parameters.Value + ", ";
            }
            Log.Logger.Info("Request Isogeo API : " + line);
        }

        private Search SearchRequest(ApiParameters apiParameters, int nbResult)
        {
            Log.Logger.Info("Execution SearchRequest - query : " + '"' + apiParameters.query + '"' + ", offset : " + apiParameters.offset);
            try
            {
                Variables.search = new Search();

                var url = Variables.configurationManager.config.apiUrl + "resources/search";
                var client = new RestClient(url);
                SetProxy(client);
                var request = new RestRequest(Method.GET);

                request.AddParameter("_include", "layers");
                request.AddParameter("_include", "serviceLayers");

                request.AddParameter("_lang", apiParameters.lang);
                request.AddParameter("_limit", nbResult);

                request.AddParameter("ob", apiParameters.ob);
                request.AddParameter("od", apiParameters.od);
                request.AddParameter("_offset", apiParameters.offset);
                request.AddParameter("rel", apiParameters.rel);
                request.AddParameter("q", apiParameters.query);

                request.AddParameter("box", apiParameters.box);

                request.AddHeader("Authorization", "Bearer " + apiParameters.token.access_token);

                DebugRequest(request);
                var response = client.Execute(request);
                
                return JsonConvert.DeserializeObject<Search>(response.Content);
            }
            catch (Exception ex)
            {
                Log.Logger.Error("Error - " + ex.Message);
                return null;
            }
        }

        private void SetProxy(RestClient client)
        {
            Log.Logger.Debug("Initializing Proxy...");
            if (string.IsNullOrEmpty(Variables.configurationManager.config.proxy.proxyUrl)) return;
            Log.Logger.Info( "Setting Proxy...");
            var proxy = new WebProxy(Variables.configurationManager.config.proxy.proxyUrl, false);
            if (!string.IsNullOrEmpty(Variables.configurationManager.config.proxy.proxyUser) && !string.IsNullOrEmpty(Variables.configurationManager.config.proxy.proxyPassword))
            {
                proxy.Credentials = new NetworkCredential(Variables.configurationManager.config.proxy.proxyUser, RijndaelManagedEncryption.DecryptRijndael(
                    Variables.configurationManager.config.proxy.proxyPassword, Variables.encryptCode));
            }
            client.Proxy = proxy;
            Log.Logger.Debug("END Initializing Proxy");
        }

        /// <summary>
        /// Set current search lists with query parameters
        /// Set at the end of the method combobox' sourceItems
        /// </summary>
        /// <param name="query">query is used to define current search lists</param>
        private void SetSearchList(string query)
        {
            Log.Logger.Debug("Set search List - query : " + query);
            var textInput = "";
            Variables.searchLists = new SearchLists();

            Variables.searchLists.list.Add(new SearchList("type", true));
            Variables.searchLists.list.Add(new SearchList("keyword:inspire-theme", true));
            Variables.searchLists.list.Add(new SearchList("keyword:isogeo", true));
            Variables.searchLists.list.Add(new SearchList("format", true));
            Variables.searchLists.list.Add(new SearchList("coordinate-system", true));
            Variables.searchLists.list.Add(new SearchList("owner", true));
            Variables.searchLists.list.Add(new SearchList("action", true));
            Variables.searchLists.list.Add(new SearchList("contact", true));
            Variables.searchLists.list.Add(new SearchList("license", true));

            if (Variables.search?.tags == null) return;
            foreach (var item in Variables.search.tags)
            {
                var key = item.Key;
                var val = item.Value;
                foreach (var lst in Variables.searchLists.list)
                {
                    if (key.IndexOf(lst.filter, StringComparison.Ordinal) != 0) continue;
                    lst.lstItem.Add(new FilterItem { Id = key, Name = val});
                    break;
                }
            }

            if (!string.IsNullOrWhiteSpace(query))
            {
                var queryItems = query.Split(' ');

                foreach (var lst in Variables.searchLists.list)
                {
                    lst.query = "";
                }

                foreach (var queryItem in queryItems)
                {
                    var find = false;
                    if (queryItem.Trim() != "")
                    {
                        foreach (var lst in Variables.searchLists.list)
                        {

                            if (queryItem.IndexOf(lst.filter + ":", StringComparison.Ordinal) == 0)
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
                }

            }

            foreach (var lst in Variables.searchLists.list)
            {
                lst.lstItem = lst.lstItem.OrderBy(x => x.Name).ToList();
            }


            Variables.listLoading = true;
            foreach (var func in Variables.functionsSetlist)
            {
                func();
            }

            if (string.IsNullOrWhiteSpace(query))
            {
                Variables.searchText = textInput;
            }

            Variables.listLoading = false;
            Log.Logger.Debug("END Set search List");
        }

        /// <summary>
        /// Get current query chosen by the user (UI)
        /// </summary>
        /// <returns></returns>
        public string GetQueryCombos()
        {
            Log.Logger.Debug("Get query from UI");
            var filter = "";

            foreach (var cmb in Variables.listComboFilter)
            {
                if (cmb?.SelectedItem != null && cmb.SelectedItem?.Name != "-" && cmb.SelectedItem.Id != null)
                {
                    if (filter != "")
                        filter += " ";
                    filter += cmb.SelectedItem.Id;
                }
            }
            filter += " " + Variables.searchText;

            Log.Logger.Debug("END Get query from UI - Query : " + filter);
            return filter;
        }

        /// <summary>
        /// Save the last search request realized inside configurationManager
        /// </summary>
        public void SaveSearch()
        {
            Log.Logger.Info("Save Last search");
            Configuration.Search currentSearch = null;
            if (Variables.configurationManager?.config?.searchs?.searchs == null)
                return;
            foreach (var search in Variables.configurationManager.config.searchs.searchs)
            {
                if (search.name == Resource.Previous_search)
                {
                    currentSearch = search;
                    break;
                }
            }
            if (currentSearch == null) {
                currentSearch = new Configuration.Search {name = Resource.Previous_search};

                Variables.configurationManager.config.searchs.searchs.Add(currentSearch);
            }
            currentSearch.query = GetQueryCombos();
            currentSearch.box = GetBoxRequest();
            Variables.configurationManager.Save();
            Mediator.NotifyColleagues("ChangeQuickSearch", null);
            Log.Logger.Info("END Save Last search - Query saved : " + '"' + currentSearch.query + '"');
        }

        /// <summary>
        /// Set Filters used by UI with current search lists
        /// </summary>
        /// <param name="cmb"></param>
        /// <param name="listName"></param>
        public void SetListCombo(Filters.Filters cmb, string listName)
        {
            Log.Logger.Debug("Set UI Filter " + listName);
            foreach (var lst in Variables.searchLists.list)
            {
                if (lst.filter == listName)
                {
                    try
                    {
                        cmb.SetItems(lst.lstItem);
                        if (cmb.Items.Any(s => s?.Id != null && !string.IsNullOrWhiteSpace(lst.query) &&
                                               string.Concat(s.Id.Where(c => !char.IsWhiteSpace(c))).ToLower()
                                                   .Equals(string.Concat((lst.query).Where(c => !char.IsWhiteSpace(c))).ToLower())))
                            cmb.SelectItem("", lst.query);
                        else
                            cmb.SelectItem("-");
                    }
                    catch (Exception ex)
                    {
                        Log.Logger.Error("Error - " + listName + " : " + ex.Message);
                    }
                }
            }
            Log.Logger.Debug("END - Set UI Filter " + listName);
        }
    }
}
