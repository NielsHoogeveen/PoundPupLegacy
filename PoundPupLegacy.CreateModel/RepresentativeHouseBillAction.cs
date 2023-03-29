namespace PoundPupLegacy.CreateModel;

public record RepresentativeHouseBillAction : Node
{
    public required int? Id { get; set; }
    public required int PublisherId { get; init; }
    public required DateTime CreatedDateTime { get; init; }
    public required DateTime ChangedDateTime { get; init; }
    public required string Title { get; init; }
    public required int? OwnerId { get; init; }
    public required int NodeTypeId { get; init; }
    public required List<TenantNode> TenantNodes { get; init; }
    public required int RepresentativeId { get; init; }
    public required int HouseBillId { get; init; }
    public required int BillActionTypeId { get; init; }
    public required DateTime Date { get; init; }
}
