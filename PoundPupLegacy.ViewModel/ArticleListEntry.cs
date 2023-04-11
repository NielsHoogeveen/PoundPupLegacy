namespace PoundPupLegacy.ViewModel;

public record ArticleListEntry
{
    public required int Id { get; init; }
    public required string Title { get; init; }

    public required string Text { get; init; }

    public required Authoring Authoring { get; init; }

    public required Link[] Tags { get; init; }
}
