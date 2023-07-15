namespace PoundPupLegacy.DomainModel;

public abstract record PersonOrganizationRelationType : Nameable
{
    private PersonOrganizationRelationType() { }
    public required PersonOrganizationRelationTypeDetails PersonOrganizationRelationTypeDetails { get; init; }
    public sealed record ToCreate : PersonOrganizationRelationType, NameableToCreate
    {
        public required Identification.Possible Identification { get; init; }
        public required NodeDetails.ForCreate NodeDetails { get; init; }
        public required NameableDetails.ForCreate NameableDetails { get; init; }
    }
    public sealed record ToUpdate : PersonOrganizationRelationType, NameableToUpdate
    {
        public required Identification.Certain Identification { get; init; }
        public required NodeDetails.ForUpdate NodeDetails { get; init; }
        public required NameableDetails.ForUpdate NameableDetails { get; init; }
    }
}
public sealed record PersonOrganizationRelationTypeDetails
{
    public required bool HasConcreteSubtype { get; init; }
}
