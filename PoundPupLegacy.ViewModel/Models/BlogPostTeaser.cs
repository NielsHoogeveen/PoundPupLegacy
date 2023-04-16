namespace PoundPupLegacy.ViewModel.Models;

public record BlogPostTeaser
{
    public required int Id { get; init; }
    public required string Title { get; init; }

    public required Authoring Authoring { get; init; }

    public required string Text { get; init; }
}
