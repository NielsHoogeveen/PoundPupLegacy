namespace PoundPupLegacy.ViewModel;

public record OrganizationListEntry
{
    public string Path { get; set; }
    public string Name { get; set; }
    public int NumberOfEntries { get; set; }
    public string LatestEntryTitle { get; set; }
    public int LatestEntryId { get; set; }
}
