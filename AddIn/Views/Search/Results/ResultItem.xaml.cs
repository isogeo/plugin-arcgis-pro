using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using Isogeo.AddIn.ViewsModels.Metadata;
using Isogeo.Map.DataType;
using Isogeo.Map.MapFunctions;
using Isogeo.Models;
using Isogeo.Models.API;
using Isogeo.Models.Network;
using Isogeo.Utils.LogManager;
using MVVMPattern.MediatorPattern;
using RelayCommand = MVVMPattern.RelayCommand.RelayCommand;

namespace Isogeo.AddIn.Views.Search.Results
{
    public partial class ResultItem
    {
        public Result result;
        // Isogeo geometry types
        private readonly List<string> _polygonList = new List<string>(new[] { "CurvePolygon", "MultiPolygon", "MultiSurface", "Polygon", "PolyhedralSurface" });
        private readonly List<string> _pointList = new List<string>(new[] { "Point", "MultiPoint" });
        private readonly List<string> _lineList = new List<string>(new[] { "CircularString", "CompoundCurve", "Curve", "LineString", "MultiCurve", "MultiLineString" });
        private readonly List<string> _multiList = new List<string>(new[] { "Geometry", "GeometryCollection" });

        //Isogeo formats
        private readonly List<string> _vectorFormatList = new List<string>(new[] { "shp", "dxf", "dgn", "filegdb", "tab", "arcsde" });
        private readonly List<string> _rasterFormatList = new List<string>(new[] { "esriasciigrid", "geotiff", "intergraphgdb", "jpeg", "png", "xyz", "ecw" });

        private readonly List<ServiceType> _dataList = new List<ServiceType>();

        public ICommand btnMenuOnClick;

        private Metadata.Metadata _metadataInstance;
        private MetadataViewModel _metadataViewModel;

        private readonly IMapFunctions _mapFunctions;
        private readonly RestFunctions _restFunctions;

        private void InitResources()
        {
            var resources = Resources;
            resources.BeginInit();

            if (FrameworkApplication.ApplicationTheme == ApplicationTheme.Dark ||
                FrameworkApplication.ApplicationTheme == ApplicationTheme.HighContrast)
            {
                resources.MergedDictionaries.Add(
                    new ResourceDictionary
                    {
                        Source = new Uri("pack://application:,,,/Isogeo.Resources;component/Themes/DarkTheme.xaml")
                    });
            }
            else
            {
                resources.MergedDictionaries.Add(
                    new ResourceDictionary
                    {
                        Source = new Uri("pack://application:,,,/Isogeo.Resources;component/Themes/LightTheme.xaml")
                    });
            }
            resources.EndInit();
        }

        internal bool IsInDesignMode =>
            (bool)DependencyPropertyDescriptor.FromProperty(
                DesignerProperties.IsInDesignModeProperty, typeof(DependencyObject)).Metadata.DefaultValue;

        public ResultItem(IMapFunctions mapFunctions, RestFunctions restFunctions)
        {
            InitializeComponent();
            _mapFunctions = mapFunctions;
            _restFunctions = restFunctions;
            DataContext = this;
            if (!IsInDesignMode)
                InitResources();
        }

        public void Init(Result item)
        {
            result = item;
            SetComponent();
            GetLinks();
            SetCombo();
        }

        public ICommand BtnMenuOnClick
        {
            get
            {
                return btnMenuOnClick ?? (btnMenuOnClick = new RelayCommand(
                    x => BtnMenu_OnClickEvent(),
                    y => CanRunBtnMenu_OnClick()));
            }
        }

        private bool CanRunBtnMenu_OnClick()
        {
            return CmbLayer != null && CmbLayer.IsVisible && CmbLayer.IsEnabled && CmbLayer.Items.Count > 0;
        }

        private void SetComponent()
        {
            ImgType.Content = Isogeo.Language.Resources.Unknown_geometry;
            if (result.geometry != null)
            {
                // ReturnPicture("pack://application:,,,/Isogeo.Resources;component/Resources/gavel.png");
                if (_polygonList.Contains(result.geometry)) ImgType.Content = Isogeo.Language.Resources.Polygon;
                    if (_pointList.Contains(result.geometry)) ImgType.Content = Isogeo.Language.Resources.Point;
                    if (_lineList.Contains(result.geometry)) ImgType.Content = Isogeo.Language.Resources.Line;
                    if (_multiList.Contains(result.geometry)) ImgType.Content = Isogeo.Language.Resources.MultiPolygon;
            }
            else
            {
                if (result.type == "rasterDataset")
                {
                    ImgType.Content = Isogeo.Language.Resources.Raster;
                }
                else if (result.type == "service")
                {
                    ImgType.Content = Isogeo.Language.Resources.Service;
                }
                else
                {
                    ImgType.Content = Isogeo.Language.Resources.Unknown_geometry;
                }
            }
            TxtTitle.Text = result.title;
            var toolTip = new ToolTip {Content = result.@abstract};
            TxtTitle.ToolTip = toolTip;
        }

