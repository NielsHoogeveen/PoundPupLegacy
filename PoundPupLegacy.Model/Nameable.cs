namespace PoundPupLegacy.Model;

public interface Nameable : Node
{
    string Description { get; }

    int? FileIdTileImage { get; }

    public List<VocabularyName> VocabularyNames { get; }
}
