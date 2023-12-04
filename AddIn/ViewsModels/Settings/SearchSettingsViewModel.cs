using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using ArcGIS.Desktop.Framework.Dialogs;
using Isogeo.AddIn.Models.FilterManager;
using Isogeo.AddIn.Models.Filters;
using Isogeo.AddIn.Models.Filters.Components;
using Isogeo.AddIn.Views.Search.AskNameWindow;
using Isogeo.Map;
using Isogeo.Network;
using Isogeo.Utils.ConfigurationManager;
using Isogeo.Utils.LogManager;
using Microsoft.Win32;
using MVVMPattern;
using MVVMPattern.MediatorPattern;
using MVVMPattern.RelayCommand;

namespace Isogeo.AddIn.ViewsModels.Settings
{
    public class SearchSettingsViewModel : ViewModelBase
    {
        public QuickSearchSettingsFilters QuickSearchSettingsFilters { get; set; }

        private readonly INetworkManager _networkManager;

        private readonly IFilterManager _filterManager;

        private readonly IConfigurationManager _configurationManager;

        private readonly IMapManager _mapManager;

        public SearchSettingsViewModel(INetworkManager networkManager, IFilterManager filterManager, IMapManager mapManager,
            IConfigurationManager configurationManager)
        {
            _configurationManager = configurationManager;
            _networkManager = networkManager;
            _filterManager = filterManager;
            _mapManager = mapManager;
            InitGeographicalOperator();
            InitQuickSearch();
            Mediator.Register("AddNewQuickSearch", AddNewQuickSearchEvent);
        }

        private GeoGraphicalSettingsFilters _geoGraphicalSettingsFilters;
        public GeoGraphicalSettingsFilters GeoGraphicalSettingsFilters
        {
            get => _geoGraphicalSettingsFilters;
            set
            {
                _geoGraphicalSettingsFilters = value;
                OnPropertyChanged(nameof(GeoGraphicalSettingsFilters));
            }
        }

        public string SdePathFile
        {
            get => _configurationManager.Config.FileSde;
            set
            {
                _configurationManager.Config.FileSde = value;
                _configurationManager.Save();
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

        private bool IsDuplicateQuickSearchName(string name)
        {
            return _configurationManager.Config.Searchs.SearchDetails.Any(search => search.Name == name);
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
            var frm = new AskNameWindow(true, QuickSearchSettingsFilters.Name, _configurationManager);
            frm.ShowDialog();

            if (frm.isSave == false) return;

            if (!NewQuickSearchNameIsValid(frm.TxtQuickSearchName.Text))
                return;

            for (var i = 0; i < _configurationManager.Config.Searchs.SearchDetails.Count; i += 1) // todo
            {
                if (_configurationManager.Config.Searchs.SearchDetails[i].Name !=
                    QuickSearchSettingsFilters.SelectedItem.Name) continue;
                _configurationManager.Config.Searchs.SearchDetails[i].Name = frm.TxtQuickSearchName.Text;
                var item = QuickSearchSettingsFilters.SelectedItem;
                QuickSearchSettingsFilters.Items.Remove(item);
                var newItem = new FilterItem(item.Id, frm.TxtQuickSearchName.Text);
                QuickSearchSettingsFilters.AddItem(newItem);
                QuickSearchSettingsFilters.SelectItem(newItem.Name);
                _configurationManager.Save();
                Mediator.NotifyColleagues("ChangeQuickSearch", null);
                break;
            }
        }

        private bool CanRename()
        {
            if (QuickSearchSettingsFilters?.SelectedItem != null && QuickSearchSettingsFilters?.Items?.Count > 0) 
                return true;
            return false;
        }

        private bool CanDelete()
        {
            if (QuickSearchSettingsFilters?.Items?.Count > 0)
                return true;
            return false;
        }

        private void Delete()
        {
            foreach (var search in _configurationManager.Config.Searchs.SearchDetails.Where(search => search.Name == QuickSearchSettingsFilters.SelectedItem.Name))
            {
                _configurationManager.Config.Searchs.SearchDetails.Remove(search);
                QuickSearchSettingsFilters.Items.Remove(QuickSearchSettingsFilters.SelectedItem);
                Mediator.NotifyColleagues("ChangeQuickSearch", null);
                _configurationManager.Save();
                break; // todo find
            }
        }

        private bool CanSave()
        {
            return QuickSearchSettingsFilters?.Items?.Count > 0 && QuickSearchSettingsFilters?.SelectedItem != null;
        }

        private void Save()
        {
            _configurationManager.Config.DefaultSearch = QuickSearchSettingsFilters.SelectedItem.Id;
            _configurationManager.Save();
        }


        private void GeoGraphicalSettings_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(nameof(GeoGraphicalSettingsFilters));
        }

        private void InitGeographicalOperator()
        {// todo
            GeoGraphicalSettingsFilters = new GeoGraphicalSettingsFilters("GeoGraphicalSettings", _networkManager, _filterManager, _mapManager,
                _configurationManager);
            if (_configurationManager.Config.GeographicalOperator == "contains" ||  
                _configurationManager.Config.GeographicalOperator == "within" ||
                _configurationManager.Config.GeographicalOperator == "intersects") 
                GeoGraphicalSettingsFilters.SelectItem("", _configurationManager.Config.GeographicalOperator);
            GeoGraphicalSettingsFilters.PropertyChanged += GeoGraphicalSettings_PropertyChanged;
            OnPropertyChanged(nameof(GeoGraphicalSettingsFilters));
        }

        private void QuickSearchSettings_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(nameof(QuickSearchSettingsFilters));
        }

        private void AddNewQuickSearchEvent(object newSearch)
        {
            QuickSearchSettingsFilters.AddItem((Isogeo.Models.Configuration.Search)newSearch);
            QuickSearchSettingsFilters.SelectItem(((Isogeo.Models.Configuration.Search)newSearch).Name);
        }

        private void InitQuickSearch()
        {
            QuickSearchSettingsFilters = new QuickSearchSettingsFilters("QuickSearchSettings", _networkManager, _filterManager, _mapManager);
            QuickSearchSettingsFilters.PropertyChanged += QuickSearchSettings_PropertyChanged;
            QuickSearchSettingsFilters.SetItems(_configurationManager.Config.Searchs.SearchDetails);
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
    }
}
