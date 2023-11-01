using System.Text.Json.Serialization;

namespace PoundPupLegacy.Models;
[JsonSerializable(typeof(List<Subgroup>))]
public partial class SubgroupJsonContext : JsonSerializerContext { }

public record Subgroup
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public required string Path { get; init; }
    public required string Description { get; init; }

}
