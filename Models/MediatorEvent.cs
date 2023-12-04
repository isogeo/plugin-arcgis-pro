namespace Isogeo.Models
{
    public class MediatorEvent
    {
        private MediatorEvent(string value) { Value = value; }

        public string Value { get; }

        public static MediatorEvent ChangeBox => new("ChangeBox");
        public static MediatorEvent ChangeQuery => new("ChangeQuery");
        public static MediatorEvent ClearResults => new("ClearResults");
        public static MediatorEvent ChangeOffset => new ("ChangeOffset");
        public static MediatorEvent EnableDockableWindowIsogeo => new("EnableDockableWindowIsogeo");
        public static MediatorEvent ChangeQuickSearch => new("ChangeQuickSearch");
        public static MediatorEvent CurrentResult => new("CurrentResult");
        public static MediatorEvent AddNewQuickSearch => new("AddNewQuickSearch");
        public static MediatorEvent SetSortingDefault => new("SetSortingDefault");
        public static MediatorEvent UserAuthentication => new("UserAuthentication");
        

        public static implicit operator string(MediatorEvent item) { return item.Value; }

        public override string ToString()
        {
            return Value;
        }
    }
}
