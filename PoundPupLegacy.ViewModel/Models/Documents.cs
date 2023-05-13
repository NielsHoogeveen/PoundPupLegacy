namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(Documents))]
public partial class DocumentsJsonContext : JsonSerializerContext { }

public record Documents: TermedList<ArticleList, ArticleListEntry>
{
    private SelectionItem[] termNames = Array.Empty<SelectionItem>();
    public SelectionItem[] TermNames {
        get => termNames;
        set {
            if (value != null) {
                termNames = value;
            }
        }
    }
    public required ArticleList Items { get; init; }
}
