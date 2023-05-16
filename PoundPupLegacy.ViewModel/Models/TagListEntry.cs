namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(TagListEntry))]
public partial class TagListEntryJsonContext : JsonSerializerContext { }

public sealed record TagListEntry : ListEntry
{
    public required string Path { get; init; }

    public required string Title { get; init; }

    public required string NodeTypeName { get; init; }
}
