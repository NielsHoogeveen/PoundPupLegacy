namespace PoundPupLegacy.ViewModel;

public interface SimpleTextNode : Node
{
    public string Text { get; }
    public Link[] SeeAlsoBoxElements { get; }

}
