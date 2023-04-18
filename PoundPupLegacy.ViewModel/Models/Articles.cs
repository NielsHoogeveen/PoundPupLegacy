namespace PoundPupLegacy.ViewModel.Models;

public record Articles: IPagedList<ArticleListEntry>
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
    private ArticleListEntry[] _entries = Array.Empty<ArticleListEntry>();
    public ArticleListEntry[] Entries
    {
        get => _entries;
        set
        {
            if (value != null)
            {
                _entries = value;
            }
        }
    }
    public required int NumberOfEntries { get; init; }

}
