namespace PoundPupLegacy.ViewModel.Models;

public abstract record SimpleTextNodeBase: NodeBase, SimpleTextNode
{
    private BasicLink[] seeAlsoBoxElements = Array.Empty<BasicLink>();
    public BasicLink[] SeeAlsoBoxElements {
        get => seeAlsoBoxElements;
        init {
            if (value is not null) {
                seeAlsoBoxElements = value;
            }
        }
    }
    public required string Text { get; init; }

}

public interface SimpleTextNode : Node
{
    public string Text { get; }
    public BasicLink[] SeeAlsoBoxElements { get; }

}
