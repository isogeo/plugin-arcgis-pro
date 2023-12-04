using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using ArcGIS.Desktop.Framework.Dialogs;
using Isogeo.AddIn.Models.FilterManager;
using Isogeo.AddIn.Models.Filters.Components;
using Isogeo.AddIn.Views.Search.AskNameWindow;
using Isogeo.Models;
using Isogeo.Models.Configuration;
using Isogeo.Network;
using Isogeo.Utils.LogManager;
using MVVMPattern;
using MVVMPattern.MediatorPattern;
using MVVMPattern.RelayCommand;

namespace Isogeo.AddIn.ViewsModels.Search.Results
{
    public class ResultsToolBarViewModel : ViewModelBase
    {
        private readonly INetworkManager _networkManager;

        private readonly IFilterManager _filterManager;

        private readonly ConfigurationManager _configurationManager;

        private bool _isUpdateCombo;

        private string _btnResultsContent;
        public string BtnResultsContent
        {
            get => _btnResultsContent;
            set
            {
                _btnResultsContent = value;
                OnPropertyChanged(nameof(BtnResultsContent));
            }
        }

        private ObservableCollection<FilterItem> _cmbSortingMethod;
        public ObservableCollection<FilterItem> CmbSortingMethod
        {
            get => _cmbSortingMethod;
            set
            {
                _cmbSortingMethod = value;
                OnPropertyChanged(nameof(CmbSortingMethod));
            }
        }

        private ObservableCollection<FilterItem> _cmbSortingDirection;
        public ObservableCollection<FilterItem> CmbSortingDirection
        {
            get => _cmbSortingDirection;
            set
            {
                _cmbSortingDirection = value;
                OnPropertyChanged(nameof(CmbSortingDirection));
            }
        }

        private void InitCmbs()
        {
            CmbSortingMethod = new ObservableCollection<FilterItem>
            {
                new("relevance", Language.Resources.Sorting_method_relevance),
                new("title", Language.Resources.Sorting_method_title),
                new("modified", Language.Resources.Sorting_method_modified),
                new("created", Language.Resources.Sorting_method_created),
                new("_modified", Language.Resources.Sorting_method_metadata_modified),
                new("_created", Language.Resources.Sorting_method_metadata_created)
            };

            CmbSortingDirection = new ObservableCollection<FilterItem>
            {
                new("asc", Language.Resources.Sorting_method_ascending),
                new("desc", Language.Resources.Sorting_method_descending)
            };

            _cmbSortingMethodSelectedItem = CmbSortingMethod[0];
            _filterManager.SetCmbSortingMethod(_cmbSortingMethodSelectedItem);
            OnPropertyChanged(nameof(CmbSortingMethodSelectedItem));

            _cmbSortingDirectionSelectedItem = CmbSortingDirection[1];
            _filterManager.SetCmbSortingDirection(_cmbSortingDirectionSelectedItem);
            OnPropertyChanged(nameof(CmbSortingDirectionSelectedItem));
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
                //Task.Run(async () => await Refresh());
                OnPropertyChanged(nameof(CmbSortingDirectionSelectedItem));
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
                //Task.Run(async () => await Refresh());
                OnPropertyChanged(nameof(CmbSortingMethodSelectedItem));
            }
        }

        private async void Refresh_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            await Refresh();
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

        public ResultsToolBarViewModel(INetworkManager networkManager, IFilterManager filterManager, ConfigurationManager configurationManager)
        {
            _networkManager = networkManager;
            _filterManager = filterManager;
            _configurationManager = configurationManager;
            Mediator.Register("ChangeQuery", TotalResultsEvent);
            Mediator.Register("setSortingDefault", SetSortingDefaultEvent);
            BtnResultsContent = Language.Resources.Results;
            InitCmbs();
            SetSortingDefault();
            CmbSortingDirectionSelectedItem.PropertyChanged += Refresh_PropertyChanged;
            CmbSortingMethodSelectedItem.PropertyChanged += Refresh_PropertyChanged;
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

        private static bool CanSave()
        {
            return true;
        }

        private bool IsDuplicateQuickSearchName(string name)
        {
            return _configurationManager.config.Searchs.SearchDetails.Any(search => search.Name == name);
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

        private void Save()
        {
            var frm = new AskNameWindow(false, "", _configurationManager);
            frm.ShowDialog();

            if (frm.isSave == false) return;

            if (!NewQuickSearchNameIsValid(frm.TxtQuickSearchName.Text))
                return;

            var newSearch = new Isogeo.Models.Configuration.Search
            {
                Name = frm.TxtQuickSearchName.Text,
                Query = _filterManager.GetQueryCombos(),
                Box =  _filterManager.GetBoxRequest()
            };

            _configurationManager.config.Searchs.SearchDetails.Add(newSearch);
            _configurationManager.Save();

            Mediator.NotifyColleagues("AddNewQuickSearch", newSearch);
        }

        private async Task Refresh()
        {
            var ob = _filterManager.GetOb();
            var od = _filterManager.GetOd();
            var box = _filterManager.GetBoxRequest();
            var query = _filterManager.GetQueryCombos();
            if (_isUpdateCombo) return;
            DefineBtnResultsContent(); // todo
            await _networkManager.LoadData(query, 0, box, od, ob);
            _filterManager.SetSearchList(query);
        }

        public void SetSortingDefault()
        {
            _isUpdateCombo = true;
            switch (_configurationManager.config.sortMethode)
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
            OnPropertyChanged(nameof(CmbSortingMethodSelectedItem));

            switch (_configurationManager.config.SortDirection)
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
            OnPropertyChanged(nameof(CmbSortingDirectionSelectedItem));
            _isUpdateCombo = false;
        }

        public ICommand BtnResultsCommand
        {
            get
            {
                return _btnResultsCommand ??= new RelayCommand(
                    x => RunBtnResults(),
                    y => CanRunBtnResults());
            }
        }

        private void RunBtnResults()
        {
            Refresh();
        }

        private void DefineBtnResultsContent()
        {
            if (Variables.search == null || Variables.search.Total.Equals(0))
            {
                BtnResultsContent = "0 " + Language.Resources.Result;
            }
            else
            {
                BtnResultsContent = Variables.search.Total + " " + Language.Resources.Results;
            }
        }

        private static bool CanRunBtnResults()
        {
            return (Variables.search != null && !Variables.search.Total.Equals(0));
        }

        public ICommand ResetCommand
        {
            get
            {
                return _resetCommand ??= new RelayCommand(
                    x => RunReset(),
                    y => CanRunReset());
            }
        }

        private async void RunReset()
        {
            var ob = _filterManager.GetOb();
            var od = _filterManager.GetOd();
            var box = _filterManager.GetBoxRequest();
            var query = _filterManager.GetQueryCombos();
            _networkManager.SaveSearch(box, query);
            Mediator.NotifyColleagues("setSortingDefault", null);
            await _networkManager.ResetData(od, ob);
            _filterManager.SetSearchList("");
        }

        private static bool CanRunReset()
        {
            return true;
        }
    }
}
