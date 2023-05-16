namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(ArticleList))]
public partial class ArticleListJsonContext : JsonSerializerContext { }

public sealed record ArticleList : IPagedList<ArticleListEntry>
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
