using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Isogeo.AddIn.Views.Metadata;
using Isogeo.Models.API;
using Isogeo.Utils;
using MVVMPattern;
using MVVMPattern.MediatorPattern;

namespace Isogeo.AddIn.ViewsModels.Metadata
{
    class MetadataViewModel : ViewModelBase
    {
        public void NotifyProperties(List<string> properties)
        {
            foreach (var property in properties)
            {
                OnPropertyChanged(property);
            }
        }

        public void CurrentResult(object result)
        {
            _currentResult = (Result) result;
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
                Mediator.UnRegister("CurrentResult", CurrentResult);
                IsSubscribe = false;
            }
        }

        public void RegisterMediator()
        {
            if (!IsSubscribe)
            {
                Mediator.Register("CurrentResult", CurrentResult);
                IsSubscribe = true;
            }
        }

        public MetadataViewModel()
        {
            IsSubscribe = false;
            ContactItemsList = new ObservableCollection<Contact>();
            OtherContactItemsList = new ObservableCollection<Contact>();
            RegisterMediator();
        }

        public bool IsSubscribe { get; private set; }

        private Result _currentResult;

        public string CreatedAt => _currentResult?._created == null ? Isogeo.Language.Resources.NotReported : Formats.FormatDate(_currentResult._created); 

        public string LastModification => _currentResult?._modified == null ? Isogeo.Language.Resources.NotReported : Formats.FormatDate(_currentResult._modified);

        public string Language => _currentResult?.language ?? Isogeo.Language.Resources.NotReported;

