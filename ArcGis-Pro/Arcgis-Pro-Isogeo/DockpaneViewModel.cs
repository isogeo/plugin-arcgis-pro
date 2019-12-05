using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Framework.Contracts;

namespace Arcgis_Pro_Isogeo
{
    public class DockpaneViewModel : DockPane
    {
        private const string _dockPaneID = "Arcgis_Pro_Isogeo_Dockpane";

        protected DockpaneViewModel() { }

        /// <summary>
        /// Show the DockPane.
        /// </summary>
        internal static void Show()
        {
            DockPane pane = FrameworkApplication.DockPaneManager.Find(_dockPaneID);
            if (pane == null)
                return;

            pane.Activate();
        }

        /// <summary>
        /// Text shown near the top of the DockPane.
        /// </summary>
        private string _heading = "Isogeo";
        public string Heading
        {
            get { return _heading; }
            set
            {
                SetProperty(ref _heading, value, () => Heading);
            }
        }
    }

    /// <summary>
    /// Button implementation to show the DockPane.
    /// </summary>
    internal class Dockpane_ShowButton : Button
    {
        protected override void OnClick()
        {
            DockpaneViewModel.Show();
        }
    }
}
