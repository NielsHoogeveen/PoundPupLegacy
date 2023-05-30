namespace PoundPupLegacy.CreateModel;

public abstract record InterOrganizationalRelation: Node
{
    private InterOrganizationalRelation() { }
    public abstract Identification Identification { get; }
    public abstract NodeDetails NodeDetails { get; }
    public required InterOrganizationalRelationDetails InterOrganizationalRelationDetails { get; init; }

    public sealed record ToCreateForExistingParticipants : InterOrganizationalRelation, NodeToCreate
    {
        public required int OrganizationIdFrom { get; init; }
        public required int OrganizationIdTo { get; init; }
        public required Identification.Possible IdentificationForCreate { get; init; }
        public required NodeDetails.NodeDetailsForCreate NodeDetailsForCreate { get; init; }
        public override Identification Identification => IdentificationForCreate;
        public override NodeDetails NodeDetails => NodeDetailsForCreate;
    }
    public sealed record ToCreateForNewOrganizationFrom : InterOrganizationalRelation, NodeToCreate
    {
        public required int OrganizationIdTo { get; init; }
        public required Identification.Possible IdentificationForCreate { get; init; }
        public required NodeDetails.NodeDetailsForCreate NodeDetailsForCreate { get; init; }
        public override Identification Identification => IdentificationForCreate;
        public override NodeDetails NodeDetails => NodeDetailsForCreate;
        public ToCreateForExistingParticipants ResolveOrganizationFrom(int organizationIdFrom)
        {
            return new ToCreateForExistingParticipants {
                OrganizationIdFrom = organizationIdFrom,
                OrganizationIdTo = OrganizationIdTo,
                InterOrganizationalRelationDetails = InterOrganizationalRelationDetails,
                NodeDetailsForCreate = NodeDetailsForCreate,
                IdentificationForCreate = IdentificationForCreate,
            };
        }
    }

    public sealed record ToCreateForNewOrganizationTo : InterOrganizationalRelation, NodeToCreate
    {
        public required int OrganizationIdFrom { get; init; }
        public required Identification.Possible IdentificationForCreate { get; init; }
        public required NodeDetails.NodeDetailsForCreate NodeDetailsForCreate { get; init; }
        public override Identification Identification => IdentificationForCreate;
        public override NodeDetails NodeDetails => NodeDetailsForCreate;
        public ToCreateForExistingParticipants ResolveOrganizationTo(int organizationIdTo)
        {
            return new ToCreateForExistingParticipants {
                OrganizationIdFrom = OrganizationIdFrom,
                OrganizationIdTo = organizationIdTo,
                InterOrganizationalRelationDetails = InterOrganizationalRelationDetails,
                NodeDetailsForCreate = NodeDetailsForCreate,
                IdentificationForCreate = IdentificationForCreate,
            };
        }
    }
    public sealed record ToUpdate : InterOrganizationalRelation, NodeToUpdate
    {
        public required int OrganizationIdFrom { get; init; }
        public required int OrganizationIdTo { get; init; }
        public override Identification Identification => IdentificationCertain;
        public override NodeDetails NodeDetails => NodeDetailsForUpdate;
        public required Identification.Certain IdentificationCertain { get; init; }
        public required NodeDetails.ForUpdate NodeDetailsForUpdate { get; init; }
    }
}

public sealed record InterOrganizationalRelationDetails
{
    public required int InterOrganizationalRelationTypeId { get; init; }
    public required DateTimeRange DateRange { get; init; }
    public required int? DocumentIdProof { get; init; }
    public required int? GeographicalEntityId { get; init; }
    public required string? Description { get; init; }
    public required decimal? MoneyInvolved { get; init; }
    public required int? NumberOfChildrenInvolved { get; init; }
}
