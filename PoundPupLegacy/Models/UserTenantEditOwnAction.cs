namespace PoundPupLegacy.Models;

public record UserTenantEditOwnAction
{
    public required int UserId { get; init; }
    public required int TenantId { get; init; }
    public required int NodeTypeId { get; init; }
}
