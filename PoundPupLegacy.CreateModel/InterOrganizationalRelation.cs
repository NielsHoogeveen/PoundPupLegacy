namespace PoundPupLegacy.CreateModel;

public abstract record InterOrganizationalRelation: Node
{
    private InterOrganizationalRelation() { }
    public required InterOrganizationalRelationDetails InterOrganizationalRelationDetails { get; init; }

    public sealed record ToCreateForExistingParticipants : InterOrganizationalRelation, NodeToCreate
    {
        public required int OrganizationIdFrom { get; init; }
        public required int OrganizationIdTo { get; init; }
        public required Identification.Possible Identification { get; init; }
        public required NodeDetails.NodeDetailsForCreate NodeDetails { get; init; }
    }
    public sealed record ToCreateForNewOrganizationFrom : InterOrganizationalRelation, NodeToCreate
    {
        public required int OrganizationIdTo { get; init; }
        public required Identification.Possible Identification { get; init; }
        public required NodeDetails.NodeDetailsForCreate NodeDetails { get; init; }
        public ToCreateForExistingParticipants ResolveOrganizationFrom(int organizationIdFrom)
        {
            return new ToCreateForExistingParticipants {
                OrganizationIdFrom = organizationIdFrom,
                OrganizationIdTo = OrganizationIdTo,
                InterOrganizationalRelationDetails = InterOrganizationalRelationDetails,
                NodeDetails = NodeDetails,
                Identification = Identification,
            };
        }
    }

    public sealed record ToCreateForNewOrganizationTo : InterOrganizationalRelation, NodeToCreate
    {
        public required int OrganizationIdFrom { get; init; }
        public required Identification.Possible Identification { get; init; }
        public required NodeDetails.NodeDetailsForCreate NodeDetails { get; init; }
        public ToCreateForExistingParticipants ResolveOrganizationTo(int organizationIdTo)
        {
            return new ToCreateForExistingParticipants {
                OrganizationIdFrom = OrganizationIdFrom,
                OrganizationIdTo = organizationIdTo,
                InterOrganizationalRelationDetails = InterOrganizationalRelationDetails,
                NodeDetails = NodeDetails,
                Identification = Identification,
            };
        }
    }
    public sealed record ToUpdate : InterOrganizationalRelation, NodeToUpdate
    {
        public required int OrganizationIdFrom { get; init; }
        public required int OrganizationIdTo { get; init; }
        public required Identification.Certain Identification { get; init; }
        public required NodeDetails.ForUpdate NodeDetails { get; init; }
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