        public Contact ContactOwner
        {
            get
            {
                if (_currentResult?._creator?.contact == null)
                    return new Contact
                    {
                        AddressLine1 = Isogeo.Language.Resources.NotReported,
                        AddressLine2 = Isogeo.Language.Resources.NotReported,
                        City = Isogeo.Language.Resources.NotReported, 
                        Country = Isogeo.Language.Resources.NotReported,
                        Email = Isogeo.Language.Resources.NotReported,
                        Name = Isogeo.Language.Resources.NotReported,
                        Organization = Isogeo.Language.Resources.NotReported,
                        Phone = Isogeo.Language.Resources.NotReported,
                        ZipCode = Isogeo.Language.Resources.NotReported
                    };
                return _currentResult._creator.contact;

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
            if (string.IsNullOrWhiteSpace(contact.Country))
                contact.Country = "";
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
            contact.City = contact.ZipCode + " " + contact.City + " " + contact.Country;
            if (string.IsNullOrWhiteSpace(contact.City))
                contact.City = Isogeo.Language.Resources.NotReported;
        }

        public ObservableCollection<Contact> ContactItemsList { get; set; } 
        public ObservableCollection<Contact> OtherContactItemsList { get; }

        private void LoadContactsLists()
        {
            ContactItemsList.Clear();
            OtherContactItemsList.Clear();

            if (_currentResult?.contacts == null || _currentResult.contacts.Count == 0) return;
            foreach (var contact in _currentResult.contacts.Where(i => i != null))
            {
                if (contact.contact == null)
                    contact.contact = new Contact();

                FillContactWithEmpty(contact.contact);

                if (contact.contact.AddressLine2 != "")
                    contact.contact.AddressLine1 += " " + contact.contact.AddressLine2;

                FillCity(contact.contact);

                if (contact.role == "pointOfContact")
                    ContactItemsList.Add(contact.contact);
                else
                    OtherContactItemsList.Add(contact.contact);
            }
        }

        public string Title => _currentResult?.title ?? Isogeo.Language.Resources.NotReported;

        public string Owner => _currentResult?._creator?.contact?.Name ?? Isogeo.Language.Resources.NotReported;

        public string Keywords => _currentResult?.tagsLists?.Keywords == null ? 
            Isogeo.Language.Resources.NotReported : string.Join(" ; ", _currentResult.tagsLists.Keywords.ToArray());

        public string Themes => _currentResult?.tagsLists?.ThemeInspire == null ? 
            Isogeo.Language.Resources.NotReported : string.Join(" ; ", _currentResult.tagsLists.ThemeInspire.ToArray());

        public string Conformity
        {
            get
            {
                if (_currentResult?.tagsLists?.conformity == null)
                    return Isogeo.Language.Resources.NotReported;
                return _currentResult.tagsLists.conformity == 1 ? Isogeo.Language.Resources.Yes : Isogeo.Language.Resources.No;
            }
        }

        public string Description => _currentResult?.@abstract ?? Isogeo.Language.Resources.NotReported;

        public string Srs
        {
            get
            {
                if (_currentResult?.coordinate_system == null)
                    return Isogeo.Language.Resources.NotReported;
                return _currentResult.coordinate_system.name + " (EPSG:" +
                       _currentResult.coordinate_system.code + ")";
            }
        }

        public string Format
        {
            get
            {
                if (_currentResult?.tagsLists?.Formats == null)
                    return Isogeo.Language.Resources.NotReported;
                return _currentResult.tagsLists.Formats.Count > 0 ? _currentResult.tagsLists.Formats[0] : Isogeo.Language.Resources.NotReported;
            }
        }

        public string FeatCount => _currentResult?.features == null ? Isogeo.Language.Resources.NotReported : _currentResult.features.ToString();
        public string GeometryType => _currentResult?.geometry ?? Isogeo.Language.Resources.NotReported;

        public string Resolution
        {
            get
            {
                if (_currentResult?.distance == null)
                    return Isogeo.Language.Resources.NotReported;
                return _currentResult.distance + " m";
            }
        }

        public string Scale
        {
            get
            {
                if (_currentResult?.scale == null)
                    return Isogeo.Language.Resources.NotReported;
                return "1:" + _currentResult.scale;
            }
        }

        public string Typology => _currentResult?.topologicalConsistency ?? Isogeo.Language.Resources.NotReported;

        private string _specification;
        public string Specification
        {
            get
            {
                _specification = "";
                if (_currentResult?.specifications == null)
                    return Isogeo.Language.Resources.NotReported;
                foreach (var specification in _currentResult.specifications.Where(i => i != null))
                {
                    var conform = Isogeo.Language.Resources.Conform;
                    if (specification.conformant == false) 
                        conform = Isogeo.Language.Resources.No + " " + Isogeo.Language.Resources.Conform.ToLower();

                    var dateSpec = Formats.FormatDate(specification.specification.published);
                    if (_specification != "") 
                        _specification += "/n";
                    _specification += specification.specification.name + "(" + dateSpec + ") : " + conform;
                }

                return _specification;
            }
        }

        public string Envelope => "";

        public string DataCreation => _currentResult?._created == null ? Isogeo.Language.Resources.NotReported : Formats.FormatDate(_currentResult._created);

        public string DataUpdate => _currentResult?._modified == null ? Isogeo.Language.Resources.NotReported : Formats.FormatDate(_currentResult._modified);

        public string UpdateFrequency => _currentResult?.updateFrequency ?? Isogeo.Language.Resources.NotReported;

        public string ValidStart => _currentResult?.validFrom == null ? Isogeo.Language.Resources.NotReported : Formats.FormatDate(_currentResult.validFrom);

        public string ValidEnd => _currentResult?.validTo == null ? Isogeo.Language.Resources.NotReported : Formats.FormatDate(_currentResult.validTo);

        public string ValidComment => _currentResult?.validityComment == null ? Isogeo.Language.Resources.NotReported : Formats.FormatDate(_currentResult.validityComment);

        public string Method => _currentResult?.collectionMethod ?? Isogeo.Language.Resources.NotReported;

        public string Context => _currentResult?.collectionContext ?? Isogeo.Language.Resources.NotReported;

        private List<Event> _events;
        public List<Event> Events
        {
            get
            {
                if (_currentResult?.events == null)
                    return null;
                _events = new List<Event>();

                foreach (var eventObj in _currentResult.events.Where(i => i != null))
                {
                    if (eventObj.kind == "update")
                    {
                        _events.Add(eventObj);
                    }
                }

                return _events;
            }
        }

        public ObservableCollection<LicenseItem> LicenseItemsList { get; } = new ObservableCollection<LicenseItem>();
        public ObservableCollection<LimitationItem> LimitationItemsList { get; } = new ObservableCollection<LimitationItem>();

        private void LoadLicenseList()
        {
            LicenseItemsList.Clear();
            if (_currentResult?.conditions == null || _currentResult.conditions.Count <= 0) return;
            foreach (var license in _currentResult.conditions.Where(i => i != null))
            {
                var licenseItem = new LicenseItem();
                licenseItem.Init(license);
                LicenseItemsList.Add(licenseItem);
            }
        }

        private void LoadLimitationList()
        {
            LimitationItemsList.Clear();
            if (_currentResult?.limitations == null || _currentResult.limitations.Count <= 0) return;
            foreach (var limitation in _currentResult.limitations.Where(i=> i != null))
            {
                var limitationItem = new LimitationItem();
                limitationItem.Init(limitation);
                LimitationItemsList.Add(limitationItem);
            }
        }
    }
}

