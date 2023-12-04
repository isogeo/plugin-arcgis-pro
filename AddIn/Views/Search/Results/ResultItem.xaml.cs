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
using Isogeo.Network;
using Isogeo.Utils.LogManager;
using MVVMPattern.MediatorPattern;
using RelayCommand = MVVMPattern.RelayCommand.RelayCommand;

namespace Isogeo.AddIn.Views.Search.Results
{
    public partial class ResultItem
    {
        private Result _result;

        // Isogeo geometry types
        private readonly List<string> _polygonList = new(new[] { "CurvePolygon", "MultiPolygon", "MultiSurface", "Polygon", "PolyhedralSurface" });
        private readonly List<string> _pointList = new(new[] { "Point", "MultiPoint" });
        private readonly List<string> _lineList = new(new[] { "CircularString", "CompoundCurve", "Curve", "LineString", "MultiCurve", "MultiLineString" });
        private readonly List<string> _multiList = new(new[] { "Geometry", "GeometryCollection" });

        //Isogeo formats
        private readonly List<string> _vectorFormatList = new(new[] { "shp", "dxf", "dgn", "filegdb", "tab", "arcsde" });
        private readonly List<string> _rasterFormatList = new(new[] { "esriasciigrid", "geotiff", "intergraphgdb", "jpeg", "png", "xyz", "ecw" });

        private readonly List<ServiceType> _dataList = new();

        private Metadata.Metadata _metadataInstance;
        private MetadataViewModel _metadataViewModel;

        private readonly IMapFunctions _mapFunctions;
        private readonly INetworkManager _networkManager;

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

        public ResultItem(IMapFunctions mapFunctions, INetworkManager networkManager)
        {
            InitializeComponent();
            _mapFunctions = mapFunctions;
            _networkManager = networkManager;
            DataContext = this;
            if (!IsInDesignMode)
                InitResources();
        }

        public void Init(Result item)
        {
            _result = item;
            SetComponent();
            GetLinks();
            SetCombo();
        }

        private ICommand _btnMenuOnClick;
        public ICommand BtnMenuOnClick
        {
            get
            {
                return _btnMenuOnClick ??= new RelayCommand(
                    x => BtnMenu_OnClickEvent(),
                    y => CanRunBtnMenu_OnClick());
            }
        }

        private async Task BtnMenu_OnClickEvent()
        {
            var resultDetails = await _networkManager.GetDetails(_result.Id);
            if (resultDetails != null)
                _result = resultDetails;
            MniLoadData_OnClick();
        }

        private bool CanRunBtnMenu_OnClick()
        {
            return CmbLayer != null && CmbLayer.IsVisible && CmbLayer.IsEnabled && CmbLayer.Items.Count > 0;
        }

