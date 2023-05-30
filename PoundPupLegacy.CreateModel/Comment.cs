namespace PoundPupLegacy.CreateModel;

public sealed record Comment : PossiblyIdentifiable
{
    public required Identification.Possible IdentificationForCreate { get; init; }
    public Identification Identification => IdentificationForCreate;

    public required int NodeId { get; init; }

    public required int? CommentIdParent { get; init; }

    public required int PublisherId { get; init; }

    public required int NodeStatusId { get; init; }

    public required DateTime CreatedDateTime { get; init; }

    public required string IPAddress { get; init; }

    public required string Title { get; init; }

    public required string Text { get; init; }
}
