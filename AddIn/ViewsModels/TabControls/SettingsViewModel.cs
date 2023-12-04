using ArcGIS.Desktop.Framework.Contracts;
using Isogeo.AddIn.Models;
using Isogeo.AddIn.ViewsModels.Settings;
using Isogeo.Map;
using Isogeo.Network;

namespace Isogeo.AddIn.ViewsModels.TabControls
{
    public class SettingsViewModel : ViewModelBase
    {
        public SearchSettingsViewModel SearchSettingsViewModel { get; set; }
        public ProxySettingsViewModel ProxySettingsViewModel { get; set; }
        public AuthenticationSettingsViewModel AuthenticationSettingsViewModel { get; set; }

        public SettingsViewModel(INetworkManager networkManager, FilterManager filterManager, IMapManager mapManager)
        {
            SearchSettingsViewModel = new SearchSettingsViewModel(networkManager, filterManager, mapManager);
            ProxySettingsViewModel = new ProxySettingsViewModel();
            AuthenticationSettingsViewModel = new AuthenticationSettingsViewModel(networkManager);
        }
    }
}
