using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Windows;
using Isogeo.Models;
using Isogeo.Models.API;
using Isogeo.Utils.Configuration;
using Isogeo.Utils.ConfigurationManager;
using Isogeo.Utils.LogManager;
using Isogeo.Utils.ManageEncrypt;
using MVVMPattern.MediatorPattern;
using MessageBox = ArcGIS.Desktop.Framework.Dialogs.MessageBox;
using Resource = Isogeo.Language.Resources;
using Search = Isogeo.Models.API.Search;

namespace Isogeo.Network
{
    public class NetworkManager : INetworkManager
    {
        private readonly IConfigurationManager _configurationManager;

        private Models.Network.Authentication.Authentication _frmAuthentication;

        private ApiBearerToken? _existingApiBearerToken;

        private bool AuthenticationPopUpIsOpen => _frmAuthentication != null && _frmAuthentication.IsLoaded;

        private static bool _isFirstLoad = true;
        private static bool _isFirstUserRequest = true;

        private readonly HttpClient _client;

        public NetworkManager(IConfigurationManager configurationManager)
        {
            _configurationManager = configurationManager;
            var proxy = GetProxy();
            var httpClientHandler = new HttpClientHandler()
            {
                Proxy = proxy
            };
            _client = new HttpClient(httpClientHandler);
        }

