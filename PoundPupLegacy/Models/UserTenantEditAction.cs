using System.Text.Json.Serialization;

namespace PoundPupLegacy.Models;

[JsonSerializable(typeof(UserTenantEditAction))]
internal partial class UserTenantEditActionJsonContext : JsonSerializerContext { }

public sealed record UserTenantEditAction
{
    public required int UserId { get; init; }
    public required int TenantId { get; init; }
    public required int NodeTypeId { get; init; }
    public required string NodeTypeName { get; init; }
}
