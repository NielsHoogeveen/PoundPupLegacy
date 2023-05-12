namespace PoundPupLegacy.CreateModel;

public record SenatorSenateBillAction : Node
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
    public required int SenatorId { get; init; }
    public required int SenateBillId { get; init; }
    public required int BillActionTypeId { get; init; }
    public required DateTime Date { get; init; }
}
