using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Forms;
using ArcMapAddinIsogeo;
using MouseEventArgs = System.Windows.Input.MouseEventArgs;
using UserControl = System.Windows.Controls.UserControl;
using API = ArcMapAddinIsogeo.API;


namespace Arcgis_Pro_Isogeo
{
    /// <summary>
    /// Interaction logic for DockpaneView.xaml
    /// </summary>
    public partial class DockpaneView : UserControl
    {
        // TODO remplace Timer later
        private Timer _timer;
        private double resultPanelHeight = 0;

        // TODO implementation ArcGis pro later
        // protected IGxApplication app;
        // protected IGxCatalog catalog;

        public DockpaneView()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(DockableWindowIsogeo_Load);
            Variables.dockableWindowIsogeo = this;
            // TODO this.Hook = hook;
            Variables.functionsTranslate.Add(translate);
            init();
           resultPanelHeight = Results.LstResults.Height;

            _timer = new Timer();
            _timer.Interval = 500;
            _timer.Tick += (sender, e) => ResizeFinished();


            // TODO this.app = Internal.AddInStartupObject.GetHook<IGxApplication>() as IGxApplication;
            // TODO this.catalog = this.app.Catalog;


            // TODO IActiveView activeView = (IActiveView)ArcMap.Document.ActiveView.FocusMap;

            // TODO IActiveViewEvents_ItemAddedEventHandler m_ActiveViewEventsItemAdded;
            // TODO IActiveViewEvents_ItemDeletedEventHandler m_ActiveViewEventsItemDeleted;

            //Create an instance of the delegate, add it to ItemAdded event
            // TODO m_ActiveViewEventsItemAdded = new IActiveViewEvents_ItemAddedEventHandler(OnActiveViewEventsItemAdded);
            // TODO ((IActiveViewEvents_Event)(activeView)).ItemAdded += m_ActiveViewEventsItemAdded;

            //Create an instance of the delegate, add it to ItemDeleted event
            // TODO m_ActiveViewEventsItemDeleted = new IActiveViewEvents_ItemDeletedEventHandler(OnActiveViewEventsItemAdded);
            // TODO ((IActiveViewEvents_Event)(activeView)).ItemDeleted += m_ActiveViewEventsItemDeleted;
        }

        private void pLayerEvents_VisibilityChanged(Boolean currentState)
        {
            layersVisibleChange();
        }


        private void OnActiveViewEventsItemAdded(object Item)
        {

            // TODO
           /* if (Item is ILayerEvents_Event)
            {
                ILayerEvents_Event pLayerEvents = Item as ILayerEvents_Event;
                pLayerEvents.VisibilityChanged += new ILayerEvents_VisibilityChangedEventHandler(pLayerEvents_VisibilityChanged);
            }*/


            layersVisibleChange();


        }


        private void layersVisibleChange()
        {
            //TODO Variables.layersVisible = new List<ILayer>();
            //TODO IActiveView activeView = (IActiveView)ArcMap.Document.ActiveView.FocusMap;

            /*TODO var mapLayers = ((IMap)activeView).Layers;

            for (var layer = mapLayers.Next(); layer != null; layer = mapLayers.Next())
            {
                addGroupLayerVisibility(layer);
            }*/
        }

        //TODO
        /*private void addGroupLayerVisibility(ILayer layer)
        {

            var comLayer = layer as ICompositeLayer;
            var groupLayer = layer as IGroupLayer;
            if (layer.Visible == true)
            {
                if ((comLayer != null) && (groupLayer != null))
                {

                    addGroupLayerVisibility(layer);

                }
                else
                {
                    Variables.layersVisible.Add(layer);
                }
            }
        }*/



        private void DockableWindowIsogeo_Load(object sender, EventArgs e)
        {
            Variables.restFunctions = new API.RestFunctions();
            if (Variables.configurationManager.config.query == "-") Variables.configurationManager.config.query = " ";
            Variables.restFunctions.reloadinfosAPI("", 0, false);
            Variables.restFunctions.reloadinfosAPI(Variables.configurationManager.config.query, 0, false);
            if (Variables.configurationManager.config.owner == null) Variables.configurationManager.config.owner = "";
            SearchSettings.CmbOwnerProperty.SelectedValue = Variables.configurationManager.config.owner;
        }

        private void translate()
        {
            TabiSearch.Header = Variables.localisationManager.getValue(ArcMapAddinIsogeo.Localization.LocalizationItem.Search);
            TabiSettings.Header = Variables.localisationManager.getValue(ArcMapAddinIsogeo.Localization.LocalizationItem.Settings);
        }

        private void getConfiguration()
        {
            Variables.configurationManager = new ArcMapAddinIsogeo.Configuration.ConfigurationManager();
            Variables.localisationManager = new ArcMapAddinIsogeo.Localization.LocalizationManager();
            if (Variables.configurationManager.config.query == null) Variables.configurationManager.config.query = "action:view";
            if (Variables.configurationManager.config.query == "" || Variables.configurationManager.config.query == " ") Variables.configurationManager.config.query = "action:view";
            Variables.localisationManager.translatesAll();
            // TODO ProxySettings.setValues();
            // TODO SearchSettings.setValues();
            ResultsToolBar.setSortingDefault();
            MenuSearchManagment.setValues();
        }

        private void init()
        {
            // AdvancedSearch.setHeight();
            getConfiguration();
            Variables.functionsTranslate.Add(translate);
            Variables.localisationManager = new ArcMapAddinIsogeo.Localization.LocalizationManager();
            Variables.localisationManager.translatesAll();

        }
        /// <summary>
        /// Host object of the dockable window
        /// </summary>
        private object Hook
        {
            get;
            set;
        }

        private void DockableWindowIsogeo_SizeChanged(object sender, EventArgs e)
        {
            //Variables.restFunctions.reloadinfosAPI("", 0, true);
            //reultsPanel1.clear();
            //base.OnSizeChanged(e);
            _timer.Start();
        }

        private void DockableWindowIsogeo_Resize(object sender, EventArgs e)
        {
            //Variables.restFunctions.reloadinfosAPI("", 0, true);
        }

        private void DockableWindowIsogeo_MouseUp(object sender, MouseEventArgs e)
        {

        }

        private void ResizeFinished()
        {
            _timer.Stop();

            // Your code
            if (resultPanelHeight != Results.LstResults.Height && Variables.haveResult == true) Variables.restFunctions.reloadinfosAPI("", 0, true);

        }

    }
}
