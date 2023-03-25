namespace PoundPupLegacy.ViewModel;

public record Articles : PagedList
{
    private SelectionItem[] termNames = Array.Empty<SelectionItem>();
    public SelectionItem[] TermNames { 
        get => termNames;
        set {
            if(value != null) {
                termNames = value;
            }
        } 
    }
    private ArticleListEntry[] articleListEntries = Array.Empty<ArticleListEntry>();
    public ArticleListEntry[] ArticleListEntries { 
        get => articleListEntries;
        set {
            if(value != null) {
                articleListEntries = value;
            }
        }
    }
    public int NumberOfEntries { get; set; }
    public int PageNumber { get; set; }
    public int NumberOfPages { get; set; }
    public string QueryString { get; set; }
    public string Path => "articles";

}
