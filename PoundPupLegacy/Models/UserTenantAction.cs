using System.Text.Json.Serialization;

namespace PoundPupLegacy.Models;

[JsonSerializable(typeof(UserTenantAction))]
internal partial class UserTenantActionJsonContext : JsonSerializerContext { }
public sealed record UserTenantAction
{
    public required int UserId { get; init; }
    public required int TenantId { get; init; }
    public required string Action { get; init; }
}