        private void SetComponent()
        {
            ImgType.Content = Isogeo.Language.Resources.Unknown_geometry;
            if (_result.Geometry != null)
            {
                if (_polygonList.Contains(_result.Geometry)) ImgType.Content = Isogeo.Language.Resources.Polygon;
                    if (_pointList.Contains(_result.Geometry)) ImgType.Content = Isogeo.Language.Resources.Point;
                    if (_lineList.Contains(_result.Geometry)) ImgType.Content = Isogeo.Language.Resources.Line;
                    if (_multiList.Contains(_result.Geometry)) ImgType.Content = Isogeo.Language.Resources.MultiPolygon;
            }
            else
            {
                if (_result.Type == "rasterDataset")
                {
                    ImgType.Content = Isogeo.Language.Resources.Raster;
                }
                else if (_result.Type == "service")
                {
                    ImgType.Content = Isogeo.Language.Resources.Service;
                }
                else
                {
                    ImgType.Content = Isogeo.Language.Resources.Unknown_geometry;
                }
            }
            TxtTitle.Text = _result.Title;
            var toolTip = new ToolTip {Content = _result.Abstract};
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
                        CmbLayer?.Items.Add(_dataList[0].Title);
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
                        if (data?.Type != null)
                            type = data.Type;
                        if (data?.Title != null)
                            title = data.Title;
                        CmbLayer.Items.Add(type + " - " + title);
                    }
                    CmbLayer.SelectedIndex = 0;
                    break;
            }

        }

        private static ServiceType CreateServiceType(Result item)
        {
            var type = " ";
            var title = " ";
            var url = " ";
            var name = " ";
            var creator = " ";
            var id = " ";

            if (item?.Format != null)
                type = item.Format;
            if (item?.Title != null)
                title = item.Title;
            if (item?.Path != null)
                url = item.Path;
            if (item?.Name != null)
                name = item.Name;
            if (item?.Creator?.Id != null)
                creator = item.Creator.Id;
            if (item?.Id != null)
                id = item.Id;

            return new ServiceType(type, title, url, name, creator, id);
        }

        private static ServiceType CreateServiceType(Result itemResult, ServiceLayer item)
        {
            var type = " ";
            var title = " ";
            var url = " ";
            var name = " ";
            var creator = " ";
            var id = " ";

            if (item?.Service?.Format != null)
                type = item.Service.Format.ToUpper();
            if (item?.Titles != null && item.Titles.Count > 0 && item.Titles[0].Value != null)
                title = item.Titles[0].Value;
            if (item?.Service?.Path != null)
                url = item.Service.Path;
            if (item?.Name != null && GetId(item.Name) != null)
                name = GetId(item.Name);
            if (itemResult?.Creator?.Id != null)
                creator = itemResult.Creator.Id;
            if (itemResult?.Id != null && item != null)
                id = item.Id;

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

            if (itemResult?.Format != null)
                type = itemResult.Format.ToUpper();
            if (itemResult?.Format != null && layer?.Id != null && layer.Titles != null)
                title = GetTitle(itemResult.Format.ToUpper(), layer.Id, layer.Titles);
            if (itemResult?.Path != null)
                url = itemResult.Path;
            if (layer?.Id != null && GetId(layer.Id) != null)
                name = GetId(layer.Id);
            if (itemResult?.Creator?.Id != null)
                creator = itemResult.Creator.Id;
            if (itemResult?.Id != null)
                id = itemResult.Id;
            return new ServiceType(type, title, url, name, creator, id);
        }

        private void GetLinks()
        {

            if (_vectorFormatList.Contains(_result.Format))
            {
                _dataList.Add(CreateServiceType(_result));
            }

            if (_rasterFormatList.Contains(_result.Format))
            {
                var item = CreateServiceType(_result);
                _dataList.Add(new ServiceType("raster", item.Title, item.Url, item.Name, item.Creator, item.Id));
            }

            if (_result.Format == "postgis")
            {

            }

            // This is the new association mode. The layer and service
            //information are stored in the "serviceLayers" include, when
            // associated with a vector or raster data.


            if (_result.Type == "vectorDataset" || _result.Type == "rasterDataset")
            {
                foreach (var serviceLayer in _result.ServiceLayers)
                {
                    _dataList.Add(CreateServiceType(_result, serviceLayer));
                }

            }

            // New association mode. For services metadata sheet, the layers
            // are stored in the purposely named include: "layers".
            if (_result.Type == "service")
            {
                if (_result.Layers != null)
                {
                    foreach (var layer in _result.Layers)
                    {
                        _dataList.Add(CreateServiceType(_result, layer));
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
                    layerTitle = titles[0].Value;
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
            var resultDetails = await _networkManager.GetDetails(item.Id);
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
            if (!await OpenMetadata(_result))
                _networkManager.OpenAuthenticationPopUp();
        }

        private void MniLoadData_OnClick()
        {
            if (_dataList == null || _dataList.Count == 0)
                return;
            var currentService = _dataList.Count == 1 ? _dataList[0] : _dataList[CmbLayer.SelectedIndex];

            if (currentService != null && currentService.Type?.ToUpper() == "ARCSDE")
                currentService = new ServiceType(currentService.Type, currentService.Title, Variables.configurationManager?.config?.FileSde, 
                    currentService.Name, currentService.Creator, currentService.Id);
            QueuedTask.Run(() =>
            {
                _mapFunctions.AddLayer(currentService);
            });
        }
    }
}
