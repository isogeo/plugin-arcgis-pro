using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using ArcMapAddinIsogeo;
using API = ArcMapAddinIsogeo.API;

namespace Arcgis_Pro_Isogeo.UI.Search.Results
{
    /// <summary>
    /// Logique d'interaction pour ResultItem.xaml
    /// </summary>
    public partial class ResultItem : UserControl
    {
        public API.Result result;
        // Isogeo geometry types
        private List<String> polygon_list = new List<String>(new String[] { "CurvePolygon", "MultiPolygon", "MultiSurface", "Polygon", "PolyhedralSurface" });
        private List<String> point_list = new List<String>(new String[] { "Point", "MultiPoint" });
        private List<String> line_list = new List<String>(new String[] { "CircularString", "CompoundCurve", "Curve", "LineString", "MultiCurve", "MultiLineString" });
        private List<String> multi_list = new List<String>(new String[] { "Geometry", "GeometryCollection" });

        //Isogeo formats
        private List<String> vectorformat_list = new List<String>(new String[] { "shp", "dxf", "dgn", "filegdb", "tab", "arcsde" });
        private List<String> rasterformat_list = new List<String>(new String[] { "esriasciigrid", "geotiff", "intergraphgdb", "jpeg", "png", "xyz", "ecw" });

        private List<ArcMapAddinIsogeo.DataType.ServiceType> data_list = new List<ArcMapAddinIsogeo.DataType.ServiceType>();

        public ResultItem()
        {
            InitializeComponent();
        }

        public void Init(API.Result result)
        {
            this.result = result;
            setComponent();
            getLinks();
            setCombo();
            Variables.functionsTranslate.Add(translate);
            translate();
        }

        private void translate()
        {
            MniLoadData.Header = Variables.localisationManager.getValue(ArcMapAddinIsogeo.Localization.LocalizationItem.menuLoad);
            MniShowMetadata.Header = Variables.localisationManager.getValue(ArcMapAddinIsogeo.Localization.LocalizationItem.menuMetadata);
            MniOpenCatalog.Header = Variables.localisationManager.getValue(ArcMapAddinIsogeo.Localization.LocalizationItem.menuOpenCatalog);
        }

        private BitmapImage ReturnPicture(String imagePath)
        {
            return new BitmapImage(new Uri(imagePath, UriKind.Absolute));
        }

        private void setComponent()
        {
            ImgType.Source =
                ReturnPicture("pack://application:,,,/" + Variables._assemblyName + ";component/Resources/gavel.png"); //imageList1.Images[5];
            if (result.geometry != null)
            {
                if (result.geometry != null)
                {
                    if (polygon_list.Contains(result.geometry) == true) ImgType.Source = 
                        ReturnPicture("pack://application:,,,/" + Variables._assemblyName + ";component/Resources/gavel.png"); ; // imageList1.Images[0];
                    if (point_list.Contains(result.geometry) == true) ImgType.Source = 
                        ReturnPicture("pack://application:,,,/" + Variables._assemblyName + ";component/Resources/gavel.png"); // imageList1.Images[1];
                    if (line_list.Contains(result.geometry) == true) ImgType.Source = 
                        ReturnPicture("pack://application:,,,/" + Variables._assemblyName + ";component/Resources/gavel.png"); // imageList1.Images[2];
                    if (multi_list.Contains(result.geometry) == true) ImgType.Source = 
                        ReturnPicture("pack://application:,,,/" + Variables._assemblyName + ";component/Resources/gavel.png"); // imageList1.Images[3];
                    //if (result.geometry=="TIN") img_type.Image = imageList1.Images[4];
                }
                else
                {

                }

            }
            else
            {
                if (result.type == "rasterDataset")
                {
                    ImgType.Source = 
                        ReturnPicture("pack://application:,,,/" + Variables._assemblyName + ";component/Resources/gavel.png"); // imageList1.Images[4];
                }
            }
            TxtTitle.Text = result.title;
            ToolTip toolTip = new ToolTip();
            toolTip.Content = result.@abstract;
            TxtTitle.ToolTip = toolTip;
        }

