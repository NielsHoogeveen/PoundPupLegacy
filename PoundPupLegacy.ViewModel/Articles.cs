namespace PoundPupLegacy.ViewModel;

public record struct Articles
{
    public List<TermName> TermNames { get; set; }
    public List<ArticleListEntry> ArticleListEntries { get; set; }
}
