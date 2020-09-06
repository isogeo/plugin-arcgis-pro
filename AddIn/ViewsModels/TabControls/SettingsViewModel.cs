using ArcGIS.Desktop.Framework.Contracts;
using Isogeo.AddIn.ViewsModels.Settings;

namespace Isogeo.AddIn.ViewsModels.TabControls
{
    public class SettingsViewModel : ViewModelBase
    {
        public SearchSettingsViewModel SearchSettingsViewModel { get; set; }
        public ProxySettingsViewModel ProxySettingsViewModel { get; set; }

        public SettingsViewModel()
        {
            SearchSettingsViewModel = new SearchSettingsViewModel();
            ProxySettingsViewModel = new ProxySettingsViewModel();
        }
    }
}
