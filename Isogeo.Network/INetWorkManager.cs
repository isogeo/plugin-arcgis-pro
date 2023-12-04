using Isogeo.Models.API;

namespace Isogeo.Network
{
    public interface INetworkManager
    {
        public void OpenAuthenticationPopUp();

        public Task ResetData(string box, string od, string ob);

        public Task LoadData(string query, int offset, string box, string od, string ob);

        // Doesn't send a signal by mediator to prevent box change (& so no camera change)
        public Task ReloadData(int offset, string comboQuery, string box, string od, string ob);

        /// <summary>
        /// Get Metadata details from API, Window is locked during the process. If token is invalid, authentication pop-up is opened
        /// </summary>
        /// <param name="mdId">Metadata id from API</param>
        public Task<Result> GetDetails(string mdId);

        /// <summary>
        /// Save the last search request realized inside configurationManager
        /// </summary>
        public void SaveSearch(string box, string query);

        internal Task<Token> SetConnection(string clientId, string clientSecret);
    }
}
