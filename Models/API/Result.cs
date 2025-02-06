using System.Text.Json.Serialization;

namespace Isogeo.Models.API
{
    public class Result
    {
        [JsonPropertyName("_id")]
        public string? Id { get; set; }

        [JsonPropertyName("_created")]
        public string? Created { get; set; }

        [JsonPropertyName("_modified")]
        public string? Modified { get; set; }

        [JsonPropertyName("_deleted")]
        public bool? Deleted { get; set; }

        [JsonPropertyName("created")]
        public string? DataCreationDate { get; set; }

        [JsonPropertyName("modified")]
        public string? DataModificationDate { get; set; }

        [JsonPropertyName("_creator")]
        public Creator? Creator { get; set; }

        [JsonPropertyName("keywords")]
        public List<Keyword>? Keywords { get; set; }

        [JsonPropertyName("type")]
        public string? Type { get; set; }

        [JsonPropertyName("series")]
        public bool? Series { get; set; }

        [JsonPropertyName("scale")]
        public int? Scale { get; set; }

        [JsonPropertyName("distance")]
        public double? Distance { get; set; }

        [JsonPropertyName("validFrom")]
        public string? ValidFrom { get; set; }

        [JsonPropertyName("validTo")]
        public string? ValidTo { get; set; }

        [JsonPropertyName("validityComment")]
        public string? ValidityComment { get; set; }

        [JsonPropertyName("updateFrequency")]
        public string? UpdateFrequency { get; set; }

        [JsonPropertyName("encoding")]
        public string? Encoding { get; set; }

        [JsonPropertyName("collectionMethod")]
        public string? CollectionMethod { get; set; }

        [JsonPropertyName("collectionContext")]
        public string? CollectionContext { get; set; }

        [JsonPropertyName("editionProfile")]
        public string? EditionProfile { get; set; }

        [JsonPropertyName("title")]
        public string? Title { get; set; }

        [JsonPropertyName("abstract")]
        public string? Abstract { get; set; }

        [JsonPropertyName("format")]
        public string? Format { get; set; }

        [JsonPropertyName("formatVersion")]
        public string? FormatVersion { get; set; }

        [JsonPropertyName("language")]
        public string? Language { get; set; }

        [JsonPropertyName("envelope")]
        public Envelope? Envelope { get; set; }

        [JsonPropertyName("geometry")]
        public string? Geometry { get; set; }

        [JsonPropertyName("features")]
        public int? Features { get; set; }

        [JsonPropertyName("feature-attributes")]
        public List<FeatureAttributes>? FeatureAttributes { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("path")]
        public string? Path { get; set; }

        [JsonPropertyName("topologicalConsistency")]
        public string? TopologicalConsistency { get; set; }

        [JsonPropertyName("_abilities")]
        public List<string>? Abilities { get; set; }

        [JsonPropertyName("bbox")]
        public List<double>? Bbox { get; set; }

        [JsonPropertyName("layers")]
        public List<Layer>? Layers { get; set; }

        [JsonPropertyName("serviceLayers")]
        public List<ServiceLayer>? ServiceLayers { get; set; }

        [JsonPropertyName("tags")]
        public IDictionary<string, string>? Tags { get; set; }

        [JsonPropertyName("tagsLists")]
        public Tags? TagsLists { get; set; }

        [JsonPropertyName("contacts")]
        public List<ContactParent>? Contacts { get; set; }

        [JsonPropertyName("coordinate_system")]
        public CoordinateSystem? CoordinateSystem { get; set; }

        [JsonPropertyName("events")]
        public List<Event>? Events { get; set; }

        [JsonPropertyName("specifications")]
        public List<Specification>? Specifications { get; set; }

        [JsonPropertyName("conditions")]
        public List<Condition>? Conditions { get; set; }

        [JsonPropertyName("limitations")]
        public List<Limitation>? Limitations { get; set; }
    }	
}