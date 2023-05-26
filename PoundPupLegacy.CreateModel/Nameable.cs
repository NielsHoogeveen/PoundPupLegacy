namespace PoundPupLegacy.CreateModel;

public interface ImmediatelyIdentifiableNameable : Nameable, ImmediatelyIdentifiableSearchable
{
    public List<NewTermForNewNameble> TermsToAdd { get; }
}
public interface EventuallyIdentifiableNameable: Nameable, EventuallyIdentifiableSearchable
{
    public List<NewTermForNewNameble> Terms { get; }
}

public interface Nameable : Searchable
{
    string Description { get; }

    int? FileIdTileImage { get; }


}

public abstract record NewNameableBase: NewNodeBase, EventuallyIdentifiableNameable
{
    public required string Description { get; init; }

    public required int? FileIdTileImage { get; init; }

    public required List<NewTermForNewNameble> Terms { get; init; }
}

public abstract record ExistingNameableBase : ExistingNodeBase, ImmediatelyIdentifiableNameable
{
    public required string Description { get; init; }

    public required int? FileIdTileImage { get; init; }

    public required List<NewTermForNewNameble> TermsToAdd { get; init; }
}