namespace PoundPupLegacy.DomainModel;

public abstract record InterOrganizationalRelation : Node
{
    private InterOrganizationalRelation() { }
    public required InterOrganizationalRelationDetails InterOrganizationalRelationDetails { get; init; }

    public abstract record ToCreate : InterOrganizationalRelation, NodeToCreate
    {
        private ToCreate() { }
        public required Identification.Possible Identification { get; init; }
        public required NodeDetails.ForCreate NodeDetails { get; init; }
        public sealed record ForExistingParticipants : ToCreate
        {
            public required int OrganizationIdFrom { get; init; }
            public required int OrganizationIdTo { get; init; }
        }
        public sealed record ForNewOrganizationFrom : ToCreate
        {
            public required int OrganizationIdTo { get; init; }
            public ForExistingParticipants ResolveOrganizationFrom(int organizationIdFrom)
            {
                return new ForExistingParticipants {
                    OrganizationIdFrom = organizationIdFrom,
                    OrganizationIdTo = OrganizationIdTo,
                    InterOrganizationalRelationDetails = InterOrganizationalRelationDetails,
                    NodeDetails = NodeDetails,
                    Identification = Identification,
                };
            }
        }

        public sealed record ForNewOrganizationTo : ToCreate
        {
            public required int OrganizationIdFrom { get; init; }
            public ForExistingParticipants ResolveOrganizationTo(int organizationIdTo)
            {
                return new ForExistingParticipants {
                    OrganizationIdFrom = OrganizationIdFrom,
                    OrganizationIdTo = organizationIdTo,
                    InterOrganizationalRelationDetails = InterOrganizationalRelationDetails,
                    NodeDetails = NodeDetails,
                    Identification = Identification,
                };
            }
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
