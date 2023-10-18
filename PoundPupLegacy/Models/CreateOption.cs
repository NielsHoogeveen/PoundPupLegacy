using System.Text.Json.Serialization;

namespace PoundPupLegacy.Models;
[JsonSerializable(typeof(List<CreateOption>))]
public partial class CreateOptionJsonContext : JsonSerializerContext { }

public record CreateOption
{
    public required string Name { get; init; }
    public required string Path { get; init; }
    public required string Description { get; init; }

}
