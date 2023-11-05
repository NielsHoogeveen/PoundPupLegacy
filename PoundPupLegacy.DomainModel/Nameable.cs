namespace PoundPupLegacy.DomainModel;

public interface NameableToUpdate : Nameable, SearchableToUpdate
{
    NameableDetails.ForUpdate NameableDetails { get; }
}
public interface NameableToCreate : Nameable, SearchableToCreate
{
    NameableDetails.ForCreate NameableDetails { get; }
}
public interface Nameable : Searchable
{
}
public abstract record NameableDetails: IRequest
{
    private NameableDetails() { }
    public required string Description { get; init; }
    public required int? FileIdTileImage { get; init; }
    public sealed record ForCreate : NameableDetails
    {
        public required List<Term.ToCreateForNewNameable> Terms { get; init; }
    }
    public sealed record ForUpdate : NameableDetails
    {
        public required List<int> TermsToRemove { get; init; }
        public required List<Term.ToUpdate> TermsToUpdate { get; init; }
        public required List<Term.ToCreateForExistingNameable> TermsToAdd { get; init; }
    }
}

