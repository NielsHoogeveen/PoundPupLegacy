namespace PoundPupLegacy.CreateModel;

public sealed record NewPersonOrganizationRelation : NewNodeBase, EventuallyIdentifiablePersonOrganizationRelation
{
    public required int? PersonId { get; set; }
    public required int OrganizationId { get; init; }
    public required int PersonOrganizationRelationTypeId { get; init; }
    public required DateTimeRange DateRange { get; init; }
    public required int? DocumentIdProof { get; init; }
    public required int? GeographicalEntityId { get; init; }
    public required string? Description { get; init; }
}
public sealed record ExistingPersonOrganizationRelation : ExistingNodeBase, ImmediatelyIdentifiablePersonOrganizationRelation
{
    public required int? PersonId { get; set; }
    public required int OrganizationId { get; init; }
    public required int PersonOrganizationRelationTypeId { get; init; }
    public required DateTimeRange DateRange { get; init; }
    public required int? DocumentIdProof { get; init; }
    public required int? GeographicalEntityId { get; init; }
    public required string? Description { get; init; }
}
public interface ImmediatelyIdentifiablePersonOrganizationRelation : PersonOrganizationRelation, ImmediatelyIdentifiableNode
{
}
public interface EventuallyIdentifiablePersonOrganizationRelation : PersonOrganizationRelation, EventuallyIdentifiableNode
{
}
public interface PersonOrganizationRelation : Node
{
    int? PersonId { get; }
    int OrganizationId { get; }
    int PersonOrganizationRelationTypeId { get; }
    DateTimeRange DateRange { get; }
    int? DocumentIdProof { get; }
    int? GeographicalEntityId { get; }
    string? Description { get; }
}
