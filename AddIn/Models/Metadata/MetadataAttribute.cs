namespace Isogeo.AddIn.Models.Metadata
{
    internal class MetadataAttribute
    {
        public MetadataAttribute(string name, string alias, string comment, string type, string description)
        {
            Name = name;
            Alias = alias;
            Type = type;
            Comment = comment;
            Description = description;
        }

        public string Name { get; }
        
        public string Alias { get; }

        public string Comment { get; }

        public string Type { get; }
        
        public string Description { get; }
    }
}
