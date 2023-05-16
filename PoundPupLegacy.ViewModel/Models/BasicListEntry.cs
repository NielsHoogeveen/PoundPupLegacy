namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(BasicListEntry))]
public partial class BasicListEntryJsonContext : JsonSerializerContext { }

public sealed record BasicListEntry : ListEntry
{
    public required string Path { get; init; }
    public required string Title { get; init; }
}
