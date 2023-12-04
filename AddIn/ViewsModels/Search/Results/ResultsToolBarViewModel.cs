using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using ArcGIS.Desktop.Framework.Dialogs;
using Isogeo.AddIn.Models;
using Isogeo.AddIn.Views.Search.AskNameWindow;
using Isogeo.Map.MapFunctions;
using Isogeo.Models;
using Isogeo.Models.Filters;
using Isogeo.Models.Network;
using Isogeo.Utils.LogManager;
using MVVMPattern;
using MVVMPattern.MediatorPattern;
using MVVMPattern.RelayCommand;

namespace Isogeo.AddIn.ViewsModels.Search.Results
{
    public class ResultsToolBarViewModel : ViewModelBase
    {
        private readonly RestFunctions _restFunctions;

        private readonly IMapFunctions _mapFunctions;

        private readonly FilterManager _filterManager;

        private bool _isUpdateCombo;

        private string _btnResultsContent;
        public string BtnResultsContent
        {
            get => _btnResultsContent;
            set
            {
                _btnResultsContent = value;
                OnPropertyChanged("BtnResultsContent");
            }
        }

        private ObservableCollection<FilterItem> _cmbSortingMethod;
        public ObservableCollection<FilterItem> CmbSortingMethod
        {
            get => _cmbSortingMethod;
            set
            {
                _cmbSortingMethod = value;
                OnPropertyChanged("CmbSortingMethod");
            }
        }

        private ObservableCollection<FilterItem> _cmbSortingDirection;
        public ObservableCollection<FilterItem> CmbSortingDirection
        {
            get => _cmbSortingDirection;
            set
            {
                _cmbSortingDirection = value;
                OnPropertyChanged("CmbSortingDirection");
            }
        }

        private void InitCmbs()
        {
            CmbSortingMethod = new ObservableCollection<FilterItem>
            {
                new FilterItem {Id = "relevance", Name = Language.Resources.Sorting_method_relevance},
                new FilterItem {Id = "title", Name = Language.Resources.Sorting_method_title},
                new FilterItem {Id = "modified", Name = Language.Resources.Sorting_method_modified},
                new FilterItem {Id = "created", Name = Language.Resources.Sorting_method_created},
                new FilterItem {Id = "_modified", Name = Language.Resources.Sorting_method_metadata_modified},
                new FilterItem {Id = "_created", Name = Language.Resources.Sorting_method_metadata_created}
            };

            CmbSortingDirection = new ObservableCollection<FilterItem>
            {
                new FilterItem {Id = "asc", Name = Language.Resources.Sorting_method_ascending},
                new FilterItem {Id = "desc", Name = Language.Resources.Sorting_method_descending}
            };

            _cmbSortingMethodSelectedItem = CmbSortingMethod[0];
            _filterManager.SetCmbSortingMethod(_cmbSortingMethodSelectedItem);
            OnPropertyChanged("CmbSortingMethodSelectedItem");

            _cmbSortingDirectionSelectedItem = CmbSortingDirection[1];
            _filterManager.SetCmbSortingDirection(_cmbSortingDirectionSelectedItem);
            OnPropertyChanged("CmbSortingDirectionSelectedItem");
        }

        private FilterItem _cmbSortingDirectionSelectedItem;
        public FilterItem CmbSortingDirectionSelectedItem
        {
            get => _cmbSortingDirectionSelectedItem;
            set
            {
                if (value == null)
                    return;
                _cmbSortingDirectionSelectedItem = value;
                _filterManager.SetCmbSortingDirection(_cmbSortingDirectionSelectedItem);
                Refresh();
                OnPropertyChanged("CmbSortingDirectionSelectedItem");
            }
        }

        private FilterItem _cmbSortingMethodSelectedItem;
        public FilterItem CmbSortingMethodSelectedItem
        {
            get => _cmbSortingMethodSelectedItem;
            set
            {
                if (value == null)
                    return;
                _cmbSortingMethodSelectedItem = value;
                _filterManager.SetCmbSortingMethod(_cmbSortingMethodSelectedItem);
                Refresh();
                OnPropertyChanged("CmbSortingMethodSelectedItem");
            }
        }

        private ICommand _btnResultsCommand;
        private ICommand _resetCommand;

        public void TotalResultsEvent(object obj)
        {
            DefineBtnResultsContent();
        }

        private void SetSortingDefaultEvent(object obj)
        {
            SetSortingDefault();
        }

