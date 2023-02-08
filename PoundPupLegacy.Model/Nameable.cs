namespace PoundPupLegacy.Model;

public interface Nameable : Searchable
{
    string Description { get; }

    int? FileIdTileImage { get; }

    public List<VocabularyName> VocabularyNames { get; }
}