        public async Task<Token> SetConnection(string clientId, string clientSecret)
        {
            Token newToken;

            Log.Logger.Info("Set Connection : " + clientId);
            if (!string.IsNullOrEmpty(clientSecret))
            {
                if (_existingApiBearerToken == null || _existingApiBearerToken.ExpirationDate < DateTimeOffset.UtcNow)
                {
                    newToken = await GetAccessToken(clientId, clientSecret);
                    _existingApiBearerToken = new ApiBearerToken(newToken);
                }
                else
                {
                    newToken = new Token() { AccessToken = _existingApiBearerToken.AccessToken, ExpiresIn = 1, StatusResult = "OK", TokenType = "Bearer" };
                }
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
            _frmAuthentication = new Models.Network.Authentication.Authentication(this, _configurationManager);
            _frmAuthentication.ShowDialog();
        }

        private async Task<(bool, string)> CheckFirstRequestThenTokenThenSearchRequest(string query, int offset, string box, string od, string ob)
        {
            if (_isFirstUserRequest)
            {
                _isFirstUserRequest = false;
                query = _configurationManager.Config.DefaultSearch;
                var state = await TokenThenSearchRequest(query, offset, _configurationManager.GlobalSoftwareSettings.NbResult, box,  od, ob);
                if (!state)
                    _isFirstUserRequest = true;
                return (state, query);
            }
            return ((await TokenThenSearchRequest(query, offset, _configurationManager.GlobalSoftwareSettings.NbResult, box, od, ob)), query);
        }

        public async Task ResetData(string od, string ob)
        {
            Log.Logger.Debug("Execute Reset Data");
            const string query = "";
            Mediator.NotifyColleagues(MediatorEvent.ChangeBox, "");
            var response = await CheckFirstRequestThenTokenThenSearchRequest(query, 0, "", od, ob);
            Mediator.NotifyColleagues(MediatorEvent.ChangeQuery, new QueryItem { Query = response.Item2 });
            Mediator.NotifyColleagues(MediatorEvent.ClearResults, null);
            if (!response.Item1)
                Application.Current.Dispatcher.Invoke(OpenAuthenticationPopUp);
        }

        public async Task<Search?> GetDataWithoutMediatorEventRaised(string query, int offset, string box, string od, string ob,
            CancellationToken token = default)
        {
            var nbResult = _configurationManager.GlobalSoftwareSettings.NbResult;
            try
            {
                var newToken = await SetConnection(_configurationManager.Config.UserAuthentication.Id,
                    RijndaelManagedEncryption.DecryptRijndael(
                        _configurationManager.Config.UserAuthentication.Secret,
                        _configurationManager.GlobalSoftwareSettings.EncryptCode));
                if (!(string.IsNullOrEmpty(newToken?.AccessToken) || newToken.StatusResult != "OK"))
                {
                    return await
                        SearchRequest(new ApiParameters(newToken, query, offset: offset, ob: ob,
                            od: od, box: box, rel: GetRelRequest()), nbResult, true, token);
                }
                return null;
            }
            catch (Exception ex)
            {
                Log.Logger.Error("Error Authentication : " + ex.Message);
                return null;
            }
        }

        public async Task ChangeOffset(string query, int offset, string box, string od, string ob)
        {
            Log.Logger.Debug("Execute ChangeOffset network");
            if (!string.IsNullOrWhiteSpace(box))
                Mediator.NotifyColleagues(MediatorEvent.ChangeBox, box);
            var response = await CheckFirstRequestThenTokenThenSearchRequest(query, offset, box, od, ob);
            if (response.Item1)
                Mediator.NotifyColleagues(MediatorEvent.ChangeOffset, offset);
            else
            {
                Mediator.NotifyColleagues(MediatorEvent.ClearResults, offset);
                Application.Current.Dispatcher.Invoke(OpenAuthenticationPopUp);
            }
        }

        public async Task LoadData(string query, int offset, string box, string od, string ob)
        {
            Log.Logger.Debug("Execute Load Data");
            Mediator.NotifyColleagues(MediatorEvent.ChangeBox, box);
            var response = await CheckFirstRequestThenTokenThenSearchRequest(query, offset, box, od, ob);
            Mediator.NotifyColleagues(MediatorEvent.ChangeQuery, new QueryItem { Query = response.Item2 });
            if (response.Item1)
                Mediator.NotifyColleagues(MediatorEvent.ChangeOffset, offset);
            else
            {
                Mediator.NotifyColleagues(MediatorEvent.ClearResults, offset);
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

            Mediator.NotifyColleagues(MediatorEvent.EnableDockableWindowIsogeo, false);
            try
            {
                var newToken = await SetConnection(_configurationManager.Config.UserAuthentication.Id,
                    RijndaelManagedEncryption.DecryptRijndael(
                        _configurationManager.Config.UserAuthentication.Secret, 
                        _configurationManager.GlobalSoftwareSettings.EncryptCode));
                if (!(string.IsNullOrEmpty(newToken?.AccessToken) || newToken.StatusResult != "OK"))
                {
                    Variables.search = await
                        SearchRequest(new ApiParameters(newToken, query, offset: offset, ob: ob, 
                            od: od, box: box, rel: GetRelRequest()), nbResult);
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
                Mediator.NotifyColleagues(MediatorEvent.EnableDockableWindowIsogeo, true);
            }

            return state;
        }

        /// <summary>
        /// Get Metadata details from API, Window is locked during the process. If token is invalid, authentication pop-up is opened
        /// </summary>
        /// <param name="mdId">Metadata id from API</param>
        /// <param name="cancellationToken"></param>
        public async Task<Result> GetDetails(string mdId, CancellationToken cancellationToken = default)
        {
            Log.Logger.Info("GetDetails - md_id : " + mdId);

            Mediator.NotifyColleagues(MediatorEvent.EnableDockableWindowIsogeo, false);
            Result result = null;
            try
            {
                var newApiToken = await SetConnection(_configurationManager.Config.UserAuthentication.Id,
                    RijndaelManagedEncryption.DecryptRijndael(
                        _configurationManager.Config.UserAuthentication.Secret, _configurationManager.GlobalSoftwareSettings.EncryptCode));
                if (!(string.IsNullOrEmpty(newApiToken?.AccessToken) || newApiToken.StatusResult != "OK"))
                { 
                    result = await ApiDetailsResourceRequest(mdId, new ApiParameters(newApiToken), cancellationToken);
                }
            }
            catch (Exception ex)
            {
                Log.Logger.Error("Error Authentication : " + ex.Message);
            }
            finally
            {
                Mediator.NotifyColleagues(MediatorEvent.EnableDockableWindowIsogeo, true);
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
                var url = _configurationManager.Config.ApiIdUrl + "oauth/token";
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
        private async Task<Result?> ApiDetailsResourceRequest(string mdId, ApiParameters parameters, CancellationToken token = default)
        {
            Log.Logger.Info("Execution DetailsResourceRequest - ID : " + mdId);
            try
            {
                var url = _configurationManager.Config.ApiUrl + "resources/" + mdId;

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

                    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", parameters.token.AccessToken);
                var response = await _client.GetAsync(url, token);
                if (!response.IsSuccessStatusCode)
                    return null;

                var content = await response.Content.ReadAsStringAsync(token);
                var result = JsonSerializer.Deserialize<Result>(content);

                result.TagsLists = new Tags(result);
                return result;
            }
            catch (Exception ex)
            {
                Log.Logger.Error("Error - " + ex.Message);
            }
            return null;
        }

        private string GetRelRequest()
        {
            return !string.IsNullOrWhiteSpace(_configurationManager.Config.GeographicalOperator) ? _configurationManager.Config.GeographicalOperator : "";
        }

        private static Dictionary<string, string> RemoveUnusedParameters(Dictionary<string, string> dictionary)
        {
            var result = dictionary.Where(x => !(string.IsNullOrWhiteSpace(x.Value) || x.Value == "0"))
                .ToDictionary(x => x.Key, x => x.Value);
            return result;
        }

        private async Task<Search?> SearchRequest(ApiParameters apiParameters, int nbResult,
            bool includeLayers = false, CancellationToken token = default)
        {
            Log.Logger.Info("Execution SearchRequest - query : " + '"' + apiParameters.query + '"' + ", offset : " + apiParameters.offset);
            try
            {
                var url = _configurationManager.Config.ApiUrl + "resources/search";

                var dictionary = new Dictionary<string, string>
                {
                    { "_lang", apiParameters.lang },
                    { "_limit", nbResult.ToString() },
                    { "ob", apiParameters.ob },
                    { "od", apiParameters.od },
                    { "_offset", apiParameters.offset.ToString() },
                    { "q", apiParameters.query },
                    { "box", apiParameters.box },
                };

                // perf optimization : Isogeo API
                dictionary = RemoveUnusedParameters(dictionary);
                if (!string.IsNullOrWhiteSpace(apiParameters.box))
                    dictionary.Add("rel", apiParameters.rel);

                url += dictionary.Aggregate(
                    includeLayers ? "?_include=serviceLayers&_include=layers" : "?", (current, 
                    element) => current + $"&{element.Key}={element.Value}");

                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiParameters.token.AccessToken);
                var response = await _client.GetAsync(url, token);
                if (!response.IsSuccessStatusCode)
                    return null;

                var content = await response.Content.ReadAsStringAsync(token);
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
            if (string.IsNullOrEmpty(_configurationManager.Config.Proxy.ProxyUrl)) 
                return null;
            Log.Logger.Info( "Setting Proxy...");
            var proxy = new WebProxy(_configurationManager.Config.Proxy.ProxyUrl, false);
            if (!string.IsNullOrEmpty(_configurationManager.Config.Proxy.ProxyUser) && !string.IsNullOrEmpty(_configurationManager.Config.Proxy.ProxyPassword))
            {
                proxy.Credentials = new NetworkCredential(_configurationManager.Config.Proxy.ProxyUser, RijndaelManagedEncryption.DecryptRijndael(
                    _configurationManager.Config.Proxy.ProxyPassword, _configurationManager.GlobalSoftwareSettings.EncryptCode));
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
            Utils.Configuration.Search currentSearch = null;
            if (_configurationManager?.Config?.Searchs?.SearchDetails == null)
                return;
            foreach (var search in _configurationManager.Config.Searchs.SearchDetails)
            {
                if (search.Name == Resource.Previous_search)
                {
                    currentSearch = search;
                    break;
                }
            }
            if (currentSearch == null) {
                currentSearch = new Utils.Configuration.Search {Name = Resource.Previous_search};

                _configurationManager.Config.Searchs.SearchDetails.Add(currentSearch);
            }
            currentSearch.Query = query;
            currentSearch.Box = box;
            _configurationManager.Save();
            Mediator.NotifyColleagues(MediatorEvent.ChangeQuickSearch, null);
            Log.Logger.Info("END Save Last search - Query saved : " + '"' + currentSearch.Query + '"');
        }
    }
}
