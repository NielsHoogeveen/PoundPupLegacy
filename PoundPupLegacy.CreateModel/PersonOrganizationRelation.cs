namespace PoundPupLegacy.CreateModel;

public abstract record PersonOrganizationRelation: Node
{
    private PersonOrganizationRelation() { }
    public required PersonOrganizationRelationDetails PersonOrganizationRelationDetails { get; init; }
    public sealed record ToCreateForNewPerson : PersonOrganizationRelation, NodeToCreate
    {
        public required int? PersonId { get; set; }
        public required int OrganizationId { get; init; }

        public required Identification.Possible Identification { get; init; }
        public required NodeDetails.NodeDetailsForCreate NodeDetails { get; init; }
        public ToCreateForExistingParticipants ResolvePerson(int personId)
        {
            return new ToCreateForExistingParticipants {
                PersonId = personId,
                OrganizationId = OrganizationId,
                NodeDetails = NodeDetails,
                Identification = Identification,
                PersonOrganizationRelationDetails = PersonOrganizationRelationDetails,
            };
        }
    }
    public sealed record ToCreateForNewOrganization : PersonOrganizationRelation, NodeToCreate
    {
        public required int PersonId { get; set; }
        public required int? OrganizationId { get; set; }
        public required Identification.Possible Identification { get; init; }
        public required NodeDetails.NodeDetailsForCreate NodeDetails { get; init; }
        public ToCreateForExistingParticipants ResolveOrganization(int organizationId)
        {
            return new ToCreateForExistingParticipants {
                PersonId = PersonId,
                OrganizationId = organizationId,
                NodeDetails = NodeDetails,
                Identification = Identification,
                PersonOrganizationRelationDetails = PersonOrganizationRelationDetails,
            };
        }
    }

    public sealed record ToCreateForExistingParticipants : PersonOrganizationRelation, NodeToCreate
    {
        public required int PersonId { get; set; }
        public required int OrganizationId { get; init; }
        public required Identification.Possible Identification { get; init; }
        public required NodeDetails.NodeDetailsForCreate NodeDetails { get; init; }
    }

    public sealed record ToUpdate : PersonOrganizationRelation, NodeToUpdate
    {
        public required int PersonId { get; set; }
        public required int OrganizationId { get; init; }
        public required Identification.Certain Identification { get; init; }
        public required NodeDetails.ForUpdate NodeDetails { get; init; }
    }
}
public sealed record PersonOrganizationRelationDetails
{
    public required int PersonOrganizationRelationTypeId { get; init; }
    public required DateTimeRange DateRange { get; init; }
    public required int? DocumentIdProof { get; init; }
    public required int? GeographicalEntityId { get; init; }
    public required string? Description { get; init; }
}