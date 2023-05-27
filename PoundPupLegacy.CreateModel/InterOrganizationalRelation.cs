namespace PoundPupLegacy.CreateModel;

public sealed record NewInterOrganizationalRelationForExistingParticipants : NewNodeBase, EventuallyIdentifiableInterOrganizationalRelationForExistingParticipants
{
    public required int OrganizationIdFrom { get; init; }
    public required int OrganizationIdTo { get; init; }
    public required int InterOrganizationalRelationTypeId { get; init; }
    public required DateTimeRange DateRange { get; init; }
    public required int? DocumentIdProof { get; init; }
    public required int? GeographicalEntityId { get; init; }
    public required string? Description { get; init; }
    public required decimal? MoneyInvolved { get; init; }
    public required int? NumberOfChildrenInvolved { get; init; }
}
public sealed record NewInterOrganizationalRelationForNewOrganizationFrom : NewNodeBase, EventuallyIdentifiableInterOrganizationalRelationForNewOrganizationFrom
{
    public required int OrganizationIdTo { get; init; }
    public required int InterOrganizationalRelationTypeId { get; init; }
    public required DateTimeRange DateRange { get; init; }
    public required int? DocumentIdProof { get; init; }
    public required int? GeographicalEntityId { get; init; }
    public required string? Description { get; init; }
    public required decimal? MoneyInvolved { get; init; }
    public required int? NumberOfChildrenInvolved { get; init; }
    public EventuallyIdentifiableInterOrganizationalRelationForExistingParticipants ResolveOrganizationFrom(int organizationIdFrom)
    {
        return new NewInterOrganizationalRelationForExistingParticipants {
            OrganizationIdFrom = organizationIdFrom,
            OrganizationIdTo = OrganizationIdTo,
            AuthoringStatusId = AuthoringStatusId,
            ChangedDateTime = ChangedDateTime,
            CreatedDateTime = CreatedDateTime,
            Description = Description,
            DateRange = DateRange,
            DocumentIdProof = DocumentIdProof,
            GeographicalEntityId = GeographicalEntityId,
            Id = Id,
            InterOrganizationalRelationTypeId = InterOrganizationalRelationTypeId,
            MoneyInvolved = MoneyInvolved,
            TermIds = TermIds,
            NodeTypeId = NodeTypeId,
            NumberOfChildrenInvolved = NumberOfChildrenInvolved,
            OwnerId = OwnerId,
            PublisherId = PublisherId,
            TenantNodes = TenantNodes,
            Title = Title,
        };
    }

}

public sealed record NewInterOrganizationalRelationForNewOrganizationTo : NewNodeBase, EventuallyIdentifiableInterOrganizationalRelationForNewOrganizationTo
{
    public required int OrganizationIdFrom { get; init; }
    public required int InterOrganizationalRelationTypeId { get; init; }
    public required DateTimeRange DateRange { get; init; }
    public required int? DocumentIdProof { get; init; }
    public required int? GeographicalEntityId { get; init; }
    public required string? Description { get; init; }
    public required decimal? MoneyInvolved { get; init; }
    public required int? NumberOfChildrenInvolved { get; init; }
    public EventuallyIdentifiableInterOrganizationalRelationForExistingParticipants ResolveOrganizationTo(int organizationIdTo) 
    {
        return new NewInterOrganizationalRelationForExistingParticipants {
            OrganizationIdFrom = OrganizationIdFrom,
            OrganizationIdTo = organizationIdTo,
            AuthoringStatusId = AuthoringStatusId,
            ChangedDateTime = ChangedDateTime,
            CreatedDateTime = CreatedDateTime,
            Description = Description,
            DateRange = DateRange,
            DocumentIdProof = DocumentIdProof,
            GeographicalEntityId = GeographicalEntityId,
            Id = Id,
            InterOrganizationalRelationTypeId = InterOrganizationalRelationTypeId,
            MoneyInvolved = MoneyInvolved,
            TermIds = TermIds,
            NodeTypeId = NodeTypeId,
            NumberOfChildrenInvolved = NumberOfChildrenInvolved,
            OwnerId = OwnerId,
            PublisherId = PublisherId,
            TenantNodes = TenantNodes,
            Title = Title,
        };
    }
}
public sealed record ExistingInterOrganizationalRelation : ExistingNodeBase, ImmediatelyIdentifiableInterOrganizationalRelation
{
    public required int OrganizationIdFrom { get; init; }
    public required int OrganizationIdTo { get; init; }
    public required int InterOrganizationalRelationTypeId { get; init; }
    public required DateTimeRange DateRange { get; init; }
    public required int? DocumentIdProof { get; init; }
    public required int? GeographicalEntityId { get; init; }
    public required string? Description { get; init; }
    public required decimal? MoneyInvolved { get; init; }
    public required int? NumberOfChildrenInvolved { get; init; }
}
public interface ImmediatelyIdentifiableInterOrganizationalRelation : InterOrganizationalRelation, ImmediatelyIdentifiableNode
{
    int OrganizationIdFrom { get; }
    int OrganizationIdTo { get; }
}
public interface EventuallyIdentifiableInterOrganizationalRelationForNewOrganizationFrom : EventuallyIdentifiableInterOrganizationalRelation
{
    int OrganizationIdTo { get; }
    public EventuallyIdentifiableInterOrganizationalRelationForExistingParticipants ResolveOrganizationFrom(int organizationIdFrom);

}
public interface EventuallyIdentifiableInterOrganizationalRelationForNewOrganizationTo : EventuallyIdentifiableInterOrganizationalRelation
{
    int OrganizationIdFrom { get; }

    public EventuallyIdentifiableInterOrganizationalRelationForExistingParticipants ResolveOrganizationTo(int organizationIdTo);
}
public interface EventuallyIdentifiableInterOrganizationalRelationForExistingParticipants: EventuallyIdentifiableInterOrganizationalRelation
{
    int OrganizationIdFrom { get; }
    int OrganizationIdTo { get; }

}
public interface EventuallyIdentifiableInterOrganizationalRelation : InterOrganizationalRelation, EventuallyIdentifiableNode
{
}

public interface InterOrganizationalRelation : Node
{
    int InterOrganizationalRelationTypeId { get; }
    DateTimeRange DateRange { get; }
    int? DocumentIdProof { get; }
    int? GeographicalEntityId { get; }
    string? Description { get; }
    decimal? MoneyInvolved { get; }
    int? NumberOfChildrenInvolved { get; }
}
