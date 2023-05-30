using static PoundPupLegacy.CreateModel.InterOrganizationalRelation;

namespace PoundPupLegacy.CreateModel;

public abstract record PersonOrganizationRelation: Node
{
    private PersonOrganizationRelation() { }
    public required PersonOrganizationRelationDetails PersonOrganizationRelationDetails { get; init; }
    public abstract Identification Identification { get; }
    public abstract NodeDetails NodeDetails { get; }
    public sealed record ToCreateForNewPerson : PersonOrganizationRelation, NodeToCreate
    {
        public required int? PersonId { get; set; }
        public required int OrganizationId { get; init; }

        public required Identification.Possible IdentificationForCreate { get; init; }
        public required NodeDetails.NodeDetailsForCreate NodeDetailsForCreate { get; init; }
        public override Identification Identification => IdentificationForCreate;
        public override NodeDetails NodeDetails => NodeDetailsForCreate;
        public ToCreateForExistingParticipants ResolvePerson(int personId)
        {
            return new ToCreateForExistingParticipants {
                PersonId = personId,
                OrganizationId = OrganizationId,
                NodeDetailsForCreate = NodeDetailsForCreate,
                IdentificationForCreate = IdentificationForCreate,
                PersonOrganizationRelationDetails = PersonOrganizationRelationDetails,
            };
        }
    }
    public sealed record ToCreateForNewOrganization : PersonOrganizationRelation, NodeToCreate
    {
        public required int PersonId { get; set; }
        public required int? OrganizationId { get; set; }
        public required Identification.Possible IdentificationForCreate { get; init; }
        public required NodeDetails.NodeDetailsForCreate NodeDetailsForCreate { get; init; }
        public override Identification Identification => IdentificationForCreate;
        public override NodeDetails NodeDetails => NodeDetailsForCreate;
        public ToCreateForExistingParticipants ResolveOrganization(int organizationId)
        {
            return new ToCreateForExistingParticipants {
                PersonId = PersonId,
                OrganizationId = organizationId,
                NodeDetailsForCreate = NodeDetailsForCreate,
                IdentificationForCreate = IdentificationForCreate,
                PersonOrganizationRelationDetails = PersonOrganizationRelationDetails,
            };
        }
    }

    public sealed record ToCreateForExistingParticipants : PersonOrganizationRelation, NodeToCreate
    {
        public required int PersonId { get; set; }
        public required int OrganizationId { get; init; }
        public required Identification.Possible IdentificationForCreate { get; init; }
        public required NodeDetails.NodeDetailsForCreate NodeDetailsForCreate { get; init; }
        public override Identification Identification => IdentificationForCreate;
        public override NodeDetails NodeDetails => NodeDetailsForCreate;
    }

    public sealed record ToUpdate : PersonOrganizationRelation, NodeToUpdate
    {
        public required int PersonId { get; set; }
        public required int OrganizationId { get; init; }
        public override Identification Identification => IdentificationCertain;
        public override NodeDetails NodeDetails => NodeDetailsForUpdate;
        public required Identification.Certain IdentificationCertain { get; init; }
        public required NodeDetails.ForUpdate NodeDetailsForUpdate { get; init; }
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