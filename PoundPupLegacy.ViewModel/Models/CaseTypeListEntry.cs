namespace PoundPupLegacy.ViewModel.Models;

public record CaseTypeListEntry: ListEntry
{
    public required string Path { get; init; }

    public required string Title { get; init; }

    public required string Text { get; init; }
}
