namespace PoundPupLegacy.ViewModel.Models;

public record ArticleList : IPagedList<ArticleListEntry>
{
    private ArticleListEntry[] _entries = Array.Empty<ArticleListEntry>();
    public ArticleListEntry[] Entries {
        get => _entries;
        set {
            if (value != null) {
                _entries = value;
            }
        }
    }
    public required int NumberOfEntries { get; init; }

}
