using Isogeo.Models.API;

namespace Isogeo.Network
{
    public interface INetworkManager
    {
        public void OpenAuthenticationPopUp();

        public Task ResetData(string od, string ob);

        public Task LoadData(string query, int offset, string box, string od, string ob);

        public Task<Search?> GetDataWithoutMediatorEventRaised(string query, int offset, string box, string od, string ob, CancellationToken token = default);

        /// <summary>
        /// Change only offset (no change box or change query)
        /// </summary>
        public Task ChangeOffset(string query, int offset, string box, string od, string ob);

        /// <summary>
        /// Get Metadata details from API, Window is locked during the process. If token is invalid, authentication pop-up is opened
        /// </summary>
        /// <param name="mdId">Metadata id from API</param>
        /// <param name="token"></param>
        public Task<Result> GetDetails(string mdId, CancellationToken token = default);

        /// <summary>
        /// Save the last search request realized inside configurationManager
        /// </summary>
        public void SaveSearch(string box, string query);

        internal Task<Token> SetConnection(string clientId, string clientSecret);
    }
}
