namespace PoundPupLegacy.CreateModel;

public interface NameableToUpdate : Nameable, SearchableToUpdate
{
    NameableDetails.ForUpdate NameableDetails { get; }
}
public interface NameableToCreate: Nameable, SearchableToCreate
{
    NameableDetails.ForCreate NameableDetails { get; }
}
public interface Nameable : Searchable
{
}
public abstract record NameableDetails
{
    private NameableDetails() { }
    public required string Description { get; init; }
    public required int? FileIdTileImage { get; init; }
    public sealed record ForCreate: NameableDetails
    {
        public required List<NewTermForNewNameable> Terms { get; init; }
    }
    public sealed record ForUpdate : NameableDetails
    {
        public required List<NewTermForExistingNameable> TermsToAdd { get; init; }
    }
}

