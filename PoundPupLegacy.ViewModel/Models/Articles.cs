namespace PoundPupLegacy.ViewModel.Models;

public record Articles
{
    private SelectionItem[] termNames = Array.Empty<SelectionItem>();
    public SelectionItem[] TermNames
    {
        get => termNames;
        set
        {
            if (value != null)
            {
                termNames = value;
            }
        }
    }
    private ArticleListEntry[] articleListEntries = Array.Empty<ArticleListEntry>();
    public ArticleListEntry[] ArticleListEntries
    {
        get => articleListEntries;
        set
        {
            if (value != null)
            {
                articleListEntries = value;
            }
        }
    }
    public required int NumberOfEntries { get; init; }

}
