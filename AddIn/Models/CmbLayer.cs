using System.Collections.Generic;
using System.Windows;

namespace Isogeo.AddIn.Models
{
    public class CmbLayer
    {
        public Visibility Visibility { get; set; } = Visibility.Hidden;

        public bool IsEnabled { get; set; } = false;

        public List<string> Items { get; } = new();

        public int SelectedIndex { get; set; }

        public bool IsVisible => Visibility == Visibility.Visible;
    }
}
