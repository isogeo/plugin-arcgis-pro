using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using ArcGIS.Desktop.Framework.Dialogs;
using Isogeo.AddIn.Models;
using Isogeo.AddIn.Models.Filters;
using Isogeo.AddIn.Models.Filters.Components;
using Isogeo.AddIn.Views.Search.AskNameWindow;
using Isogeo.Map.MapFunctions;
using Isogeo.Models;
using Isogeo.Models.Network;
using Isogeo.Utils.LogManager;
using Microsoft.Win32;
using MVVMPattern;
using MVVMPattern.MediatorPattern;
using MVVMPattern.RelayCommand;

namespace Isogeo.AddIn.ViewsModels.Settings
{
    public class SearchSettingsViewModel : ViewModelBase
    {
        public QuickSearchSettingsFilters QuickSearchSettingsFilter { get; set; }

        private readonly RestFunctions _restFunctions;

        private readonly FilterManager _filterManager;

        private readonly IMapFunctions _mapFunctions;

        private GeoGraphicalSettingsFilters _geoGraphicalSettings;
        public GeoGraphicalSettingsFilters GeoGraphicalSettings
        {
            get => _geoGraphicalSettings;
            set
            {
                _geoGraphicalSettings = value;
                OnPropertyChanged(nameof(GeoGraphicalSettings));
            }
        }

        public string SdePathFile
        {
            get => Variables.configurationManager.config.fileSde;
            set
            {
                Variables.configurationManager.config.fileSde = value;
                Variables.configurationManager.Save();
                OnPropertyChanged(nameof(SdePathFile));
            }
        }

        private ICommand _saveCommand;
        public ICommand SaveCommand
        {
            get
            {
                return _saveCommand ??= new RelayCommand(
                    x => Save(),
                    y => CanSave());
            }
        }

        private ICommand _deleteCommand;
        public ICommand DeleteCommand
        {
            get
            {
                return _deleteCommand ??= new RelayCommand(
                    x => Delete(),
                    y => CanDelete());
            }
        }

        private ICommand _renameCommand;
        public ICommand RenameCommand
        {
            get
            {
                return _renameCommand ??= new RelayCommand(
                    x => Rename(),
                    y => CanRename());
            }
        }

        private static bool IsDuplicateQuickSearchName(string name)
        {
            return Variables.configurationManager.config.searchs.searchs.Any(search => search.name == name);
        }

        private static bool NewQuickSearchNameIsValid(string name)
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
            var frm = new AskNameWindow(true, QuickSearchSettingsFilter.Name);
            frm.ShowDialog();

            if (frm.isSave == false) return;

            if (!NewQuickSearchNameIsValid(frm.TxtQuickSearchName.Text))
                return;

            for (var i = 0; i < Variables.configurationManager.config.searchs.searchs.Count; i += 1) // todo
            {
                if (Variables.configurationManager.config.searchs.searchs[i].name !=
                    QuickSearchSettingsFilter.SelectedItem.Name) continue;
                Variables.configurationManager.config.searchs.searchs[i].name = frm.TxtQuickSearchName.Text;
                var item = QuickSearchSettingsFilter.SelectedItem;
                QuickSearchSettingsFilter.Items.Remove(item);
                var newItem = new FilterItem(item.Id, frm.TxtQuickSearchName.Text);
                QuickSearchSettingsFilter.AddItem(newItem);
                QuickSearchSettingsFilter.SelectItem(newItem.Name);
                Variables.configurationManager.Save();
                Mediator.NotifyColleagues("ChangeQuickSearch", null);
                break;
            }
        }

        private bool CanRename()
        {
            if (QuickSearchSettingsFilter?.SelectedItem != null && QuickSearchSettingsFilter?.Items?.Count > 0) 
                return true;
            return false;
        }

        private bool CanDelete()
        {
            if (QuickSearchSettingsFilter?.Items?.Count > 0)
                return true;
            return false;
        }

        private void Delete()
        {
            foreach (var search in Variables.configurationManager.config.searchs.searchs.Where(search => search.name == QuickSearchSettingsFilter.SelectedItem.Name))
            {
                Variables.configurationManager.config.searchs.searchs.Remove(search);
                QuickSearchSettingsFilter.Items.Remove(QuickSearchSettingsFilter.SelectedItem);
                Mediator.NotifyColleagues("ChangeQuickSearch", null);
                Variables.configurationManager.Save();
                break; // todo find
            }
        }

        private bool CanSave()
        {
            return QuickSearchSettingsFilter?.Items?.Count > 0 && QuickSearchSettingsFilter?.SelectedItem != null;
        }

        private void Save()
        {
            Variables.configurationManager.config.defaultSearch = QuickSearchSettingsFilter.SelectedItem.Id;
            Variables.configurationManager.Save();
        }


        private void GeoGraphicalSettings_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(nameof(GeoGraphicalSettings));
        }

        private void InitGeographicalOperator()
        {// todo
            GeoGraphicalSettings = new GeoGraphicalSettingsFilters("GeoGraphicalSettings", _restFunctions, _filterManager, _mapFunctions);
            if (Variables.configurationManager.config.geographicalOperator == "contains" ||  
                Variables.configurationManager.config.geographicalOperator == "within" ||
                Variables.configurationManager.config.geographicalOperator == "intersects") 
                GeoGraphicalSettings.SelectItem("", Variables.configurationManager.config.geographicalOperator);
            GeoGraphicalSettings.PropertyChanged += GeoGraphicalSettings_PropertyChanged;
            OnPropertyChanged(nameof(GeoGraphicalSettings));
        }

        private void QuickSearchSettings_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(nameof(QuickSearchSettingsFilter));
        }

        private void AddNewQuickSearchEvent(object newSearch)
        {
            QuickSearchSettingsFilter.AddItem((Isogeo.Models.Configuration.Search)newSearch);
            QuickSearchSettingsFilter.SelectItem(((Isogeo.Models.Configuration.Search)newSearch).name);
        }

        private void InitQuickSearch()
        {
            QuickSearchSettingsFilter = new QuickSearchSettingsFilters("QuickSearchSettings", _restFunctions, _filterManager, _mapFunctions);
            QuickSearchSettingsFilter.PropertyChanged += QuickSearchSettings_PropertyChanged;
            QuickSearchSettingsFilter.SetItems(Variables.configurationManager.config.searchs.searchs);
        }

        private ICommand _loadSdeFileCommand;
        public ICommand LoadSdeFileCommand
        {
            get
            {
                return _loadSdeFileCommand ??= new RelayCommand(
                    x => LoadSdeFile(),
                    y => CanLoadSdeFile());
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

        public SearchSettingsViewModel(RestFunctions restFunctions, FilterManager filterManager, IMapFunctions mapFunctions)
        {
            _restFunctions = restFunctions;
            _filterManager = filterManager;
            _mapFunctions = mapFunctions;
            InitGeographicalOperator();
            InitQuickSearch();
            Mediator.Register("AddNewQuickSearch", AddNewQuickSearchEvent);
        }
    }
}
