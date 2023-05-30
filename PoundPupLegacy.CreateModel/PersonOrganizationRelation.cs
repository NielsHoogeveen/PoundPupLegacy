namespace PoundPupLegacy.CreateModel;

public abstract record PersonOrganizationRelation: Node
{
    private PersonOrganizationRelation() { }
    public required PersonOrganizationRelationDetails PersonOrganizationRelationDetails { get; init; }
    public abstract record ToCreate : PersonOrganizationRelation, NodeToCreate
    {
        private ToCreate() { }
        public required Identification.Possible Identification { get; init; }
        public required NodeDetails.ForCreate NodeDetails { get; init; }
        public sealed record ForNewPerson : ToCreate
        {
            public required int? PersonId { get; set; }
            public required int OrganizationId { get; init; }

            public ForExistingParticipants ResolvePerson(int personId)
            {
                return new ForExistingParticipants {
                    PersonId = personId,
                    OrganizationId = OrganizationId,
                    NodeDetails = NodeDetails,
                    Identification = Identification,
                    PersonOrganizationRelationDetails = PersonOrganizationRelationDetails,
                };
            }
        }
        public sealed record ForNewOrganization : ToCreate
        {
            public required int PersonId { get; set; }
            public required int? OrganizationId { get; set; }
            public ForExistingParticipants ResolveOrganization(int organizationId)
            {
                return new ForExistingParticipants {
                    PersonId = PersonId,
                    OrganizationId = organizationId,
                    NodeDetails = NodeDetails,
                    Identification = Identification,
                    PersonOrganizationRelationDetails = PersonOrganizationRelationDetails,
                };
            }
        }

        public sealed record ForExistingParticipants : ToCreate
        {
            public required int PersonId { get; set; }
            public required int OrganizationId { get; init; }
        }
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