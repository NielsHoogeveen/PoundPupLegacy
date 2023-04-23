namespace PoundPupLegacy.ViewModel.Models;

public record BlogListEntry : ListEntry
{
    public required int Id { get; init; }
    public required string Path { get; init; }
    public required string Title { get; init; }
    public required string? FilePathAvatar { get; init; }
    public required int NumberOfEntries { get; init; }
    public required BasicLink LatestEntry { get; init; }
}
