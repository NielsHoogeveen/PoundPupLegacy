namespace PoundPupLegacy.ViewModel;

public record Articles: PagedList
{
    public List<TermName> TermNames { get; set; }
    public List<ArticleListEntry> ArticleListEntries { get; set; }
    public int NumberOfEntries { get; set; }
    public int PageNumber { get; set; }
    public int NumberOfPages { get; set; }
    public string QueryString { get; set; }
    public string Path => "articles";

}
