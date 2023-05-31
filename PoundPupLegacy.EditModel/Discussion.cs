namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(Discussion.ToUpdate), TypeInfoPropertyName = "DiscussionToUpdate")]
[JsonSerializable(typeof(NodeDetails.ForUpdate), TypeInfoPropertyName = "NodeDetailsForUpdate")]
public partial class ExistingDiscussionJsonContext : JsonSerializerContext { }
[JsonSerializable(typeof(Discussion.ToCreate))]
[JsonSerializable(typeof(NodeDetails.ForCreate), TypeInfoPropertyName = "NodeDetailsForCreate")]
public partial class NewDiscussionJsonContext : JsonSerializerContext { }

public abstract record Discussion : SimpleTextNode, ResolvedNode
{
    public required SimpleTextNodeDetails SimpleTextNodeDetails { get; init; }
    public abstract NodeDetails NodeDetails { get; }
    public sealed record ToUpdate : Discussion, ExistingNode
    {
        public override NodeDetails NodeDetails => NodeDetailsForUpdate;
        public required NodeDetails.ForUpdate NodeDetailsForUpdate { get; init; }
        public required NodeIdentification NodeIdentification { get; init; }
    }

    public sealed record ToCreate : Discussion, ResolvedNewNode
    {
        public override NodeDetails NodeDetails => NodeDetailsForCreate;
        public required NodeDetails.ForCreate NodeDetailsForCreate { get; init; }
    }

}

