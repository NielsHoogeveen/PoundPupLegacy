namespace PoundPupLegacy.CreateModel;

public abstract record BasicOrganization : Organization
{
    private BasicOrganization() { }
  
    public sealed record ToCreate : BasicOrganization, OrganizationToCreate
    {
        public required Identification.Possible Identification { get; init; }
        public required NodeDetails.NodeDetailsForCreate NodeDetails { get; init; }
        public required NameableDetails.ForCreate NameableDetails { get; init; }
        public required LocatableDetails.LocatableDetailsForCreate LocatableDetails { get; init; }
        public required OrganizationDetails.OrganizationDetailsForCreate OrganizationDetails { get; init; }
    }
    public sealed record ToUpdate : BasicOrganization, OrganizationToUpdate
    {
        public required Identification.Certain Identification { get; init; }
        public required NodeDetails.ForUpdate NodeDetails { get; init; }
        public required NameableDetails.ForUpdate NameableDetails { get; init; }
        public required LocatableDetails.LocatableDetailsForUpdate LocatableDetails { get; init; }
        public required OrganizationDetails.OrganizationDetailsForUpdate OrganizationDetails { get; init; }   
    }
}
