namespace PoundPupLegacy.ViewModel.Models;

public interface Documentable : Node
{
    public DocumentListItem[] Documents { get; }
}
