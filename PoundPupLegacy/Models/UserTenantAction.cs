namespace PoundPupLegacy.Models;

public record UserTenantAction
{
    public required int UserId { get; init; }
    public required int TenantId { get; init; }
    public required string Action { get; init; }
}
