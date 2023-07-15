namespace PoundPupLegacy.DomainModel;

public abstract record OrganizationType : Nameable
{
    private OrganizationType() { }
    public required OrganizationTypeDetails OrganizationTypeDetails { get; init; }
    public sealed record ToCreate : OrganizationType, NameableToCreate
    {
        public required Identification.Possible Identification { get; init; }
        public required NodeDetails.ForCreate NodeDetails { get; init; }
        public required NameableDetails.ForCreate NameableDetails { get; init; }
    }
    public sealed record ToUpdate : OrganizationType, NameableToUpdate
    {
        public required Identification.Certain Identification { get; init; }
        public required NodeDetails.ForUpdate NodeDetails { get; init; }
        public required NameableDetails.ForUpdate NameableDetails { get; init; }
    }
}

public sealed record OrganizationTypeDetails
{
    public required bool HasConcreteSubtype { get; init; }
}
