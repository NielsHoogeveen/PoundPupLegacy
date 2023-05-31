namespace PoundPupLegacy.CreateModel;

public abstract record BasicOrganization : Organization
{
    private BasicOrganization() { }
  
    public sealed record ToCreate : BasicOrganization, OrganizationToCreate
    {
        public required Identification.Possible Identification { get; init; }
        public required NodeDetails.ForCreate NodeDetails { get; init; }
        public required NameableDetails.ForCreate NameableDetails { get; init; }
        public required LocatableDetails.ForCreate LocatableDetails { get; init; }
        public required OrganizationDetails.ForCreate OrganizationDetails { get; init; }
    }
    public sealed record ToUpdate : BasicOrganization, OrganizationToUpdate
    {
        public required Identification.Certain Identification { get; init; }
        public required NodeDetails.ForUpdate NodeDetails { get; init; }
        public required NameableDetails.ForUpdate NameableDetails { get; init; }
        public required LocatableDetails.ForUpdate LocatableDetails { get; init; }
        public required OrganizationDetails.ForUpdate OrganizationDetails { get; init; }   
    }
}
