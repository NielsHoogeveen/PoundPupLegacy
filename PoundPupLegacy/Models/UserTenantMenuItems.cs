namespace PoundPupLegacy.Models;

public record UserTenantMenuItems
{
    public required int UserId { get; init; }
    public required int TenantId { get; init; }
    public required List<MenuItem> MenuItems { get; init;}
}
