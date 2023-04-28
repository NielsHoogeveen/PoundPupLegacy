namespace PoundPupLegacy.ViewModel.Models;

public record Articles 
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
    public required ArticleList ArticleList { get; init; }
}
