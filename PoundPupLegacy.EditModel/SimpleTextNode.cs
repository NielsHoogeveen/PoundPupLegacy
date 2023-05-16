namespace PoundPupLegacy.EditModel;

public interface SimpleTextNode : Node
{
    string Text { get; set; }

}

public abstract record SimpleTextNodeBase : NodeBase, SimpleTextNode
{
    public required string Text { get; set; }
}
