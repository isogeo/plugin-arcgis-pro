using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using ArcGIS.Desktop.Framework.Dialogs;
using Isogeo.AddIn.Views.Search.AskNameWindow;
using Isogeo.Models;
using Isogeo.Models.Filters;
using Isogeo.Utils.LogManager;
using Microsoft.Win32;
using MVVMPattern;
using MVVMPattern.MediatorPattern;
using MVVMPattern.RelayCommand;

namespace Isogeo.AddIn.ViewsModels.Settings
{
    public class SearchSettingsViewModel : ViewModelBase
    {
        public QuickSearchSettings QuickSearchSettings { get; set; }

        private GeoGraphicalSettings _geoGraphicalSettings;
        public GeoGraphicalSettings GeoGraphicalSettings
        {
            get => _geoGraphicalSettings;
            set
            {
                _geoGraphicalSettings = value;
                OnPropertyChanged("GeoGraphicalSettings");
            }
        }

        private string _sdePathFile = "";
        public string SdePathFile
        {
            get => _sdePathFile;
            set
            {
                _sdePathFile = value;
                Variables.configurationManager.config.fileSde = value;
                OnPropertyChanged("SdePathFile");
            }
        }

        private ICommand _saveCommand;
        public ICommand SaveCommand
        {
            get
            {
                return _saveCommand ?? (_saveCommand = new RelayCommand(
                    x => Save(),
                    y => CanSave()));
            }
        }

        private ICommand _deleteCommand;
        public ICommand DeleteCommand
        {
            get
            {
                return _deleteCommand ?? (_deleteCommand = new RelayCommand(
                    x => Delete(),
                    y => CanDelete()));
            }
        }

        private ICommand _renameCommand;
        public ICommand RenameCommand
        {
            get
            {
                return _renameCommand ?? (_renameCommand = new RelayCommand(
                    x => Rename(),
                    y => CanRename()));
            }
        }

        private static bool IsDuplicateQuickSearchName(string name)
        {
            return Variables.configurationManager.config.searchs.searchs.Any(search => search.name == name);
        }

        private bool NewQuickSearchNameIsValid(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                Log.Logger.Error("Save QuickSearch - " + name + ": " + Language.Resources.Quicksearch_name_mandatory);
                MessageBox.Show(name + @": " + Language.Resources.Quicksearch_name_mandatory);
                return false;
            }

            if (IsDuplicateQuickSearchName(name))
            {
                Log.Logger.Error("Save QuickSearch - " + name + ": " + Language.Resources.Quicksearch_already_exist);
                MessageBox.Show(name + @": " + Language.Resources.Quicksearch_already_exist);
                return false;
            }

            return true;
        }

        private void Rename()
        {
            var frm = new AskNameWindow(true, QuickSearchSettings.Name);
            frm.ShowDialog();

            if (frm.isSave == false) return;

            if (!NewQuickSearchNameIsValid(frm.TxtQuickSearchName.Text))
                return;

            for (var i = 0; i < Variables.configurationManager.config.searchs.searchs.Count; i += 1)
            {
                if (Variables.configurationManager.config.searchs.searchs[i].name !=
                    QuickSearchSettings.SelectedItem.Name) continue;
                Variables.configurationManager.config.searchs.searchs[i].name = frm.TxtQuickSearchName.Text;
                var item = QuickSearchSettings.SelectedItem;
                QuickSearchSettings.Items.Remove(item);
                item.Name = frm.TxtQuickSearchName.Text;
                QuickSearchSettings.AddItem(item);
                QuickSearchSettings.SelectItem(item.Name);
                Variables.configurationManager.Save();
                Mediator.NotifyColleagues("ChangeQuickSearch", null);
                break;
            }
        }

        private bool CanRename()
        {
            if (QuickSearchSettings?.SelectedItem != null && QuickSearchSettings?.Items?.Count > 0) 
                return true;
            return false;
        }

        private bool CanDelete()
        {
            if (QuickSearchSettings?.Items?.Count > 0)
                return true;
            return false;
        }

        private void Delete()
        {
            foreach (var search in Variables.configurationManager.config.searchs.searchs.Where(search => search.name == QuickSearchSettings.SelectedItem.Name))
            {
                Variables.configurationManager.config.searchs.searchs.Remove(search);
                QuickSearchSettings.Items.Remove(QuickSearchSettings.SelectedItem);
                Mediator.NotifyColleagues("ChangeQuickSearch", null);
                Variables.configurationManager.Save();
                break;
            }
        }

        private bool CanSave()
        {
            return QuickSearchSettings?.Items?.Count > 0 && QuickSearchSettings?.SelectedItem != null;
        }

        private void Save()
        {
            Variables.configurationManager.config.defaultSearch = QuickSearchSettings.SelectedItem.Id;
            Variables.configurationManager.Save();
        }


        private void GeoGraphicalSettings_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged("GeoGraphicalSettings");
        }

        private void InitGeographicalOperator()
        {
            GeoGraphicalSettings = new GeoGraphicalSettings("GeoGraphicalSettings");
            if (Variables.configurationManager.config.geographicalOperator == "contains" ||  
                Variables.configurationManager.config.geographicalOperator == "within" ||
                Variables.configurationManager.config.geographicalOperator == "intersects") 
                GeoGraphicalSettings.SelectItem("", Variables.configurationManager.config.geographicalOperator);
            GeoGraphicalSettings.PropertyChanged += GeoGraphicalSettings_PropertyChanged;
            OnPropertyChanged("GeoGraphicalSettings");
        }

        private void QuickSearchSettings_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged("QuickSearchSettings");
        }

        private void AddNewQuickSearchEvent(object newSearch)
        {
            QuickSearchSettings.AddItem((Models.Configuration.Search)newSearch);
            QuickSearchSettings.SelectItem(((Models.Configuration.Search)newSearch).name);
        }

        private void InitQuickSearch()
        {
            QuickSearchSettings = new QuickSearchSettings("QuickSearchSettings");
            QuickSearchSettings.PropertyChanged += QuickSearchSettings_PropertyChanged;
            QuickSearchSettings.SetItems(Variables.configurationManager.config.searchs.searchs);
        }

        private ICommand _loadSdeFileCommand;
        public ICommand LoadSdeFileCommand
        {
            get
            {
                return _loadSdeFileCommand ?? (_loadSdeFileCommand = new RelayCommand(
                    x => LoadSdeFile(),
                    y => CanLoadSdeFile()));
            }
        }

        private static bool CanLoadSdeFile()
        {
            return true;
        }

        private void LoadSdeFile()
        {
            try
            {
                var fileDialog = new OpenFileDialog();
                if (fileDialog.ShowDialog() == true)
                    SdePathFile = fileDialog.FileName;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error SDE", "Isogeo");
                Log.Logger.Error("Error - " + ex.Message);
            }
        }

        public SearchSettingsViewModel()
        {
            InitGeographicalOperator();
            InitQuickSearch();
            Mediator.Register("AddNewQuickSearch", AddNewQuickSearchEvent);
        }
    }
}
