using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Windows;
using Isogeo.Models.API;
using Isogeo.Models.Configuration;
using Isogeo.Network;
using Isogeo.Utils.LogManager;
using Isogeo.Utils.ManageEncrypt;
using MVVMPattern.MediatorPattern;
using MessageBox = ArcGIS.Desktop.Framework.Dialogs.MessageBox;
using Resource = Isogeo.Language.Resources;
using Search = Isogeo.Models.API.Search;

namespace Isogeo.Models.Network
{
    public class RestFunctions : IRestFunctions
    {
        private Authentication.Authentication _frmAuthentication;

        private bool AuthenticationPopUpIsOpen => _frmAuthentication != null && _frmAuthentication.IsLoaded;

        private static bool _isFirstLoad = true;
        private static bool _isFirstUserRequest = true;

        private readonly HttpClient _client;

        public RestFunctions()
        {
            var proxy = GetProxy();
            var httpClientHandler = new HttpClientHandler()
            {
                Proxy = proxy
            };
            _client = new HttpClient(httpClientHandler);
        }

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

        internal async Task<Token> SetConnection(string clientId, string clientSecret)
        {
            Token newToken;

            Log.Logger.Info("Set Connection : " + clientId);
            if (!string.IsNullOrEmpty(clientSecret))
            {
                newToken = await GetAccessToken(clientId, clientSecret);
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
                if (_isFirstLoad == false)
                {
                    MessageBox.Show(Resource.Message_Query_authentication_ko_invalid + "\n" +
                                    Resource.Message_contact_support, "Isogeo");
                    Log.Logger.Info(Resource.Message_Query_authentication_ko_invalid + "\n" +
                                    Resource.Message_contact_support);
                }
                _isFirstLoad = false;
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
                    if (_isFirstLoad == false)
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
            _isFirstLoad = false;
            return newToken;
        }

        public void OpenAuthenticationPopUp()
        {
            if (AuthenticationPopUpIsOpen) return;
            _frmAuthentication = new Authentication.Authentication(this);
            _frmAuthentication.ShowDialog();
        }

        private async Task<(bool, string)> CheckFirstRequestThenTokenThenSearchRequest(string query, int offset, string box, string od, string ob)
        {
            if (_isFirstUserRequest)
            {
                _isFirstUserRequest = false;
                query = Variables.configurationManager.config.defaultSearch;
                var state = await TokenThenSearchRequest(query, offset, Variables.nbResult, box,  od, ob);
                if (!state)
                    _isFirstUserRequest = true;
                return (state, query);
            }
            return ((await TokenThenSearchRequest(query, offset, Variables.nbResult, box, od, ob)), query);
        }

        public async Task ResetData(string box, string od, string ob)
        {
            Log.Logger.Debug("Execute Reset Data");
            const string query = "";
            Mediator.NotifyColleagues("ChangeBox", "");
            var response = await CheckFirstRequestThenTokenThenSearchRequest(query, 0, box, od, ob);
            Mediator.NotifyColleagues("ChangeQuery", new QueryItem { query = response.Item2 });
            Mediator.NotifyColleagues("ClearResults", null);
            if (!response.Item1)
                Application.Current.Dispatcher.Invoke(OpenAuthenticationPopUp); // todo
        }

        public async Task LoadData(string query, int offset, string box, string od, string ob)
        {
            Log.Logger.Debug("Execute Load Data");
            if (!string.IsNullOrWhiteSpace(box))
                Mediator.NotifyColleagues("ChangeBox", box);
            var response = await CheckFirstRequestThenTokenThenSearchRequest(query, offset, box, od, ob);
            Mediator.NotifyColleagues("ChangeQuery", new QueryItem { query = response.Item2 });
            if (response.Item1)
                Mediator.NotifyColleagues("ChangeOffset", offset);
            else
            {
                Mediator.NotifyColleagues("ClearResults", offset);
                Application.Current.Dispatcher.Invoke(OpenAuthenticationPopUp); // todo
            }
        }

        public async Task ReloadData(int offset, string comboQuery, string box, string od, string ob)
        {
            Log.Logger.Debug("Execute Reload Data");
            //var query = GetQueryCombos();
            var result = await CheckFirstRequestThenTokenThenSearchRequest(comboQuery, offset, box, od, ob);
            Mediator.NotifyColleagues("ChangeQuery", new QueryItem { query = result.Item2 });
            if (result.Item1)
                Mediator.NotifyColleagues("ChangeOffset", offset);
            else
            {
                Mediator.NotifyColleagues("ClearResults", offset);
                Application.Current.Dispatcher.Invoke(OpenAuthenticationPopUp); // todo
            }
        }

        /// <summary>
        ///  Make request to API with request inside Query.
        ///  If query parameter is empty, query will be chosen from UI combobox.
        /// </summary>
        private async Task<bool> TokenThenSearchRequest(string query, int offset, int nbResult, string box, string od, string ob)
        {
            var state = true;

            Mediator.NotifyColleagues("EnableDockableWindowIsogeo", false);
            try
            {
                var newToken = await SetConnection(Variables.configurationManager.config.userAuthentication.id,
                    RijndaelManagedEncryption.DecryptRijndael(
                        Variables.configurationManager.config.userAuthentication.secret, Variables.encryptCode));
                if (!(string.IsNullOrEmpty(newToken?.access_token) || newToken.StatusResult != "OK"))
                {
                    Variables.search = await
                        SearchRequest(new ApiParameters(newToken, query, offset: offset, ob: /*Variables.cmbSortingMethodSelectedItem?.Id*/ob, 
                            od: /*Variables.cmbSortingDirectionSelectedItem?.Id*/od, box: box, rel: GetRelRequest()), nbResult);
                    //SetSearchList(query);
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
        public async Task<Result> GetDetails(string mdId)
        {
            Log.Logger.Info("GetDetails - md_id : " + mdId);

            Mediator.NotifyColleagues("EnableDockableWindowIsogeo", false);
            Result result = null;
            try
            {
                var newToken = await SetConnection(Variables.configurationManager.config.userAuthentication.id,
                    RijndaelManagedEncryption.DecryptRijndael(
                        Variables.configurationManager.config.userAuthentication.secret, Variables.encryptCode));
                if (!(string.IsNullOrEmpty(newToken?.access_token) || newToken.StatusResult != "OK"))
                { 
                    result = await ApiDetailsResourceRequest(mdId, new ApiParameters(newToken));
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
        private async Task<Token> GetAccessToken(string clientId, string clientSecret)
        {
            Log.Logger.Debug("Ask for Token - cliendId : " + clientId);
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            Token newToken = null;
            var form = new Dictionary<string, string>
            {
                {"grant_type", "client_credentials"}
            };

            try
            {
                var clientCredentials = Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}");
                var url = Variables.configurationManager.config.apiIdUrl + "oauth/token";
                var authenticationHeader = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(clientCredentials));
                _client.DefaultRequestHeaders.Authorization = authenticationHeader;
                var response = await _client.PostAsync(url, new FormUrlEncodedContent(form));

                
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    newToken = JsonSerializer.Deserialize<Token>(content);
                }

                if (newToken == null)
                    newToken = new Token();
                newToken.StatusResult = response.StatusCode.ToString();
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
        private async Task<Result> ApiDetailsResourceRequest(string mdId, ApiParameters parameters)
        {
            Log.Logger.Info("Execution DetailsResourceRequest - ID : " + mdId);
            try
            {
                var url = Variables.configurationManager.config.apiUrl + "resources/" + mdId;

                var includes = new List<(string, string)>()
                {
                    ("_include", "conditions"),
                    ("_include", "contacts"),
                    ("_include", "coordinate-system"),
                    ("_include", "events"),  
                    ("_include", "feature-attributes"),
                    ("_include", "limitations"),
                    ("_include", "keywords"),
                    ("_include", "specifications"),
                    ( "_lang", CultureInfo.InstalledUICulture.TwoLetterISOLanguageName )
                };

                url += includes.Aggregate("?", (current,
                        element) => current + $"&{element.Item1}={element.Item2}");

                    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", parameters.token.access_token);
                var response = await _client.GetAsync(url);
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<Result>(content);

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

        private async Task<Search> SearchRequest(ApiParameters apiParameters, int nbResult)
        {
            Log.Logger.Info("Execution SearchRequest - query : " + '"' + apiParameters.query + '"' + ", offset : " + apiParameters.offset);
            try
            {
                Variables.search = new Search();

                var url = Variables.configurationManager.config.apiUrl + "resources/search";

                var dictionary = new Dictionary<string, string>
                {
                    { "_include", "serviceLayers" },
                    { "_lang", apiParameters.lang },
                    { "_limit", nbResult.ToString() },
                    { "ob", apiParameters.ob },
                    { "od", apiParameters.od },
                    { "_offset", apiParameters.offset.ToString() },
                    { "rel", apiParameters.rel },
                    { "q", apiParameters.query },
                    { "box", apiParameters.box },
                };

                url += dictionary.Aggregate("?_include=layers", (current, 
                    element) => current + $"&{element.Key}={element.Value}");

                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiParameters.token.access_token);
                var response = await _client.GetAsync(url);

                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Search>(content);
            }
            catch (Exception ex)
            {
                Log.Logger.Error("Error - " + ex.Message);
                return null;
            }
        }
      

        private WebProxy GetProxy()
        {
            Log.Logger.Debug("Initializing Proxy...");
            if (string.IsNullOrEmpty(Variables.configurationManager.config.proxy.proxyUrl)) 
                return null;
            Log.Logger.Info( "Setting Proxy...");
            var proxy = new WebProxy(Variables.configurationManager.config.proxy.proxyUrl, false);
            if (!string.IsNullOrEmpty(Variables.configurationManager.config.proxy.proxyUser) && !string.IsNullOrEmpty(Variables.configurationManager.config.proxy.proxyPassword))
            {
                proxy.Credentials = new NetworkCredential(Variables.configurationManager.config.proxy.proxyUser, RijndaelManagedEncryption.DecryptRijndael(
                    Variables.configurationManager.config.proxy.proxyPassword, Variables.encryptCode));
            }

            Log.Logger.Debug("END Initializing Proxy");
        return proxy;
        }

        /// <summary>
        /// Save the last search request realized inside configurationManager
        /// </summary>
        public void SaveSearch(string box, string query)
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
            currentSearch.query = query;
            currentSearch.box = box;
            Variables.configurationManager.Save();
            Mediator.NotifyColleagues("ChangeQuickSearch", null);
            Log.Logger.Info("END Save Last search - Query saved : " + '"' + currentSearch.query + '"');
        }
    }
}
