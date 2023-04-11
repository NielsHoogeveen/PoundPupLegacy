namespace PoundPupLegacy.ViewModel;

public record Cases : PagedList
{
    public required CaseListEntry[] CaseListEntries { get; init; }
    public int NumberOfEntries { get; set; }
    public int PageNumber { get; set; }
    public int NumberOfPages { get; set; }
    public string QueryString { get; set; } = "";
    public string Path => "cases";

}
