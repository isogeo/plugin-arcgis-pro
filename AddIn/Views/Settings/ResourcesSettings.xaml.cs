using System.Windows.Controls;
using Isogeo.Utils.LogManager;

namespace Isogeo.AddIn.Views.Settings
{
    public partial class ResourcesSettings : UserControl
    {
        public ResourcesSettings()
        {
            InitializeComponent();
            TbLogPath.Text = Log.GetLogFilePath();
        }
    }
}
