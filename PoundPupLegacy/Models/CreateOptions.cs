using System.Text.Json.Serialization;

namespace PoundPupLegacy.Models;
[JsonSerializable(typeof(List<CreateOptions>))]
public partial class CreateOptionsJsonContext : JsonSerializerContext { }

public record CreateOptions
{
    public required string Name { get; init; }
    public required string Path { get; init; }
    public required string Description { get; init; }

}
