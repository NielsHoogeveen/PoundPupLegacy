using System.Text.Json.Serialization;

namespace PoundPupLegacy.Models;

[JsonSerializable(typeof(UserTenantEditOwnAction))]
internal partial class UserTenantEditOwnActionJsonContext : JsonSerializerContext { }

public sealed record UserTenantEditOwnAction
{
    public required int UserId { get; init; }
    public required int TenantId { get; init; }
    public required int NodeTypeId { get; init; }
}
