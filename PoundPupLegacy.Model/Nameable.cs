namespace PoundPupLegacy.Model;

public interface Nameable : Node
{
    string Description { get; }

    public List<VocabularyName> VocabularyNames { get; }
}
