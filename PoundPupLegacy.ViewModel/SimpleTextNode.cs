namespace PoundPupLegacy.ViewModel;

public interface SimpleTextNode: Node
{
    public string Text { get; set; }
    public SeeAlsoBoxElement[] SeeAlsoBoxElements { get; set; }

}
