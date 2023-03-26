namespace PoundPupLegacy.Models;

public record UserTenantEditAction
{
    public required int UserId { get; init; }
    public required int TenantId { get; init; }
    public required int NodeTypeId { get; init; }
}
