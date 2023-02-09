namespace PoundPupLegacy.ViewModel;

public record Articles : PagedList
{
    public SelectionItem[] TermNames { get; set; }
    public ArticleListEntry[] ArticleListEntries { get; set; }
    public int NumberOfEntries { get; set; }
    public int PageNumber { get; set; }
    public int NumberOfPages { get; set; }
    public string QueryString { get; set; }
    public string Path => "articles";

}
