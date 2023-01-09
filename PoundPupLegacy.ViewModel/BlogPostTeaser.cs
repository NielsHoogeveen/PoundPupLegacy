namespace PoundPupLegacy.ViewModel;

public record struct BlogPostTeaser
{
    public int Id { get; set; }
    public string Title { get; set; }

    public Authoring Authoring { get; set; }

    public string Text { get; set; }
}