        private void setCombo()
        {
            LblLayer.Visibility = Visibility.Visible;
            //img_layer.Visible=true;
            CmbLayer.IsEnabled = false;
            MniLoadData.IsEnabled = true;
            MniShowMetadata.IsEnabled = true;
            MniOpenCatalog.IsEnabled = true;

            // get useful metadata
            //added. The "Add" column has to be filled accordingly.
            //If the data can't be added, just insert "can't" text.
            switch (data_list.Count)
            {
                case 0:
                    LblLayer.Content = "Can't be added";
                    MniLoadData.IsEnabled = false;
                    MniOpenCatalog.IsEnabled = false;
                    break;
                case 1:
                    LblLayer.Content = data_list[0].title;
                    switch (data_list[0].type)
                    {
                        case "WMS":
                            ImgLayer.Source = 
                                ReturnPicture("pack://application:,,,/" + Variables._assemblyName + ";component/Resources/gavel.png");  // imageList2.Images[0];
                            break;
                        case "WFS":
                            ImgLayer.Source =
                                ReturnPicture("pack://application:,,,/" + Variables._assemblyName + ";component/Resources/gavel.png");// imageList2.Images[1];
                            break;
                        case "WMTS":
                            ImgLayer.Source = 
                                ReturnPicture("pack://application:,,,/" + Variables._assemblyName + ";component/Resources/gavel.png");// imageList2.Images[1];
                            break;
                        case "PostGIS table":
                            ImgLayer.Source = 
                                ReturnPicture("pack://application:,,,/" + Variables._assemblyName + ";component/Resources/gavel.png"); ;// imageList2.Images[2];
                            break;
                        case "Data file":
                            ImgLayer.Source = 
                                ReturnPicture("pack://application:,,,/" + Variables._assemblyName + ";component/Resources/gavel.png"); // imageList2.Images[3];
                            break;
                    }
                    break;
                default:
                    //If there is only one way for the data to be added, insert a label.
                    //Else, add a combobox, storing all possibilities.                    
                    LblLayer.IsEnabled = false;
                    //img_layer.Visible=false;
                    CmbLayer.IsEnabled = true;

                    foreach (ArcMapAddinIsogeo.DataType.ServiceType data in data_list)
                    {
                        CmbLayer.Items.Add(data.type + " - " + data.title);
                    }
                    CmbLayer.SelectedIndex = 0;
                    break;
            }

        }

        private void getLinks()
        {

            if (vectorformat_list.Contains(result.format) == true)
            {
                data_list.Add(new ArcMapAddinIsogeo.DataType.ServiceType(result.format, result.title, result.path, result.name, result._creator._id, result._id));
            }

            if (rasterformat_list.Contains(result.format) == true)
            {
                data_list.Add(new ArcMapAddinIsogeo.DataType.ServiceType("raster", result.title, result.path, result.name, result._creator._id, result._id));
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
                    data_list.Add(new ArcMapAddinIsogeo.DataType.ServiceType(serviceLayer.service.format.ToUpper(), serviceLayer.titles[0].value, serviceLayer.service.path, getId(serviceLayer.id), result._creator._id, result._id));
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
                        data_list.Add(new ArcMapAddinIsogeo.DataType.ServiceType(result.format.ToUpper(), getTitle(result.format.ToUpper(), layer.id, layer.titles), result.path, getId(layer.id), result._creator._id, result._id));
                    }
                }
            }
        }


        private String getId(String id)
        {
            String newId = id;
            newId = newId.Replace('{', ' ');
            newId = newId.Trim();
            newId = newId.Replace('}', ':');
            return newId;
        }

        private String getTitle(String type, String id, List<API.Title> titles)
        {
            String layer_title = id;
            if (type == "WFS")
            {
                layer_title = System.Text.RegularExpressions.Regex.Replace(id, "\\{.*?}", String.Empty);
            }

            try
            {
                if (titles.Count > 0)
                {
                    layer_title = titles[0].value;
                }
            }
            catch { }
            if (layer_title == "") layer_title = type + " Layer";
            return layer_title;
        }

        private void openMetadata()
        {
            Variables.currentResult = null;
            Variables.restFunctions.getDetails(result._id);
            MniShowMetadata_OnClick(null, null);
        }

        private void TxtTitle_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            openMetadata();
        }

        private void ImgType_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            openMetadata();
        }

        // TODO : this function is a test
        private void UIElement_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            openMetadata();
        }

        private void BtnMenu_OnClick(object sender, RoutedEventArgs e)
        {
            Variables.currentResult = null;
            Variables.restFunctions.getDetails(result._id);

            MniLoadData_OnClick(null, null);
        }

        private void MniOpenCatalog_OnClick(object sender, RoutedEventArgs e)
        {
            ArcMapAddinIsogeo.DataType.ServiceType currentService;
            if (data_list.Count == 1)
            {
                currentService = data_list[0];
            }
            else
            {
                currentService = data_list[CmbLayer.SelectedIndex];
            }

            String clientId = Variables.configurationManager.config.userAuthentification.id;
            String clientSecret = RijndaelManagedEncryption.DecryptRijndael(Variables.configurationManager.config.userAuthentification.secret, Variables.encryptCode);


            String url = "https://app.isogeo.com/groups/" + currentService.creator + "/resources/" + currentService.id + "/identification";
            System.Diagnostics.Process.Start(url);
        }

        private void MniShowMetadata_OnClick(object sender, RoutedEventArgs e)
        {
            Metadata.Metadata frmMetadata = new Metadata.Metadata();
            frmMetadata.ShowDialog();
        }

        private void MniLoadData_OnClick(object sender, RoutedEventArgs e)
        {
            ArcMapAddinIsogeo.DataType.ServiceType currentService;
            if (data_list.Count == 1)
            {
                currentService = data_list[0];
            }
            else
            {
                currentService = data_list[CmbLayer.SelectedIndex];
            }

            // TODO ArcMapAddinIsogeo.Utils.MapFunctions.AddLayer(currentService);
        }
    }
}
