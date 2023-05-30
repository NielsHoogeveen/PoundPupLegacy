namespace PoundPupLegacy.CreateModel;

public abstract record Discussion : SimpleTextNode
{
    private Discussion() { }
    public required SimpleTextNodeDetails SimpleTextNodeDetails { get; init; }
    public sealed record ToCreate : Discussion, SimpleTextNodeToCreate
    {
        public required Identification.Possible Identification { get; init; }
        public required NodeDetails.ForCreate NodeDetails { get; init; }
    }
    public sealed record ToUpdate : Discussion, SimpleTextNodeToUpdate
    {
        public required Identification.Certain Identification { get; init; }
        public required NodeDetails.ForUpdate NodeDetails { get; init; }
    }
}
