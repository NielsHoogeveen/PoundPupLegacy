namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(Discussion.ToUpdate), TypeInfoPropertyName = "DiscussionToUpdate")]
[JsonSerializable(typeof(NodeDetails.ForUpdate), TypeInfoPropertyName = "NodeDetailsForUpdate")]
public partial class ExistingDiscussionJsonContext : JsonSerializerContext { }
[JsonSerializable(typeof(Discussion.ToCreate))]
[JsonSerializable(typeof(NodeDetails.ForCreate), TypeInfoPropertyName = "NodeDetailsForCreate")]
public partial class NewDiscussionJsonContext : JsonSerializerContext { }

public abstract record Discussion : SimpleTextNode, ResolvedNode, Node<Discussion.ToUpdate, Discussion.ToCreate>, Resolver<Discussion.ToUpdate, Discussion.ToCreate, Unit>
{
    private Discussion() { }
    public Node<ToUpdate, ToCreate> Resolve(Unit data) => this;
    public required SimpleTextNodeDetails SimpleTextNodeDetails { get; init; }
    public abstract NodeDetails NodeDetails { get; }
    public abstract T Match<T>(Func<ToUpdate, T> existingItem, Func<ToCreate, T> newItem);
    public abstract void Match(Action<ToUpdate> existingItem, Action<ToCreate> newItem);
    public sealed record ToUpdate : Discussion, ExistingNode
    {
        public override NodeDetails NodeDetails => NodeDetailsForUpdate;
        public required NodeDetails.ForUpdate NodeDetailsForUpdate { get; init; }
        public required NodeIdentification NodeIdentification { get; init; }
        public override T Match<T>(Func<ToUpdate, T> existingItem, Func<ToCreate, T> newItem)
        {
            return existingItem(this);
        }
        public override void Match(Action<ToUpdate> existingItem, Action<ToCreate> newItem)
        {
            existingItem(this);
        }
    }

    public sealed record ToCreate : Discussion, ResolvedNewNode
    {
        public override NodeDetails NodeDetails => NodeDetailsForCreate;
        public required NodeDetails.ForCreate NodeDetailsForCreate { get; init; }
        public override T Match<T>(Func<ToUpdate, T> existingItem, Func<ToCreate, T> newItem)
        {
            return newItem(this);
        }
        public override void Match(Action<ToUpdate> existingItem, Action<ToCreate> newItem)
        {
            newItem(this);
        }
    }
}

