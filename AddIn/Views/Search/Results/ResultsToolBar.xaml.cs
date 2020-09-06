using System;
using System.ComponentModel;
using System.Windows;
using ArcGIS.Desktop.Framework;

namespace Isogeo.AddIn.Views.Search.Results
{
    public partial class ResultsToolBar
    {
        private void InitResources()
        {
            Isogeo.Resources.Dummy.DummyCode();
            var resources = Resources;
            resources.BeginInit();

            if (FrameworkApplication.ApplicationTheme == ApplicationTheme.Dark ||
                FrameworkApplication.ApplicationTheme == ApplicationTheme.HighContrast)
            {
                resources.MergedDictionaries.Add(
                    new ResourceDictionary()
                    {
                        Source = new Uri("pack://application:,,,/Isogeo.Resources;component/Themes/DarkTheme.xaml")
                    });
            }
            else
            {
                resources.MergedDictionaries.Add(
                    new ResourceDictionary()
                    {
                        Source = new Uri("pack://application:,,,/Isogeo.Resources;component/Themes/LightTheme.xaml")
                    });
            }
            resources.EndInit();
        }

        internal bool IsInDesignMode
        {
            get
            {
                return (bool)DependencyPropertyDescriptor.FromProperty(
                    DesignerProperties.IsInDesignModeProperty, typeof(DependencyObject)).Metadata.DefaultValue;
            }
        }

        public ResultsToolBar()
        {
            InitializeComponent();
            if (!IsInDesignMode)
                InitResources();
        }
    }
}
