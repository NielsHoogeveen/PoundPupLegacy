using System.Text.Json.Serialization;

namespace PoundPupLegacy.Models;

[JsonSerializable(typeof(UserCreateAction))]
internal partial class UserTenantCreateActionJsonContext : JsonSerializerContext { }

public sealed record UserCreateAction: IUserAction
{
    public required int NodeTypeId { get; init; }
    public required string NodeTypeName { get; init; }
    public required int UserGroupId { get; init; }

    public string Path => $"/create/{NodeTypeName}";
}
