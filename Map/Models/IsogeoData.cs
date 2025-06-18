namespace Isogeo.Map.Models
{
    public class IsogeoData
    {
        public string Type { get; }
        public string Title { get; }
        public string Url { get; }
        public string Name { get; }
        public string Creator { get; }
        public string Id { get; }

        public IsogeoData(string type, string title, string url, string name, string creator, string id)
        {
            Type = type;
            Title = title;
            Url = url;
            Name = name;
            Creator = creator;
            Id = id;
        }
    }
}
