using System.Threading.Tasks;
using Isogeo.AddIn.Models;
using Isogeo.Models;
using MVVMPattern;
using MVVMPattern.MediatorPattern;
using System;
using System.Collections.Generic;
using System.Linq;
using Isogeo.Models.Configuration;
using Isogeo.Network;
using Isogeo.AddIn.Models.FilterManager;

namespace Isogeo.AddIn.ViewsModels.Search.PrincipalSearch
{
    public class SearchBarViewModel : ViewModelBase
    {
        private readonly INetworkManager _networkManager;
        private readonly IFilterManager _filterManager;

        private readonly IEnumerable<SearchList> _searchLists = new List<SearchList>()
        {
            new("type", true),
            new("keyword:inspire-theme", true),
            new("keyword:isogeo", true),
            new("format", true),
            new("coordinate-system", true),
            new("owner", true),
            new("action", true),
            new("contact", true),
            new("license", true) 
        };

        public string SearchText
        {
            get => Variables.searchText;
            set
            {
                Variables.searchText = value;
                OnPropertyChanged(nameof(SearchText));
            }
        }

        public string GetTextBarQuery(string query)
        {
          
            var textInput = "";
           

            if (Variables.search?.Tags == null) 
                return textInput;

            if (!string.IsNullOrWhiteSpace(query))
            {
                var queryItems = query.Split(' ');

                foreach (var queryItem in queryItems)
                {
                    var find = false;
                    if (queryItem.Trim() != "")
                    {
                        if (_searchLists.Any(lst => queryItem.IndexOf(lst.filter + ":", StringComparison.Ordinal) == 0))
                        {
                            find = true;
                        }
                    }

                    if (find) 
                        continue;
                    if (textInput != "") 
                        textInput += " ";
                    textInput += queryItem;
                }

            }

            //if (string.IsNullOrWhiteSpace(query))
            //{
            //    Variables.searchText = textInput;
            //}

            return textInput;
        }

        private void ChangeSearchTextEvent(object queryItem)
        {
            var query = ((QueryItem)queryItem).Query;
            SearchText = GetTextBarQuery(query);
            //SearchText = Variables.searchText;
        }

        public SearchBarViewModel(INetworkManager networkManager, IFilterManager filterManager)
        {
            _networkManager = networkManager;
            _filterManager = filterManager;
            Mediator.Register("ChangeQuery", ChangeSearchTextEvent);
        }

        public async Task Search()
        {
            var ob = _filterManager.GetOb();
            var od = _filterManager.GetOd();
            var query = _filterManager.GetQueryCombos();
            var box = _filterManager.GetBoxRequest();
            await _networkManager.LoadData(query, 0, box, od, ob);
            _filterManager.SetSearchList(query);
        }
    }
}
