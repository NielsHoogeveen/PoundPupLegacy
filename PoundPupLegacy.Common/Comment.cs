using System.Data;

namespace PoundPupLegacy.Common;

public abstract record Comment
{
    private Comment() { }
    public required string Title { get; init; }
    public required int NodeStatusId { get; init; }
    public required string Text { get; init; }
    public required int PublisherId { get; init; }

    public abstract T Match<T>(Func<ToCreate, T> toCreate, Func<ToUpdate, T> toUpdate);

    public sealed record ToCreate: Comment
    {
        public required int NodeId { get; init; }
        public required int? CommentIdParent { get; init; }

        public DateTime CreatedDataTime = DateTime.Now;

        public override T Match<T>(Func<ToCreate, T> toCreate, Func<ToUpdate, T> toUpdate)
        {
            return toCreate(this);
        }
    }
    public sealed record ToUpdate : Comment
    {
        public required int Id { get; init; }
        public override T Match<T>(Func<ToCreate, T> toCreate, Func<ToUpdate, T> toUpdate)
        {
            return toUpdate(this);
        }
    }
}
