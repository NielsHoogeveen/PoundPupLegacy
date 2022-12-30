namespace PoundPupLegacy.Model;

public interface Term : Node
{
    public string Name { get; }

    public int VocabularyId { get; }
}
