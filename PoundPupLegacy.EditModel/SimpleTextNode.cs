namespace PoundPupLegacy.EditModel;

public interface SimpleTextNode : Node
{
    SimpleTextNodeDetails SimpleTextNodeDetails { get; }
}

public sealed record SimpleTextNodeDetails
{
    public required string Text { get; set; }
}
