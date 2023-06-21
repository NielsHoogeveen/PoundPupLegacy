using System.Text.Json.Serialization;

namespace PoundPupLegacy.Models;
[JsonSerializable(typeof(NamedAction))]
internal partial class NamedActionJsonContext : JsonSerializerContext { }

public record NamedAction
{
    public required int TenantId { get; init; }
    public required int UserId { get; init; }
    public required string Name { get; init; }
}
