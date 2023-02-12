namespace PoundPupLegacy.EditModel;

public record BlogPost
{
    public required int NodeId { get; init; }

    public required int UrlId { get; init; }

    public required string Title { get; set; }

    public required string Text { get; set; }
}
