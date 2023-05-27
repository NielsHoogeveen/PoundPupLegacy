namespace PoundPupLegacy.CreateModel;

public interface ImmediatelyIdentifiableNameable : Nameable, ImmediatelyIdentifiableSearchable
{
    public List<NewTermForExistingNameable> TermsToAdd { get; }
}
public interface EventuallyIdentifiableNameable: Nameable, EventuallyIdentifiableSearchable
{
    public List<NewTermForNewNameable> Terms { get; }
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

    public required List<NewTermForNewNameable> Terms { get; init; }
}

public abstract record ExistingNameableBase : ExistingNodeBase, ImmediatelyIdentifiableNameable
{
    public required string Description { get; init; }

    public required int? FileIdTileImage { get; init; }

    public required List<NewTermForExistingNameable> TermsToAdd { get; init; }
}