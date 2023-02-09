namespace PoundPupLegacy.ViewModel;

public record Cases : PagedList
{
    public CaseListEntry[] CaseListEntries { get; set; }
    public int NumberOfEntries { get; set; }
    public int PageNumber { get; set; }
    public int NumberOfPages { get; set; }
    public string QueryString { get; set; }
    public string Path => "cases";

}
