namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(Discussion.ToUpdate), TypeInfoPropertyName = "DiscussionToUpdate")]
public partial class ExistingDiscussionJsonContext : JsonSerializerContext { }
[JsonSerializable(typeof(Discussion.ToCreate))]
public partial class NewDiscussionJsonContext : JsonSerializerContext { }

public abstract record Discussion : SimpleTextNode, ResolvedNode
{
    public required SimpleTextNodeDetails SimpleTextNodeDetails { get; init; }
    public abstract NodeDetails NodeDetails { get; }
    public sealed record ToUpdate : Discussion, ExistingNode
    {
        public override NodeDetails NodeDetails => NodeDetailsForUpdate;
        public required NodeDetails.NodeDetailsForUpdate NodeDetailsForUpdate { get; init; }
        public required NodeIdentification NodeIdentification { get; init; }
    }

    public sealed record ToCreate : Discussion, ResolvedNewNode
    {
        public override NodeDetails NodeDetails => NodeDetailsForCreate;
        public required NodeDetails.NodeDetailsForCreate NodeDetailsForCreate { get; init; }
    }

}

