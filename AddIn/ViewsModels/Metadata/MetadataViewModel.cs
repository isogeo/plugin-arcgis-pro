using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Isogeo.AddIn.Views.Metadata;
using Isogeo.Models.API;
using Isogeo.Utils;
using MVVMPattern;
using MVVMPattern.MediatorPattern;
using System.Globalization;
using System;
using Isogeo.Models;
using Isogeo.AddIn.Models.Metadata;
using System.Windows.Input;
using MVVMPattern.RelayCommand;
using Isogeo.AddIn.Views.Search.AskNameWindow;
using Isogeo.Utils.LogManager;
using System.Diagnostics;
using ArcGIS.Desktop.Framework.Dialogs;
using Isogeo.Utils.ConfigurationManager;

namespace Isogeo.AddIn.ViewsModels.Metadata
{
    class MetadataViewModel : ViewModelBase
    {
        private readonly IConfigurationManager _configurationManager;

        public void NotifyProperties(List<string> properties)
        {
            foreach (var property in properties)
            {
                OnPropertyChanged(property);
            }
        }

        public void CurrentResult(object result)
        {
            _currentResult = (Result)result;
            LoadLicenseList();
            LoadContactsLists();
            LoadLimitationList();
            var properties = new List<string>
            {
                "CreatedAt", "LastModification", "Language", "ContactOwner", "Title", "Owner", "Keywords",
                "Themes", "Conformity", "Description", "Srs", "Format", "FeatCount", "GeometryType", "Resolution",
                "Scale", "Typology", "Specification", "Envelope", "DataCreation", "DataUpdate", "UpdateFrequency",
                "ValidStart", "ValidEnd", "ValidComment", "Method", "Context", "Events"
            };
            NotifyProperties(properties);
        }

        public void UnRegisterMediator()
        {
            if (IsSubscribe)
            {
                Mediator.UnRegister(MediatorEvent.ResultSelected, CurrentResult);
                IsSubscribe = false;
            }
        }

        public void RegisterMediator()
        {
            if (!IsSubscribe)
            {
                Mediator.Register(MediatorEvent.ResultSelected, CurrentResult);
                IsSubscribe = true;
            }
        }

        public MetadataViewModel(IConfigurationManager configurationManager)
        {
            _configurationManager = configurationManager;
            IsSubscribe = false;
            ContactItemsList = new ObservableCollection<Contact>();
            OtherContactItemsList = new ObservableCollection<Contact>();
            RegisterMediator();
        }

        public bool IsSubscribe { get; private set; }

        private Result _currentResult;

        public string CreatedAt => _currentResult?.Created == null ? Isogeo.Language.Resources.NotReported : Formats.FormatDate(_currentResult.Created);

        public string LastModification => _currentResult?.Modified == null ? Isogeo.Language.Resources.NotReported : Formats.FormatDate(_currentResult.Modified);

        public string Language => _currentResult?.Language ?? Isogeo.Language.Resources.NotReported;

        public Contact ContactOwner
        {
            get
            {
                if (_currentResult?.Creator?.Contact == null)
                    return new Contact
                    {
                        AddressLine1 = Isogeo.Language.Resources.NotReported,
                        AddressLine2 = Isogeo.Language.Resources.NotReported,
                        AddressLine3 = Isogeo.Language.Resources.NotReported,
                        City = Isogeo.Language.Resources.NotReported,
                        CountryCode = Isogeo.Language.Resources.NotReported,
                        Email = Isogeo.Language.Resources.NotReported,
                        Name = Isogeo.Language.Resources.NotReported,
                        Organization = Isogeo.Language.Resources.NotReported,
                        Phone = Isogeo.Language.Resources.NotReported,
                        ZipCode = Isogeo.Language.Resources.NotReported
                    };

                return _currentResult?.Creator?.Contact;
            }
        }

