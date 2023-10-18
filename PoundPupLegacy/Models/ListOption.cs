using System.Text.Json.Serialization;

namespace PoundPupLegacy.Models;
[JsonSerializable(typeof(List<ListOption>))]
public partial class ListOptionJsonContext : JsonSerializerContext { }

public record ListOption
{
    public required string Name { get; init; }
    public required string Path { get; init; }
    public required string Description { get; init; }

}
