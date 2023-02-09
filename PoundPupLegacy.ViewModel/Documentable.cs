namespace PoundPupLegacy.ViewModel;

public interface Documentable : Node
{
    public DocumentListItem[] Documents { get; set; }
}
