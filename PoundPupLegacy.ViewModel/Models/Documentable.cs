namespace PoundPupLegacy.ViewModel.Models;
public abstract record DocumentableBase : NodeBase, Documentable
{
    private DocumentListEntry[] documents = Array.Empty<DocumentListEntry>();
    public DocumentListEntry[] Documents {
        get => documents;
        init {
            if(value is not null) {
                documents = value;
            }
        }
    }
}

public interface Documentable : Node
{
    public DocumentListEntry[] Documents { get; }
}
