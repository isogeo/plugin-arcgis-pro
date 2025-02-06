using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using Isogeo.AddIn.Models;
using Isogeo.AddIn.ViewsModels.Metadata;
using Isogeo.Map;
using Isogeo.Map.DataType;
using Isogeo.Models;
using Isogeo.Models.API;
using Isogeo.Network;
using Isogeo.Utils.ConfigurationManager;
using Isogeo.Utils.LogManager;
using MVVMPattern;
using MVVMPattern.MediatorPattern;
using RelayCommand = MVVMPattern.RelayCommand.RelayCommand;

namespace Isogeo.AddIn.ViewsModels.Search.Results
{
    public class ResultItemViewModel : ViewModelBase
    {
        private readonly IConfigurationManager _configurationManager;
        public Result Result { get; set; }

        // Isogeo geometry types
        private readonly List<string> _polygonList = new(new[] { "CurvePolygon", "MultiPolygon", "MultiSurface", "Polygon", "PolyhedralSurface" });
        private readonly List<string> _pointList = new(new[] { "Point", "MultiPoint" });
        private readonly List<string> _lineList = new(new[] { "CircularString", "CompoundCurve", "Curve", "LineString", "MultiCurve", "MultiLineString" });
        private readonly List<string> _multiList = new(new[] { "Geometry", "GeometryCollection" });

        //Isogeo formats
        private readonly List<string> _vectorFormatList = new(new[] { "shp", "dxf", "dgn", "filegdb", "tab", "arcsde", "postgis" });
        private readonly List<string> _rasterFormatList = new(new[] { "esriasciigrid", "geotiff", "intergraphgdb", "jpeg", "png", "xyz", "ecw" });

        private readonly List<ServiceType> _dataList = new();

        private Views.Metadata.Metadata _metadataInstance;
        private MetadataViewModel _metadataViewModel;

        public CmbLayer CmbLayer { get; set; }
        public string ImgType { get; set; }
        public string ItemTitle { get; set; }
        public ToolTip ItemToolTip { get; set; }

        private readonly IMapManager _mapManager;
        private readonly INetworkManager _networkManager;

        public Visibility WaitingCmbLayerVisibility { get; private set; }

        public ResultItemViewModel(IMapManager mapManager, INetworkManager networkManager, IConfigurationManager configurationManager)
        {
            _configurationManager = configurationManager;
            _mapManager = mapManager;
            _networkManager = networkManager;
            CmbLayer = new CmbLayer();
            WaitingCmbLayerVisibility = Visibility.Visible;
        }

        public void LoadComboBox()
        {
            GetLinks();
            SetCombo();
            WaitingCmbLayerVisibility = Visibility.Hidden;
            OnPropertyChanged(nameof(CmbLayer));
            OnPropertyChanged(nameof(WaitingCmbLayerVisibility));
        }

        public void Init(Result item)
        {
            Result = item;
            SetComponent();
        }

        private ICommand _openMetadataClickCommand;
        public ICommand OpenMetadataClickCommand
        {
            get
            {
                return _openMetadataClickCommand ??= new RelayCommand(
                    x => OpenMetadata(),
                    y => CanOpenMetadata());
            }
        }

        private bool CanOpenMetadata()
        {
            return true;
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
            var resultDetails = await _networkManager.GetDetails(Result.Id);
            if (resultDetails != null)
                Result = resultDetails;
            MniLoadData_OnClick();
        }

        private bool CanRunBtnMenu_OnClick()
        {
            return CmbLayer != null && CmbLayer.IsVisible && CmbLayer.IsEnabled && CmbLayer.Items.Count > 0;
        }

        private void SetComponent()
        {
            ImgType = Language.Resources.Unknown_geometry;
            if (Result.Geometry != null)
            {
                if (_polygonList.Contains(Result.Geometry)) ImgType = Language.Resources.Polygon;
                if (_pointList.Contains(Result.Geometry)) ImgType = Language.Resources.Point;
                if (_lineList.Contains(Result.Geometry)) ImgType = Language.Resources.Line;
                if (_multiList.Contains(Result.Geometry)) ImgType = Language.Resources.MultiPolygon;
            }
            else
            {
                if (Result.Type == "rasterDataset")
                    ImgType = Language.Resources.Raster;
                else if (Result.Type == "service")
                    ImgType = Language.Resources.Service;
                else
                    ImgType = Language.Resources.Unknown_geometry;
            }
            ItemTitle = Result.Title;
            var toolTip = new ToolTip {Content = Result.Abstract};
            ItemToolTip = toolTip;
        }