        private static void FillContactWithEmpty(Contact contact)
        {
            if (string.IsNullOrWhiteSpace(contact.AddressLine1))
                contact.AddressLine1 = Isogeo.Language.Resources.NotReported;
            if (string.IsNullOrWhiteSpace(contact.ZipCode))
                contact.ZipCode = "";
            if (string.IsNullOrWhiteSpace(contact.Name))
                contact.Name = Isogeo.Language.Resources.NotReported;
            if (string.IsNullOrWhiteSpace(contact.AddressLine2))
                contact.AddressLine2 = "";
            if (string.IsNullOrWhiteSpace(contact.AddressLine3))
                contact.AddressLine3 = "";
            if (string.IsNullOrWhiteSpace(contact.CountryCode))
                contact.CountryCode = "";
            if (string.IsNullOrWhiteSpace(contact.Email))
                contact.Email = Isogeo.Language.Resources.NotReported;
            if (string.IsNullOrWhiteSpace(contact.Organization))
                contact.Organization = Isogeo.Language.Resources.NotReported;
            if (string.IsNullOrWhiteSpace(contact.Phone))
                contact.Phone = Isogeo.Language.Resources.NotReported;
            if (string.IsNullOrWhiteSpace(contact.City))
                contact.City = "";
        }

        private static void FillCity(Contact contact)
        {
            try
            {
                contact.City = contact.ZipCode + " " + contact.City + ", " + (new RegionInfo(contact.CountryCode).DisplayName);
                if (string.IsNullOrWhiteSpace(contact.City))
                    contact.City = Isogeo.Language.Resources.NotReported;
            }
            catch (ArgumentException argEx)
            {
                // The code was not a valid country code
                contact.City = contact.ZipCode + " " + contact.City;
                if (string.IsNullOrWhiteSpace(contact.City))
                    contact.City = Isogeo.Language.Resources.NotReported;
            }
            
        }

        public ObservableCollection<Contact> ContactItemsList { get; set; }
        public ObservableCollection<Contact> OtherContactItemsList { get; }

        private void LoadContactsLists()
        {
            ContactItemsList.Clear();
            OtherContactItemsList.Clear();

            if (_currentResult?.Contacts == null || _currentResult.Contacts.Count == 0) return;
            foreach (var contact in _currentResult.Contacts.Where(i => i != null))
            {
                if (contact.Contact == null)
                    contact.Contact = new Contact();

                FillContactWithEmpty(contact.Contact);

                if (contact.Contact.AddressLine2 != "")
                    contact.Contact.AddressLine1 += ", " + contact.Contact.AddressLine2;
                if (contact.Contact.AddressLine3 != "")
                    contact.Contact.AddressLine1 += ", " + contact.Contact.AddressLine3;

                FillCity(contact.Contact);

                if (contact.Role == "pointOfContact")
                    ContactItemsList.Add(contact.Contact);
                else
                    OtherContactItemsList.Add(contact.Contact);
            }
        }

        public string Title => _currentResult?.Title ?? Isogeo.Language.Resources.NotReported;

        public string Owner => _currentResult?.Creator?.Contact?.Name ?? Isogeo.Language.Resources.NotReported;

        public string Keywords => _currentResult?.TagsLists?.Keywords == null ?
            Isogeo.Language.Resources.NotReported : string.Join(" ; ", _currentResult.TagsLists.Keywords.ToArray());

        public string Themes => _currentResult?.TagsLists?.ThemeInspire == null ?
            Isogeo.Language.Resources.NotReported : string.Join(" ; ", _currentResult.TagsLists.ThemeInspire.ToArray());

        public string Conformity
        {
            get
            {
                if (_currentResult?.TagsLists?.Conformity == null)
                    return Isogeo.Language.Resources.NotReported;
                return _currentResult.TagsLists.Conformity == 1 ? Isogeo.Language.Resources.Yes : Isogeo.Language.Resources.No;
            }
        }

        public string Description => _currentResult?.Abstract ?? Isogeo.Language.Resources.NotReported;

