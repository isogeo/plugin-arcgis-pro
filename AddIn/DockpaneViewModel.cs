using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Framework.Contracts;
using Isogeo.AddIn.Models;
using Isogeo.AddIn.ViewsModels.TabControls;
using Isogeo.Map.MapFunctions;
using Isogeo.Models;
using Isogeo.Models.Network;
using Isogeo.Utils.LogManager;
using MVVMPattern.MediatorPattern;
using Button = ArcGIS.Desktop.Framework.Contracts.Button;
using ConfigurationManager = Isogeo.Models.Configuration.ConfigurationManager;
using MessageBox = ArcGIS.Desktop.Framework.Dialogs.MessageBox;
using TabControl = ArcGIS.Desktop.Framework.Controls.TabControl;

namespace Isogeo.AddIn
{
    public class DockpaneViewModel : DockPane
    {
        private const string MenuId = "Isogeo_plugin_Menu";
        private readonly ViewModelBase _paneH1Vm;
        private readonly ViewModelBase _paneH2Vm;
        private const string DockPaneId = "Arcgis_Pro_Isogeo_Dockpane";

        private bool _isEnabled = true;
        public bool IsEnabled
        {
            get => _isEnabled;
            set
            {
                _isEnabled = value;
                NotifyPropertyChanged();
            }
        }

        public void EnableDockableWindowIsogeo(object obj)
        {
            IsEnabled = (bool) obj;
        }

        private static void InitRestFunctions()
        {
            Log.Logger.Info("Initializing Rest Functions");
            //Variables.restFunctions = new RestFunctions(); // todo
            if (Variables.configurationManager.config.query == "-") 
                Variables.configurationManager.config.query = " ";
        }

        private static void InitConfigurationManager()
        {
            Log.Logger.Info("Initializing Configuration Manager");
            Variables.configurationManager = new ConfigurationManager();
        }

        public void Exception(object sender, FirstChanceExceptionEventArgs e)
        {
            if (e.Exception.TargetSite.DeclaringType != null && e.Exception.TargetSite.DeclaringType.Assembly == Assembly.GetExecutingAssembly())
            {
                Log.Logger.ErrorFormat("Exception Thrown: {0}\n{1}", e.Exception.Message, e.Exception.StackTrace);
            }
        }

        private void InitLog()
        {
            try
            {
                var dllPAth = GetType().Assembly.Location;

                var configPath = dllPAth.Substring(0, dllPAth.LastIndexOf("\\", StringComparison.Ordinal)) + "\\";
                Log.InitializeLogManager(configPath + "log4net.config");
                Log.InitializeLogPath(configPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show(Language.Resources.Error_logManager, "Isogeo");
                MessageBox.Show(ex.StackTrace, "Isogeo");
                MessageBox.Show(ex.Message, "Isogeo");
            }
        }

        protected DockpaneViewModel()
        {
            InitLog();
            Log.Logger.Info("Isogeo ArcGisPro Add-In is opening...");
            Log.Logger.Info("Initializing DockPaneViewModel ...");
            Mediator.Register("EnableDockableWindowIsogeo", EnableDockableWindowIsogeo);
            InitConfigurationManager();
            InitRestFunctions();

            PrimaryMenuList.Add(new TabControl { Text = Language.Resources.Search_word });
            PrimaryMenuList.Add(new TabControl { Text = Language.Resources.Settings });

            IMapFunctions mapFunctions = new MapFunctions();
            var restFunctions = new RestFunctions();
            var filterManager = new FilterManager(mapFunctions);

            _paneH1Vm = new SearchViewModel(restFunctions, filterManager, mapFunctions);
            _paneH2Vm = new SettingsViewModel(restFunctions, filterManager, mapFunctions);
            _selectedPanelHeaderIndex = 0;
            CurrentPage = _paneH1Vm;
            var ob = filterManager.GetOb();
            var od = filterManager.GetOd();
            var box = filterManager.GetBoxRequest();
            Task.Run(() => Application.Current.Dispatcher.Invoke(async () => await restFunctions.ResetData(box, od, ob)));
            Log.Logger.Info("END Initializing DockPaneViewModel");
        }

        /// <summary>
        /// Show the DockPane.
        /// </summary>
        internal static void Show()
        {
            var pane = FrameworkApplication.DockPaneManager.Find(DockPaneId);
            if (pane == null)
                return;

            pane.Activate();
        }

        #region properties

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

        private readonly List<TabControl> _primaryMenuList = new List<TabControl>();
        public List<TabControl> PrimaryMenuList
        {
            get { return _primaryMenuList; }
        }
        private int _selectedPanelHeaderIndex;
        public int SelectedPanelHeaderIndex
        {
            get { return _selectedPanelHeaderIndex; }
            set
            {
                SetProperty(ref _selectedPanelHeaderIndex, value, () => SelectedPanelHeaderIndex);
                if (_selectedPanelHeaderIndex == 0)
                    CurrentPage = _paneH1Vm;
                if (_selectedPanelHeaderIndex == 1)
                    CurrentPage = _paneH2Vm;
            }
        }
        private PropertyChangedBase _currentPage;
        public PropertyChangedBase CurrentPage
        {
            get { return _currentPage; }
            set
            {
                SetProperty(ref _currentPage, value, () => CurrentPage);
            }
        }

        #endregion
        #region Burger Button

        /// <summary>
        /// Tooltip shown when hovering over the burger button.
        /// </summary>
        public string BurgerButtonTooltip
        {
            get { return "Options"; }
        }

        /// <summary>
        /// Menu shown when burger button is clicked.
        /// </summary>
        public ContextMenu BurgerButtonMenu
        {
            get { return FrameworkApplication.CreateContextMenu(MenuId); }
        }
        #endregion
    }

    /// <summary>
    /// Button implementation to show the DockPane.
    /// </summary>
    internal class DockPaneShowButton : Button
    {
        protected override void OnClick()
        {
            DockpaneViewModel.Show();
        }
    }
    internal class PanelIndicatorStaticMenuButton : Button
    {
        protected override void OnClick()
        {
        }
    }
}
