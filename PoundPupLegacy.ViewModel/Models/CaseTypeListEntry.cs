namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(CaseTypeListEntry))]
public partial class CaseTypeListEntryJsonContext : JsonSerializerContext { }

public record CaseTypeListEntry: ListEntry
{
    public required string Path { get; init; }

    public required string Title { get; init; }

    public required string Text { get; init; }
}