        private void SetCombo()
        {
            CmbLayer.Visibility = Visibility.Visible;
            CmbLayer.IsEnabled = true;

            // get useful metadata
            //added. The "Add" column has to be filled accordingly.
            //If the data can't be added, just insert "can't" text.
            switch (_dataList.Count)
            {
                case 0:
                    CmbLayer.Items.Add(Language.Resources.Cant_be_added);
                    CmbLayer.SelectedIndex = 0;
                    CmbLayer.IsEnabled = false;
                    break;
                default:                
                    foreach (var data in _dataList)
                    {
                        var type = Language.Resources.Empty;
                        var title = Language.Resources.Empty;
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
            if (itemResult?.Format != null && layer?.Id != null)
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
            if (_vectorFormatList.Contains(Result.Format))
            {
                _dataList.Add(CreateServiceType(Result));
            }

            if (_rasterFormatList.Contains(Result.Format))
            {
                var item = CreateServiceType(Result);
                _dataList.Add(new ServiceType("raster", item.Title, item.Url, item.Name, item.Creator, item.Id));
            }

            // This is the new association mode. The layer and service
            //information are stored in the "serviceLayers" include, when
            // associated with a vector or raster data.
            if ((Result.Type == "vectorDataset" || Result.Type == "rasterDataset") && Result.ServiceLayers != null)
            {
                foreach (var serviceLayer in Result.ServiceLayers)
                {
                    _dataList.Add(CreateServiceType(Result, serviceLayer));
                }
            }

            // New association mode. For services metadata sheet, the layers
            // are stored in the purposely named include: "layers".
            if (Result.Type == "service" && Result.Layers != null)
            {
                foreach (var layer in Result.Layers)
                {
                    _dataList.Add(CreateServiceType(Result, layer));
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

        private static string GetTitle(string type, string id, List<Title>? titles)
        {
            var layerTitle = id;
            if (type == "WFS")
                layerTitle = Regex.Replace(id, "\\{.*?}", string.Empty);

            if (titles is { Count: > 0 })
                layerTitle = titles[0].Value;
           
            if (string.IsNullOrWhiteSpace(layerTitle))
                layerTitle = type + " Layer";
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

            _metadataViewModel ??= new MetadataViewModel();

            _metadataViewModel.RegisterMediator();
            if (_metadataInstance == null || !_metadataInstance.IsLoaded)
            {
                _metadataInstance = new Views.Metadata.Metadata();
                DefineViewModelMetadata();
                Mediator.NotifyColleagues(MediatorEvent.ResultSelected, item);

                _metadataInstance.MinWidth = 550;
                _metadataInstance.MinHeight = 400;
            }
            Log.Logger.Info("Open Metadata");
            _metadataInstance.ShowDialog(); 
            _metadataViewModel.UnRegisterMediator();
            return true;
        }

        private async void OpenMetadata()
        {
            if (!await OpenMetadata(Result))
                _networkManager.OpenAuthenticationPopUp();
        }

        private void MniLoadData_OnClick()
        {
            if (_dataList == null || _dataList.Count == 0)
                return;
            GetLinks();
            var currentService = _dataList.Count == 1 ? _dataList[0] : _dataList[CmbLayer.SelectedIndex];

            if (currentService != null && (currentService.Type?.ToUpper() == "ARCSDE" || currentService.Type?.ToUpper() == "POSTGIS"))
                currentService = new ServiceType(currentService.Type, currentService.Title, _configurationManager?.Config?.FileSde, 
                    currentService.Name, currentService.Creator, currentService.Id);
            QueuedTask.Run(() =>
            {
                _mapManager.AddLayer(currentService);
            });
        }
    }
}
