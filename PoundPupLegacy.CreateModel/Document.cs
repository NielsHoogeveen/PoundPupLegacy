namespace PoundPupLegacy.CreateModel;

public abstract record Document : SimpleTextNode
{
    private Document() { }
    public required DocumentDetails DocumentDetails { get; init; }
    public required SimpleTextNodeDetails SimpleTextNodeDetails { get; init; }
    public abstract Identification Identification { get; }
    public abstract NodeDetails NodeDetails { get; }
    public sealed record ToCreate : Document, SimpleTextNodeToCreate
    {
        public required Identification.Possible IdentificationForCreate { get; init; }
        public required NodeDetails.NodeDetailsForCreate NodeDetailsForCreate { get; init; }

        public override Identification Identification => IdentificationForCreate;

        public override NodeDetails NodeDetails => NodeDetailsForCreate;
    }
    public sealed record DocumentToUpdate : Document, SimpleTextNodeToUpdate
    {
        public override Identification Identification => IdentificationCertain;

        public override NodeDetails NodeDetails => NodeDetailsForUpdate;

        public required Identification.Certain IdentificationCertain { get; init; }
        public required NodeDetails.ForUpdate NodeDetailsForUpdate { get; init; }
    }
}

public sealed record DocumentDetails
{
    public required FuzzyDate? Published { get; init; }
    public required string? SourceUrl { get; init; }
    public required int? DocumentTypeId { get; init; }
}
