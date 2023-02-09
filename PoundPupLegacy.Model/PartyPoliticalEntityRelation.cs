namespace PoundPupLegacy.Model;

public record PartyPoliticalEntityRelation : Node
{
    public required int? Id { get; set; }
    public required int PublisherId { get; init; }
    public required DateTime CreatedDateTime { get; init; }
    public required DateTime ChangedDateTime { get; init; }
    public required string Title { get; init; }
    public required int? OwnerId { get; init; }
    public required int NodeTypeId { get; init; }
    public required List<TenantNode> TenantNodes { get; init; }
    public required int PartyId { get; init; }
    public required int PoliticalEntityId { get; init; }
    public required int PartyPoliticalEntityRelationTypeId { get; init; }
    public required DateTimeRange DateRange { get; init; }
    public required int? DocumentIdProof { get; init; }
}
