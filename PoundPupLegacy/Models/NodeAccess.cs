using System.Text.Json.Serialization;

namespace PoundPupLegacy.Models;
[JsonSerializable(typeof(List<NodeAccess>))]
internal partial class NodeAccessJsonContext : JsonSerializerContext { }

public record NodeAccess
{
    public required string Name { get; init; }

    public required DateTime DateTime { get; init; }
}