        public string Srs
        {
            get
            {
                if (_currentResult?.CoordinateSystem == null)
                    return Isogeo.Language.Resources.NotReported;
                return _currentResult.CoordinateSystem.Name + " (EPSG:" +
                       _currentResult.CoordinateSystem.Code + ")";
            }
        }

        public string Format
        {
            get
            {
                if (_currentResult?.TagsLists?.Formats == null)
                    return Isogeo.Language.Resources.NotReported;
                return _currentResult.TagsLists.Formats.Count > 0 ? _currentResult.TagsLists.Formats[0] : Isogeo.Language.Resources.NotReported;
            }
        }

        public string Name => _currentResult?.Name == null ? Isogeo.Language.Resources.NotReported : _currentResult.Name.ToString();

        public string FeatCount => _currentResult?.Features == null ? Isogeo.Language.Resources.NotReported : _currentResult.Features.ToString();
        public string GeometryType => _currentResult?.Geometry ?? Isogeo.Language.Resources.NotReported;

        public string Resolution
        {
            get
            {
                if (_currentResult?.Distance == null)
                    return Isogeo.Language.Resources.NotReported;
                return _currentResult.Distance + " m";
            }
        }

        public string Scale
        {
            get
            {
                if (_currentResult?.Scale == null)
                    return Isogeo.Language.Resources.NotReported;
                return "1:" + _currentResult.Scale;
            }
        }

        public string Typology => _currentResult?.TopologicalConsistency ?? Isogeo.Language.Resources.NotReported;

        private string _specification;
        public string Specification
        {
            get
            {
                _specification = "";
                if (_currentResult?.Specifications == null)
                    return Isogeo.Language.Resources.NotReported;
                foreach (var specification in _currentResult.Specifications.Where(i => i != null))
                {
                    var conform = Isogeo.Language.Resources.Conform;
                    if (specification.Conformant == false)
                        conform = Isogeo.Language.Resources.No + " " + Isogeo.Language.Resources.Conform.ToLower();

                    var dateSpec = Formats.FormatDate(specification.SpecificationDetails.Published);
                    if (_specification != "")
                        _specification += "/n";
                    _specification += specification.SpecificationDetails.Name + "(" + dateSpec + ") : " + conform;
                }

                return _specification;
            }
        }

        public string Envelope => "";

        public string DataCreation => _currentResult?.DataCreationDate == null ? Isogeo.Language.Resources.NotReported : Formats.FormatDate(_currentResult.DataCreationDate);

        public string DataUpdate => _currentResult?.DataModificationDate == null ? Isogeo.Language.Resources.NotReported : Formats.FormatDate(_currentResult.DataModificationDate);

        private string _updateFrequency;
        public string UpdateFrequency
        {
            get
            {
                _updateFrequency = "";
                if (_currentResult?.UpdateFrequency == null)
                    return Isogeo.Language.Resources.NotReported;
                var stringLength = _currentResult.UpdateFrequency.Length;
                var alphaPart = _currentResult.UpdateFrequency.Substring(stringLength - 1);
                var numberPart = _currentResult.UpdateFrequency.Substring(1, stringLength - 2);

                _updateFrequency = numberPart;
                switch (alphaPart.ToUpper())
                {
                    case "Y":
                        _updateFrequency += (int.Parse(numberPart) > 1) ? (" " + Isogeo.Language.Resources.Years) : (" " + Isogeo.Language.Resources.Year);
                        break;
                    case "M":
                        _updateFrequency += (int.Parse(numberPart) > 1) ? (" " + Isogeo.Language.Resources.Months) : (" " + Isogeo.Language.Resources.Month);
                        break;
                    case "W":
                        _updateFrequency += (int.Parse(numberPart) > 1) ? (" " + Isogeo.Language.Resources.Weeks) : (" " + Isogeo.Language.Resources.Week);
                        break;
                    case "D":
                        _updateFrequency += (int.Parse(numberPart) > 1) ? (" " + Isogeo.Language.Resources.Days) : (" " + Isogeo.Language.Resources.Day);
                        break;
                    case "H":
                        _updateFrequency += (int.Parse(numberPart) > 1) ? (" " + Isogeo.Language.Resources.Hours) : (" " + Isogeo.Language.Resources.Hour);
                        break;
                }
                return _updateFrequency;
            }
        }

