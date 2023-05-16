using System.Text.Json.Serialization;

namespace PoundPupLegacy.Models;

[JsonSerializable(typeof(TenantNode))]
internal partial class TenantNodeJsonContext : JsonSerializerContext { }

public sealed record TenantNode
{
    public required int TenantId { get; init; }
    public required int UrlId { get; init; }
    public required string UrlPath { get; init; }
}
