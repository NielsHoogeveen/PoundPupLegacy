using System.Text.Json.Serialization;

namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(BasicListEntry))]
public partial class BasicListEntryJsonContext : JsonSerializerContext { }

public record BasicListEntry : ListEntry
{
    public required string Path { get; init; }
    public required string Title { get; init; }
}
