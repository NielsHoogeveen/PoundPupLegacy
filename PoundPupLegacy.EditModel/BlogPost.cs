namespace PoundPupLegacy.EditModel;

internal class BlogPost
{
    public required int Id { get; init; }

    public required string Title { get; set; }

    public required string Text { get; set; }
}
