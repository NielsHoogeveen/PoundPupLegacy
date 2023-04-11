namespace PoundPupLegacy.ViewModel;

public record BlogListEntry
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public required string? FilePathAvatar { get; init; }
    public required int NumberOfEntries { get; init; }
    public required string LatestEntryTitle { get; init; }
    public required int LatestEntryId { get; init; }
}
