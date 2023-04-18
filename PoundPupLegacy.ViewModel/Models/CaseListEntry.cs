namespace PoundPupLegacy.ViewModel.Models;

public record CaseListEntry: ListEntry
{
    public required string Path { get; init; }
    public required string Title { get; init; }
    public required string? Text { get; init; }
    public required string CaseType { get; init; }
    public required bool HasBeenPublished { get; init; }
}
