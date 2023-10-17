using System.Text.Json.Serialization;

namespace PoundPupLegacy.Models;
[JsonSerializable(typeof(List<ListOptions>))]
public partial class ListOptionsJsonContext : JsonSerializerContext { }

public record ListOptions
{
    public required string Name { get; init; }
    public required string Path { get; init; }
    public required string Description { get; init; }

}
