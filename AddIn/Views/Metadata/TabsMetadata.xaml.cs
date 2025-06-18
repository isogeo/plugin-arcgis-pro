
using ArcGIS.Desktop.Framework;
using System.Windows;
using System;
using System.ComponentModel;

namespace Isogeo.AddIn.Views.Metadata
{
    public partial class TabsMetadata
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

        internal bool IsInDesignMode =>
            (bool)DependencyPropertyDescriptor.FromProperty(
                DesignerProperties.IsInDesignModeProperty, typeof(DependencyObject)).Metadata.DefaultValue;

        public TabsMetadata()
        {
            InitializeComponent();
            if (!IsInDesignMode)
                InitResources();
        }
    }
}
