namespace PoundPupLegacy.ViewModel;

public record CaseListEntry
{
    public required string Path { get; init; }
    public required string Title { get; init; }
    public required string? Text { get; init; }
    public required string CaseType { get; init; }
    public required bool HasBeenPublished { get; init; }
}
