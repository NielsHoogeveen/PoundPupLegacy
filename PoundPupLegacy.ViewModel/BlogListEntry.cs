namespace PoundPupLegacy.ViewModel;

public record struct BlogListEntry
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? FilePathAvatar { get; set; }
    public int NumberOfEntries { get; set; }
    public string LatestEntryTitle { get; set; }
    public int LatestEntryId { get; set; }
}
