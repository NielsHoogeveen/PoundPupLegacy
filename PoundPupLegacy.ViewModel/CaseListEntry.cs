namespace PoundPupLegacy.ViewModel;

public record CaseListEntry
{
    public string Path { get; set; }
    public string Title { get; set; }
    public string? Text { get; set; }
    public DateTime? Date { get; set; }
    public string CaseType { get; set; }
    public bool HasBeenPublished { get; set; }
}
