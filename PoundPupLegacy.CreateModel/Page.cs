namespace PoundPupLegacy.CreateModel;

public abstract record Page : SimpleTextNode
{
    private Page() { }
    public required SimpleTextNodeDetails SimpleTextNodeDetails { get; init; }
    public sealed record ToCreate : Page, SimpleTextNodeToCreate
    {
        public required Identification.Possible Identification { get; init; }
        public required NodeDetails.ForCreate NodeDetails { get; init; }
    }
    public sealed record ToUpdate : Page, SimpleTextNodeToUpdate
    {
        public required Identification.Certain Identification { get; init; }
        public required NodeDetails.ForUpdate NodeDetails { get; init; }
    }
}
