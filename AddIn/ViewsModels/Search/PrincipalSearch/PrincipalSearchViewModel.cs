﻿using Isogeo.AddIn.Models.FilterManager;
using Isogeo.Map;
using Isogeo.Network;
using Isogeo.Utils.ConfigurationManager;
using MVVMPattern;

namespace Isogeo.AddIn.ViewsModels.Search.PrincipalSearch
{
    public class PrincipalSearchViewModel : ViewModelBase
    {
        public QuickSearchViewModel QuickSearchViewModel { get; set; }
        public SearchBarViewModel SearchBarViewModel { get; set; }

        public PrincipalSearchViewModel(IFilterManager filterManager, INetworkManager networkManager, IMapManager mapManager,
            IConfigurationManager configurationManager)
        {
            QuickSearchViewModel = new QuickSearchViewModel(networkManager, filterManager, mapManager, configurationManager);
            SearchBarViewModel = new SearchBarViewModel(networkManager, filterManager);
        }
    }
}
