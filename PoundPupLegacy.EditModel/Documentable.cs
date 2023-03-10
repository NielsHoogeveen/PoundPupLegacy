namespace PoundPupLegacy.EditModel;

public interface Documentable: Node
{
    List<DocumentableDocument> Documents { get; }
}
