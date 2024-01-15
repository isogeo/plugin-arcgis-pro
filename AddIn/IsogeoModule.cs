using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Framework.Contracts;

namespace Isogeo.AddIn
{
    internal class IsogeoModule : Module
    {
        private static IsogeoModule _this;

        /// <summary>
        /// Retrieve the singleton instance to this module here
        /// </summary>
        public static IsogeoModule Current => _this ??= (IsogeoModule)FrameworkApplication.FindModule("Arcgis_Pro_Isogeo_Module");

        #region Overrides
        /// <summary>
        /// Called by Framework when ArcGIS Pro is closing
        /// </summary>
        /// <returns>False to prevent Pro from closing, otherwise True</returns>
        protected override bool CanUnload()
        {
            //add your business logic
            //return false to ~cancel~ Application close
            return true;
        }

        #endregion Overrides

    }
}
