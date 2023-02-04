namespace PoundPupLegacy.ViewModel;

public interface SimpleTextNode: Node
{
    public string Text { get; set; }
    public Link[] SeeAlsoBoxElements { get; set; }

}
