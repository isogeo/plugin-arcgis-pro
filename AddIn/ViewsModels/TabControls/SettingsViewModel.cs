using ArcGIS.Desktop.Framework.Contracts;
using Isogeo.AddIn.Models;
using Isogeo.AddIn.ViewsModels.Settings;
using Isogeo.Map.MapFunctions;
using Isogeo.Models.Network;

namespace Isogeo.AddIn.ViewsModels.TabControls
{
    public class SettingsViewModel : ViewModelBase
    {
        public SearchSettingsViewModel SearchSettingsViewModel { get; set; }
        public ProxySettingsViewModel ProxySettingsViewModel { get; set; }
        public AuthenticationSettingsViewModel AuthenticationSettingsViewModel { get; set; }

        public SettingsViewModel(RestFunctions restFunctions, FilterManager filterManager, IMapFunctions mapFunctions)
        {
            SearchSettingsViewModel = new SearchSettingsViewModel(restFunctions, filterManager, mapFunctions);
            ProxySettingsViewModel = new ProxySettingsViewModel();
            AuthenticationSettingsViewModel = new AuthenticationSettingsViewModel(restFunctions);
        }
    }
}
