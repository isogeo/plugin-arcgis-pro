using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Isogeo.Models.API
{
    public class Result
    {
        [JsonPropertyName("_id")]
        public string _id { get; set; }

        [JsonPropertyName("_created")]
        public string _created { get; set; }

        [JsonPropertyName("_modified")]
        public string _modified { get; set; }

        [JsonPropertyName("_deleted")]
        public bool _deleted { get; set; }

        [JsonPropertyName("created")]
        public string created { get; set; }

        [JsonPropertyName("modified")]
        public string modified { get; set; }

        [JsonPropertyName("_creator")]
        public Creator _creator { get; set; }

        [JsonPropertyName("keywords")]
        public List<Keyword> keywords { get; set; }

        [JsonPropertyName("type")]
        public string type { get; set; }

        [JsonPropertyName("series")]
        public bool series { get; set; }

        [JsonPropertyName("scale")]
        public int scale { get; set; }

        [JsonPropertyName("distance")]
        public double distance { get; set; }

        [JsonPropertyName("validFrom")]
        public string validFrom { get; set; }

        [JsonPropertyName("validTo")]
        public string validTo { get; set; }

        [JsonPropertyName("validityComment")]
        public string validityComment { get; set; }

        [JsonPropertyName("updateFrequency")]
        public string updateFrequency { get; set; }

        [JsonPropertyName("encoding")]
        public string encoding { get; set; }

        [JsonPropertyName("collectionMethod")]
        public string collectionMethod { get; set; }

        [JsonPropertyName("collectionContext")]
        public string collectionContext { get; set; }

        [JsonPropertyName("editionProfile")]
        public string editionProfile { get; set; }

        [JsonPropertyName("title")]
        public string title { get; set; }

        [JsonPropertyName("@abstract")]
        public string @abstract { get; set; }

        [JsonPropertyName("format")]
        public string format { get; set; }

        [JsonPropertyName("formatVersion")]
        public string formatVersion { get; set; }

        [JsonPropertyName("language")]
        public string language { get; set; }

        [JsonPropertyName("envelope")]
        public Envelope envelope { get; set; }

        [JsonPropertyName("geometry")]
        public string geometry { get; set; }

        [JsonPropertyName("features")]
        public int features { get; set; }

        [JsonPropertyName("name")]
        public string name { get; set; }

        [JsonPropertyName("path")]
        public string path { get; set; }

        [JsonPropertyName("topologicalConsistency")]
        public string topologicalConsistency { get; set; }

        [JsonPropertyName("_abilities")]
        public List<string> _abilities { get; set; }

        [JsonPropertyName("bbox")]
        public List<double> bbox { get; set; }

        //[JsonPropertyName("links")]
        //public List<Link> links { get; set; }

        [JsonPropertyName("layers")]
        public List<Layer> layers { get; set; }

        [JsonPropertyName("serviceLayers")]
        public List<ServiceLayer> serviceLayers { get; set; }

        [JsonPropertyName("tags")]
        public IDictionary<string, string> tags { get; set; }

        [JsonPropertyName("tagsLists")]
        public Tags tagsLists { get; set; }

        [JsonPropertyName("contacts")]
        public List<ContactParent> contacts { get; set; }

        [JsonPropertyName("coordinate_system")]
        public Coordinate_system coordinate_system { get; set; }

        [JsonPropertyName("events")]
        public List<Event> events { get; set; }

        [JsonPropertyName("specifications")]
        public List<Specification> specifications { get; set; }

        [JsonPropertyName("conditions")]
        public List<Condition> conditions { get; set; }

        [JsonPropertyName("limitations")]
        public List<Limitation> limitations { get; set; }
    }	
}
