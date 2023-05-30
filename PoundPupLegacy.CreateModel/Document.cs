namespace PoundPupLegacy.CreateModel;

public abstract record Document : SimpleTextNode
{
    private Document() { }
    public required DocumentDetails DocumentDetails { get; init; }
    public required SimpleTextNodeDetails SimpleTextNodeDetails { get; init; }
    public sealed record ToCreate : Document, SimpleTextNodeToCreate
    {
        public required Identification.Possible Identification { get; init; }
        public required NodeDetails.NodeDetailsForCreate NodeDetails { get; init; }
    }
    public sealed record DocumentToUpdate : Document, SimpleTextNodeToUpdate
    {
        public required Identification.Certain Identification { get; init; }
        public required NodeDetails.ForUpdate NodeDetails { get; init; }
    }
}

public sealed record DocumentDetails
{
    public required FuzzyDate? Published { get; init; }
    public required string? SourceUrl { get; init; }
    public required int? DocumentTypeId { get; init; }
}
