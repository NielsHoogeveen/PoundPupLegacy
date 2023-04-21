namespace PoundPupLegacy.ViewModel.Models;

public interface SimpleTextNode : Node
{
    public string Text { get; }
    public BasicLink[] SeeAlsoBoxElements { get; }

}
