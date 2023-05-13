using System.Text.Json.Serialization;

namespace PoundPupLegacy.Models;

[JsonSerializable(typeof(UserTenantMenuItems))]
internal partial class UserTenantMenuItemsJsonContext : JsonSerializerContext { }

public record UserTenantMenuItems
{
    public required int UserId { get; init; }
    public required int TenantId { get; init; }
    public required List<MenuItem> MenuItems { get; init; }
}
