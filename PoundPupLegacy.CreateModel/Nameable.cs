namespace PoundPupLegacy.CreateModel;

public interface ImmediatelyIdentifiableNameable : Nameable, ImmediatelyIdentifiableSearchable
{
}
public interface EventuallyIdentifiableNameable: Nameable, EventuallyIdentifiableSearchable
{
}

public interface Nameable : Searchable
{
    string Description { get; }

    int? FileIdTileImage { get; }

    public List<VocabularyName> VocabularyNames { get; }
}

public abstract record NewNameableBase: NewNodeBase, EventuallyIdentifiableNameable
{
    public required string Description { get; init; }

    public required int? FileIdTileImage { get; init; }

    public required List<VocabularyName> VocabularyNames { get; init; }
}

public abstract record ExistingNameableBase : ExistingNodeBase, ImmediatelyIdentifiableNameable
{
    public required string Description { get; init; }

    public required int? FileIdTileImage { get; init; }

    public required List<VocabularyName> VocabularyNames { get; init; }
}