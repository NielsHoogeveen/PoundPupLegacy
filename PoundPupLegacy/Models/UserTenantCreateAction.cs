using System.Text.Json.Serialization;

namespace PoundPupLegacy.Models;

[JsonSerializable(typeof(UserTenantCreateAction))]
internal partial class UserTenantCreateActionJsonContext : JsonSerializerContext { }

public sealed record UserTenantCreateAction
{
    public required int UserId { get; init; }
    public required int TenantId { get; init; }
    public required int NodeTypeId { get; init; }
    public required string NodeTypeName { get; init; }
}
