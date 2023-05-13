namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(SubdivisionListEntry))]
public partial class SubdivisionListEntryJsonContext : JsonSerializerContext { }

public record SubdivisionListEntry : ListEntry
{
    public required string Title { get; init; }

    public required string Path { get; init; }
}