        public string ValidStart => _currentResult?.ValidFrom == null ? Isogeo.Language.Resources.NotReported : Formats.FormatDate(_currentResult.ValidFrom);

        public string ValidEnd => _currentResult?.ValidTo == null ? Isogeo.Language.Resources.NotReported : Formats.FormatDate(_currentResult.ValidTo);

        public string ValidComment => _currentResult?.ValidityComment == null ? Isogeo.Language.Resources.NotReported : Formats.FormatDate(_currentResult.ValidityComment);

        public string Method => _currentResult?.CollectionMethod ?? Isogeo.Language.Resources.NotReported;

        public string Context => _currentResult?.CollectionContext ?? Isogeo.Language.Resources.NotReported;

        private List<Event> _events;
        public List<Event> Events
        {
            get
            {
                if (_currentResult?.Events == null)
                    return null;
                _events = new List<Event>();

                foreach (var eventObj in _currentResult.Events.Where(i => i != null))
                {
                    if (eventObj.Kind == "update")
                    {
                        _events.Add(eventObj);
                    }
                }

                return _events;
            }
        }

        public bool MetadataAttributesIsVisible => _currentResult.Type is not ("service" or "rasterDataset" or "resource");

        public List<MetadataAttribute> MetadataAttributes => _currentResult?.FeatureAttributes?
            .Where(x => x != null).Select(y => new MetadataAttribute(y.Name, y.Alias, y.Comment, y.DataType, y.Description)).ToList() 
            ?? new List<MetadataAttribute>();

        public ObservableCollection<LicenseItem> LicenseItemsList { get; } = new();
        public ObservableCollection<LimitationItem> LimitationItemsList { get; } = new();

        private void LoadLicenseList()
        {
            LicenseItemsList.Clear();
            if (_currentResult?.Conditions == null || _currentResult.Conditions.Count <= 0) return;
            foreach (var license in _currentResult.Conditions.Where(i => i != null))
            {
                var licenseItem = new LicenseItem();
                licenseItem.Init(license);
                LicenseItemsList.Add(licenseItem);
            }
        }

        private void LoadLimitationList()
        {
            LimitationItemsList.Clear();
            if (_currentResult?.Limitations == null || _currentResult.Limitations.Count <= 0) return;
            foreach (var limitation in _currentResult.Limitations.Where(i => i != null))
            {
                var limitationItem = new LimitationItem();
                limitationItem.Init(limitation);
                LimitationItemsList.Add(limitationItem);
            }
        }

        private ICommand _editMetadataOnlineCommand;
        public ICommand EditMetadataOnlineCommand
        {
            get
            {
                return _editMetadataOnlineCommand ??= new RelayCommand(
                    x => OpenMetadataEditionOnline(),
                    y => CanEditMetadataOnline());
            }
        }

        private static bool CanEditMetadataOnline()
        {
            return true;
        }

        private void OpenMetadataEditionOnline()
        {
            string metadataEditionUrl = null;
            try
            {
                if (_currentResult.Creator?.Id == null || _currentResult.Id == null)
                    throw new Exception($"missing metadata Id(s) (creator Id: '{_currentResult.Creator?.Id}', " +
                        $"metadata Id: '{_currentResult.Id}')");
                
                metadataEditionUrl += $"{_configurationManager.Config.AppUrl}groups/{_currentResult.Creator.Id}" +
                                      $"/resources/{_currentResult.Id}/identification";
                Process.Start(new ProcessStartInfo(metadataEditionUrl) { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                MessageBox.Show(Isogeo.Language.Resources.Error_Open_External_Tool, "Isogeo");
                Log.Logger.Error(ex.Message);
            }
        }
    }
}