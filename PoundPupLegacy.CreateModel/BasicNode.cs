namespace PoundPupLegacy.CreateModel;

public abstract record BasicNode: Node
{
    private BasicNode() { }
    public sealed record ToCreate : BasicNode, NodeToCreate
    {
        public required Identification.Possible Identification { get; init; }
        public required NodeDetails.ForCreate NodeDetails { get; init; }
    }
    public sealed record ToUpdate : BasicNode, NodeToUpdate
    {
        public required Identification.Certain Identification { get; init; }
        public required NodeDetails.ForUpdate NodeDetails { get; init; }
    }
}
