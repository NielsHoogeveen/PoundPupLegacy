namespace PoundPupLegacy.CreateModel;

public abstract record BlogPost : SimpleTextNode
{
    private BlogPost() { }
    public required SimpleTextNodeDetails SimpleTextNodeDetails { get; init; }
    public sealed record ToCreate : BlogPost, SimpleTextNodeToCreate
    {
        public required Identification.Possible Identification { get; init; }
        public required NodeDetails.NodeDetailsForCreate NodeDetails { get; init; }
    }
    public sealed record ToUpdate : BlogPost, SimpleTextNodeToUpdate
    {
        public required Identification.Certain Identification { get; init; }
        public required NodeDetails.ForUpdate NodeDetails { get; init; }
    }
}