        public ResultsToolBarViewModel(RestFunctions restFunctions, IMapFunctions mapFunctions, FilterManager filterManager)
        {
            _restFunctions = restFunctions;
            _mapFunctions = mapFunctions;
            _filterManager = filterManager;
            Mediator.Register("ChangeQuery", TotalResultsEvent);
            Mediator.Register("setSortingDefault", SetSortingDefaultEvent);
            BtnResultsContent = Language.Resources.Results;
            InitCmbs();
            SetSortingDefault();
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

        private static bool CanSave()
        {
            return true;
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

        private void Save()
        {
            var frm = new AskNameWindow(false, "");
            frm.ShowDialog();

            if (frm.isSave == false) return;

            if (!NewQuickSearchNameIsValid(frm.TxtQuickSearchName.Text))
                return;

            var newSearch = new Isogeo.Models.Configuration.Search
            {
                name = frm.TxtQuickSearchName.Text,
                query = _filterManager.GetQueryCombos(),
                box =  _filterManager.GetBoxRequest()
            };

            Variables.configurationManager.config.searchs.searchs.Add(newSearch);
            Variables.configurationManager.Save();

            Mediator.NotifyColleagues("AddNewQuickSearch", newSearch);
        }

        private async void Refresh()
        {
            var ob = _filterManager.GetOb();
            var od = _filterManager.GetOd();
            var box = _filterManager.GetBoxRequest();
            var query = _filterManager.GetQueryCombos();
            if (_isUpdateCombo) return;
            DefineBtnResultsContent();
            await _restFunctions.ReloadData(0, query, box, od, ob);
            _filterManager.SetSearchList(query);
        }

        public void SetSortingDefault()
        {
            _isUpdateCombo = true;
            switch (Variables.configurationManager.config.sortMethode)
            {
                case "relevance":
                    _cmbSortingMethodSelectedItem = CmbSortingMethod.First(x => x.Id == "relevance");
                    break;
                case "title":
                    _cmbSortingMethodSelectedItem = CmbSortingMethod.First(x => x.Id == "title");
                    break;
                case "modified":
                    _cmbSortingMethodSelectedItem = CmbSortingMethod.First(x => x.Id == "modified");
                    break;
                case "created":
                    _cmbSortingMethodSelectedItem = CmbSortingMethod.First(x => x.Id == "created");
                    break;
                case "_modified":
                    _cmbSortingMethodSelectedItem = CmbSortingMethod.First(x => x.Id == "_modified");
                    break;
                case "_created":
                    _cmbSortingMethodSelectedItem = CmbSortingMethod.First(x => x.Id == "_created");
                    break;
                default:
                    _cmbSortingMethodSelectedItem = CmbSortingMethod[0];
                    break;
            }

            _filterManager.SetCmbSortingMethod(_cmbSortingMethodSelectedItem);
            OnPropertyChanged("CmbSortingMethodSelectedItem");

            switch (Variables.configurationManager.config.sortDirection)
            {
                case "asc":
                    _cmbSortingDirectionSelectedItem = CmbSortingDirection.First(x => x.Id == "asc");
                    break;
                case "desc":
                    _cmbSortingDirectionSelectedItem = CmbSortingDirection.First(x => x.Id == "desc");
                    break;
                default:
                    _cmbSortingDirectionSelectedItem = CmbSortingDirection[1];
                    break;
            }
            _filterManager.SetCmbSortingDirection(_cmbSortingDirectionSelectedItem);
            OnPropertyChanged("CmbSortingDirectionSelectedItem");
            _isUpdateCombo = false;
        }

        public ICommand BtnResultsCommand
        {
            get
            {
                return _btnResultsCommand ?? (_btnResultsCommand = new RelayCommand(
                    x => RunBtnResults(),
                    y => CanRunBtnResults()));
            }
        }

        private void RunBtnResults()
        {
            Refresh();
        }

        private void DefineBtnResultsContent()
        {
            if (Variables.search == null || Variables.search.total.Equals(0))
            {
                BtnResultsContent = "0 " + Language.Resources.Result;
            }
            else
            {
                BtnResultsContent = Variables.search.total + " " + Language.Resources.Results;
            }
        }

        private bool CanRunBtnResults()
        {
            return (Variables.search != null && !Variables.search.total.Equals(0));
        }

        public ICommand ResetCommand
        {
            get
            {
                return _resetCommand ?? (_resetCommand = new RelayCommand(
                    x => RunReset(),
                    y => CanRunReset()));
            }
        }

        private async void RunReset()
        {
            var ob = _filterManager.GetOb();
            var od = _filterManager.GetOd();
            var box = _filterManager.GetBoxRequest();
            var query = _filterManager.GetQueryCombos();
            _restFunctions.SaveSearch(box, query);
            Mediator.NotifyColleagues("setSortingDefault", null);
            await _restFunctions.ResetData(box, od, ob);
            _filterManager.SetSearchList(query);
        }

        private static bool CanRunReset()
        {
            return true;
        }
    }
}
