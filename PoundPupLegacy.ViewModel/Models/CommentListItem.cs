namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(CommentListItem))]
public partial class CommentListItemJsonContext : JsonSerializerContext { }

public sealed record CommentListItem
{
    public required int Id { get; init; }
    public required string Title { get; init; }
    public required int NodeStatusId { get; init; }
    public required string Text { get; init; }
    public required Authoring Authoring { get; init; }
    public required int? CommentIdParent { get; init; }

}
