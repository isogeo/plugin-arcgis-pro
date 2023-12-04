using System.Windows;
using System.Windows.Input;
using Isogeo.Models;
using MessageBox = ArcGIS.Desktop.Framework.Dialogs.MessageBox;

namespace Isogeo.AddIn.Views.Search.AskNameWindow
{
    public partial class AskNameWindow
    {
        public bool isRename;
        public bool isSave;
        public string oldName;

        public AskNameWindow(bool isRename, string oldName)
        {
            InitializeComponent();
            this.isRename = isRename;
            this.oldName = oldName;
        }

        public void Init(bool rename, string name)
        {
            isRename = rename;
            oldName = name;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            isSave = false;
            Close();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtQuickSearchName.Text))
            {
                MessageBox.Show(this, Isogeo.Language.Resources.Quicksearch_name_mandatory, Title);
                return;
            }

            // todo
            foreach (var search in Variables.configurationManager.config.searchs.searchs)
            {
                if (search.name == TxtQuickSearchName.Text && search.name != oldName)
                {
                    MessageBox.Show(this,
                        Isogeo.Language.Resources.Quicksearch_already_exist, Title);
                    return;
                }
            }

            isSave = true;
            Close();
        }

        private void TxtQuickSearch_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                BtnSave_Click(null, null);
            }
        }
    }
}