        private void SetCombo()
        {
            CmbLayer.Visibility = Visibility.Visible;

            // get useful metadata
            //added. The "Add" column has to be filled accordingly.
            //If the data can't be added, just insert "can't" text.
            switch (_dataList.Count)
            {
                case 0:
                    CmbLayer.Items.Add(Isogeo.Language.Resources.Cant_be_added);
                    CmbLayer.SelectedIndex = 0;
                    CmbLayer.IsEnabled = false;
                    break;
                case 1:
                    if (_dataList != null && _dataList.Count > 0)
                        CmbLayer?.Items.Add(_dataList[0].title);
                    else
                        CmbLayer?.Items.Add(Isogeo.Language.Resources.Empty);
                    if (CmbLayer != null)
                    {
                        CmbLayer.Visibility = Visibility.Visible;
                        CmbLayer.SelectedIndex = 0;
                    }
                    break;
                default:
                    //If there is only one way for the data to be added, insert a label.
                    //Else, add a combobox, storing all possibilities.                    

                    CmbLayer.Visibility = Visibility.Visible;

                    foreach (var data in _dataList)
                    {
                        var type = Isogeo.Language.Resources.Empty;
                        var title = Isogeo.Language.Resources.Empty;
                        if (data?.type != null)
                            type = data.type;
                        if (data?.title != null)
                            title = data.title;
                        CmbLayer.Items.Add(type + " - " + title);
                    }
                    CmbLayer.SelectedIndex = 0;
                    break;
            }

        }

        private ServiceType CreateServiceType(Result item)
        {
            var type = " ";
            var title = " ";
            var url = " ";
            var name = " ";
            var creator = " ";
            var id = " ";

            if (item?.format != null)
                type = item.format;
            if (item?.title != null)
                title = item.title;
            if (item?.path != null)
                url = item.path;
            if (item?.name != null)
                name = item.name;
            if (item?._creator?._id != null)
                creator = item._creator._id;
            if (item?._id != null)
                id = item._id;

            return new ServiceType(type, title, url, name, creator, id);

        }

        private ServiceType CreateServiceType(Result itemResult, ServiceLayer item)
        {
            var type = " ";
            var title = " ";
            var url = " ";
            var name = " ";
            var creator = " ";
            var id = " ";

            if (item?.service?.format != null)
                type = item.service.format.ToUpper();
            if (item?.titles != null && item.titles.Count > 0 && item.titles[0].value != null)
                title = item.titles[0].value;
            if (item?.service?.path != null)
                url = item.service.path;
            if (item?.id != null && GetId(item.id) != null)
                name = GetId(item.id);
            if (itemResult?._creator?._id != null)
                creator = itemResult._creator._id;
            if (itemResult?._id != null && item != null)
                id = item._id;

            return new ServiceType(type, title, url, name, creator, id);
        }

        private static ServiceType CreateServiceType(Result itemResult, Layer layer)
        {
            var type = " ";
            var title = " ";
            var url = " ";
            var name = " ";
            var creator = " ";
            var id = " ";

            if (itemResult?.format != null)
                type = itemResult.format.ToUpper();
            if (itemResult?.format != null && layer?.id != null && layer.titles != null)
                title = GetTitle(itemResult.format.ToUpper(), layer.id, layer.titles);
            if (itemResult?.path != null)
                url = itemResult.path;
            if (layer?.id != null && GetId(layer.id) != null)
                name = GetId(layer.id);
            if (itemResult?._creator?._id != null)
                creator = itemResult._creator._id;
            if (itemResult?._id != null)
                id = itemResult._id;
            return new ServiceType(type, title, url, name, creator, id);
        }

        private void GetLinks()
        {

            if (_vectorFormatList.Contains(result.format))
            {
                _dataList.Add(CreateServiceType(result));
            }

            if (_rasterFormatList.Contains(result.format))
            {
                var item = CreateServiceType(result);
                item.type = "raster";
                _dataList.Add(item);
            }

            if (result.format == "postgis")
            {

            }

            // This is the new association mode. The layer and service
            //information are stored in the "serviceLayers" include, when
            // associated with a vector or raster data.


            if (result.type == "vectorDataset" || result.type == "rasterDataset")
            {
                foreach (var serviceLayer in result.serviceLayers)
                {
                    _dataList.Add(CreateServiceType(result, serviceLayer));
                }

            }

            // New association mode. For services metadata sheet, the layers
            // are stored in the purposely named include: "layers".
            if (result.type == "service")
            {
                if (result.layers != null)
                {
                    foreach (var layer in result.layers)
                    {
                        _dataList.Add(CreateServiceType(result, layer));
                    }
                }
            }
        }


