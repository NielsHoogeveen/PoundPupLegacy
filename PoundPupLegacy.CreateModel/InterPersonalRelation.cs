namespace PoundPupLegacy.CreateModel;

public sealed record InterPersonalRelation : Node
{
    public required int? Id { get; set; }
    public required int PublisherId { get; init; }
    public required DateTime CreatedDateTime { get; init; }
    public required DateTime ChangedDateTime { get; init; }
    public required string Title { get; init; }
    public required int OwnerId { get; init; }
    public required int AuthoringStatusId { get; init; }
    public required int NodeTypeId { get; init; }
    public required List<TenantNode> TenantNodes { get; init; }
    public required int InterPersonalRelationTypeId { get; init; }
    public required int PersonIdFrom { get; init; }
    public required int PersonIdTo { get; init; }
    public required DateTimeRange? DateRange { get; init; }
    public required int? DocumentIdProof { get; init; }

    public required string? Description { get; init; }

}
