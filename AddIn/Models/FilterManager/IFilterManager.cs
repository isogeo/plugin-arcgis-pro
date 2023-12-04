using Isogeo.AddIn.Models.Filters.Components;

namespace Isogeo.AddIn.Models.FilterManager
{
    public interface IFilterManager
    {
        public void AddFilters(Filters.Components.Filters filters);

        public void SetGeographicFilter(Filters.Components.Filters filters);

        public void SetCmbSortingMethod(FilterItem cmbSortingMethodSelectedItem);

        public void SetCmbSortingDirection(FilterItem cmbSortingDirectionSelectedItem);

        public string GetOd();

        public string GetOb();

        public string GetBoxRequest();

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

        /// <summary>
        /// Set current search lists with query parameters
        /// Set at the end of the method combobox' sourceItems
        /// </summary>
        /// <param name="query">query is used to define current search lists</param>
        public void SetSearchList(string query);
    }
}
