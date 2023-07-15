namespace PoundPupLegacy.DomainModel;

public abstract record Profession : Nameable
{
    private Profession() { }
    public required ProfessionDetails ProfessionDetails { get; init; }
    public sealed record ToCreate : Profession, NameableToCreate
    {
        public required Identification.Possible Identification { get; init; }
        public required NodeDetails.ForCreate NodeDetails { get; init; }
        public required NameableDetails.ForCreate NameableDetails { get; init; }
    }
    public sealed record ToUpdate : Profession, NameableToUpdate
    {
        public required Identification.Certain Identification { get; init; }
        public required NodeDetails.ForUpdate NodeDetails { get; init; }
        public required NameableDetails.ForUpdate NameableDetails { get; init; }
    }
}
public sealed record ProfessionDetails
{
    public required bool HasConcreteSubtype { get; init; }
}
