namespace PoundPupLegacy.ViewModel.Models;

public interface Documentable : Node
{
    public DocumentListEntry[] Documents { get; }
}
