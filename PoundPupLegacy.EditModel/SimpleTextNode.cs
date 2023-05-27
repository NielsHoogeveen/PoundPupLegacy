namespace PoundPupLegacy.EditModel;

public interface SimpleTextNode : Node
{
    string Text { get; set; }

}

public abstract record NewSimpleTextNodeBase : NewNodeBase, SimpleTextNode
{
    public required string Text { get; set; }
}
public abstract record ExistingSimpleTextNodeBase : ExistingNodeBase, SimpleTextNode
{
    public required string Text { get; set; }
}
