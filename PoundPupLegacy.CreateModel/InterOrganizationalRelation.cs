namespace PoundPupLegacy.CreateModel;

public sealed record NewInterOrganizationalRelation : NewNodeBase, EventuallyIdentifiableInterOrganizationalRelation
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
}
public interface EventuallyIdentifiableInterOrganizationalRelation : InterOrganizationalRelation, EventuallyIdentifiableNode
{
}
public interface InterOrganizationalRelation : Node
{
    int OrganizationIdFrom { get; }
    int OrganizationIdTo { get; }
    int InterOrganizationalRelationTypeId { get; }
    DateTimeRange DateRange { get; }
    int? DocumentIdProof { get; }
    int? GeographicalEntityId { get; }
    string? Description { get; }
    decimal? MoneyInvolved { get; }
    int? NumberOfChildrenInvolved { get; }
}
