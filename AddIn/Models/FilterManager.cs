﻿using Isogeo.Models.Filters;
using Isogeo.Models;
using Isogeo.Utils.LogManager;
using System;
using System.Collections.Generic;
using System.Linq;
using Isogeo.Models.API;
using Isogeo.Map.MapFunctions;

namespace Isogeo.AddIn.Models
{
    public class FilterManager
    {
        private SearchLists _searchLists;
        private readonly List<Filters> _listComboFilter = new();
        private Filters _geographicFilter;
        private readonly IMapFunctions _mapFunctions;

        private FilterItem _cmbSortingMethodSelectedItem;
        private FilterItem _cmbSortingDirectionSelectedItem;


        public FilterManager(IMapFunctions mapFunctions)
        {
            _mapFunctions = mapFunctions;
        }

        public void AddFilters(Filters filters)
        {
            _listComboFilter.Add(filters);
        }

        public void SetGeographicFilter(Filters filters)
        {
            _geographicFilter = filters;
        }

        public void SetCmbSortingMethod(FilterItem cmbSortingMethodSelectedItem)
        {
            _cmbSortingMethodSelectedItem = cmbSortingMethodSelectedItem;
        }

        public void SetCmbSortingDirection(FilterItem cmbSortingDirectionSelectedItem)
        {
            _cmbSortingDirectionSelectedItem = cmbSortingDirectionSelectedItem;
        }

        public string GetOd()
        {
            return _cmbSortingDirectionSelectedItem?.Id;
        }

        public string GetOb()
        {
            return _cmbSortingMethodSelectedItem?.Id;
        }

        public string GetBoxRequest()
        {
            var box = "";
            if (_geographicFilter.SelectedItem.Name != "-")
                box = _mapFunctions.GetMapExtent();
            return box;
        }

        /// <summary>
        /// Set Filters used by UI with current search lists
        /// </summary>
        /// <param name="cmb"></param>
        /// <param name="listName"></param>
        public void SetListCombo(Filters cmb, string listName)
        {
            Log.Logger.Debug("Set UI Filter " + listName);
            foreach (var lst in _searchLists.list)
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

        /// <summary>
        /// Get current query chosen by the user (UI)
        /// </summary>
        /// <returns></returns>
        public string GetQueryCombos()
        {
            Log.Logger.Debug("Get query from UI");
            var filter = "";

            foreach (var cmb in _listComboFilter)
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
        /// Set current search lists with query parameters
        /// Set at the end of the method combobox' sourceItems
        /// </summary>
        /// <param name="query">query is used to define current search lists</param>
        public void SetSearchList(string query)
        {
            Log.Logger.Debug("Set search List - query : " + query);
            var textInput = "";
            _searchLists = new SearchLists();

            _searchLists.list.Add(new SearchList("type", true));
            _searchLists.list.Add(new SearchList("keyword:inspire-theme", true));
            _searchLists.list.Add(new SearchList("keyword:isogeo", true));
            _searchLists.list.Add(new SearchList("format", true));
            _searchLists.list.Add(new SearchList("coordinate-system", true));
            _searchLists.list.Add(new SearchList("owner", true));
            _searchLists.list.Add(new SearchList("action", true));
            _searchLists.list.Add(new SearchList("contact", true));
            _searchLists.list.Add(new SearchList("license", true));

            if (Variables.search?.tags == null) return;
            foreach (var item in Variables.search.tags)
            {
                var key = item.Key;
                var val = item.Value;
                foreach (var lst in _searchLists.list)
                {
                    if (key.IndexOf(lst.filter, StringComparison.Ordinal) != 0) continue;
                    lst.lstItem.Add(new FilterItem(key, val));
                    break;
                }
            }

            if (!string.IsNullOrWhiteSpace(query))
            {
                var queryItems = query.Split(' ');

                foreach (var lst in _searchLists.list)
                {
                    lst.query = "";
                }

                foreach (var queryItem in queryItems)
                {
                    var find = false;
                    if (queryItem.Trim() != "")
                    {
                        foreach (var lst in _searchLists.list)
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

            foreach (var lst in _searchLists.list)
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
    }
}