        private static string GetId(string id)
        {
            var newId = id;
            newId = newId.Replace('{', ' ');
            newId = newId.Trim();
            newId = newId.Replace('}', ':');
            return newId;
        }

        private static string GetTitle(string type, string id, List<Title> titles)
        {
            var layerTitle = id;
            if (type == "WFS")
            {
                layerTitle = Regex.Replace(id, "\\{.*?}", String.Empty);
            }

            try
            {
                if (titles.Count > 0)
                {
                    layerTitle = titles[0].value;
                }
            }
            catch (Exception ex)
            {
                Log.Logger.Debug(string.Concat(new object[]
                {
                    "Error - getTitle - ResultItem",
                    ex.Message
                }));
            }
            if (layerTitle == "") layerTitle = type + " Layer";
            return layerTitle;
        }

        /// <summary>
        /// Define ViewModels of Metadata. 
        /// It will always be the same already instantiated classes used to gain performance.
        /// If _metadataViewModel or _metadataInstance are null, method is stopped.
        /// </summary>
        private void DefineViewModelMetadata()
        {
            if (_metadataInstance == null || _metadataViewModel == null) return;
            _metadataInstance.DataContext = _metadataViewModel;
            _metadataInstance.TabsMetadata.DataContext = _metadataViewModel;
            _metadataInstance.TabsMetadata.MetadataGeneral.DataContext = _metadataViewModel;
            _metadataInstance.TabsMetadata.MetadataContacts.DataContext = _metadataViewModel;
            _metadataInstance.TabsMetadata.MetadataHistory.DataContext = _metadataViewModel;
            _metadataInstance.TabsMetadata.MetadataLicences.DataContext = _metadataViewModel;
            _metadataInstance.TabsMetadata.MetadataGeography.DataContext = _metadataViewModel;
            _metadataInstance.TabsMetadata.MetadataAdvanced.DataContext = _metadataViewModel;
            _metadataInstance.TabsMetadata.MetadataAdvanced.ContactItem.DataContext = _metadataViewModel;
            _metadataInstance.TabsMetadata.MetadataAdvanced.MetadataDetails.DataContext = _metadataViewModel;
            _metadataInstance.TabsMetadata.MetadataGeography.TechnicalInformationItem.DataContext = _metadataViewModel;
            _metadataInstance.TabsMetadata.MetadataHistory.HistoryDataItem.DataContext = _metadataViewModel;
            _metadataInstance.TabsMetadata.MetadataHistory.LastModificationMetaDataItem.DataContext = _metadataViewModel;
            _metadataInstance.TabsMetadata.MetadataContacts.LvwContactItemsList.DataContext = _metadataViewModel;
            _metadataInstance.TabsMetadata.MetadataContacts.LvwOtherContactItemsList.DataContext = _metadataViewModel;
        }

        /// <summary>
        /// Open Metadata pop-up with API result given in parameter. 
        /// Metadata details will be asked from API before load the pop-up.
        /// </summary>
        /// <param name="item">API Metadata result</param>
        public async Task<bool> OpenMetadata(Result item)
        {
            var resultDetails = await _restFunctions.GetDetails(item._id);
            if (resultDetails == null)
                return false;
            item = resultDetails;
            if (_metadataViewModel == null)
            {
                _metadataViewModel = new MetadataViewModel();
            }
            _metadataViewModel.RegisterMediator();
            if (_metadataInstance == null || !_metadataInstance.IsLoaded)
            {
                _metadataInstance = new Metadata.Metadata();
                DefineViewModelMetadata();
                Mediator.NotifyColleagues("CurrentResult", item);

                _metadataInstance.MinWidth = 550;
                _metadataInstance.MinHeight = 400;
            }
            Log.Logger.Info("Open Metadata");
            _metadataInstance.ShowDialog(); 
            _metadataViewModel.UnRegisterMediator();
            return true;
        }

        private async void OpenMetadata(object sender, MouseButtonEventArgs e)
        {
            if (!await OpenMetadata(result))
                _restFunctions.OpenAuthenticationPopUp();
        }

        private async Task BtnMenu_OnClickEvent()
        {
            var resultDetails = await _restFunctions.GetDetails(result._id);
            if (resultDetails != null)
                result = resultDetails;
            MniLoadData_OnClick();
        }

        private void MniLoadData_OnClick()
        {
            if (_dataList == null || _dataList.Count == 0)
                return;
            var currentService = _dataList.Count == 1 ? _dataList[0] : _dataList[CmbLayer.SelectedIndex];

            if (currentService != null && currentService.type?.ToUpper() == "ARCSDE")
                currentService.url = Variables.configurationManager?.config?.fileSde;
            QueuedTask.Run(() =>
            {
                _mapFunctions.AddLayer(currentService);
            });
        }
    }
}
