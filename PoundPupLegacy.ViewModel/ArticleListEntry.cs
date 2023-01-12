namespace PoundPupLegacy.ViewModel;

public record struct ArticleListEntry
{
    public int Id { get; set; }
    public string Title { get; set; }

    public string Text { get; set; }

    public Authoring Authoring { get; set; }

    public Tag[] Tags { get; set; }
}
