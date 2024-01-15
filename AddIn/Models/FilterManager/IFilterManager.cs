using Isogeo.AddIn.Models.Filters.Components;
using System;

namespace Isogeo.AddIn.Models.FilterManager
{
    public interface IFilterManager
    {
        public void AddFilters(Filters.Components.Filters filters);

        public void SetTextSearchFilter(SearchTextFilter searchTextFilter);

        public void SetGeographicFilter(Filters.Components.Filters filters);

        public void SetCmbSortingMethod(FilterItem cmbSortingMethodSelectedItem);

        public void SetCmbSortingDirection(FilterItem cmbSortingDirectionSelectedItem);

        public string GetOd();

        public string GetOb();

        public string GetBoxRequest();

        public void AddFunctionToSetFilterList(Action setListFunction);

        public bool FilterListsLoading { get; }

        /// <summary>
        /// Set Filters used by UI with current search lists
        /// </summary>
        /// <param name="cmb"></param>
        /// <param name="listName"></param>
        public void SetListCombo(Filters.Components.Filters cmb, string listName);

        /// <summary>
        /// Get current query chosen by the user (UI)
        /// </summary>
        /// <returns></returns>
        public string GetQueryCombos();
    }
}
