namespace PoundPupLegacy.Model;

public record AbuseCase : Case
{
    public required int Id { get; set; }
    public required int AccessRoleId { get; init; }
    public required DateTime CreatedDateTime { get; init; }
    public required DateTime ChangedDateTime { get; init; }
    public required string Title { get; init; }
    public required int NodeStatusId { get; init; }
    public required int NodeTypeId { get; init; }
    public required string Description { get; init; }
    public required DateTimeRange? Date { get; init; }
    public required int ChildPlacementTypeId { get; init; }
    public required int? FamilySizeId { get; init; }
    public required bool? HomeschoolingInvolved { get; init; }
    public required bool? FundamentalFaithInvolved { get; init; }
    public required bool? DisabilitiesInvolved { get; init; }
    public required bool IsTopic { get; init; }
}
