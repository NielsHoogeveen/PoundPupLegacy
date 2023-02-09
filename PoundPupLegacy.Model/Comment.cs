namespace PoundPupLegacy.Model;

public sealed record Comment : Identifiable
{
    public required int? Id { get; set; }

    public required int NodeId { get; init; }

    public required int? CommentIdParent { get; init; }

    public required int PublisherId { get; init; }

    public required int NodeStatusId { get; init; }

    public required DateTime CreatedDateTime { get; init; }

    public required string IPAddress { get; init; }

    public required string Title { get; init; }

    public required string Text { get; init; }
}
