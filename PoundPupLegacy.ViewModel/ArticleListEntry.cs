namespace PoundPupLegacy.ViewModel;

public record ArticleListEntry
{
    public int Id { get; set; }
    public string Title { get; set; }

    public string Text { get; set; }

    public Authoring Authoring { get; set; }

    public Link[] Tags { get; set; }
}
