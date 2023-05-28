namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(Discussion.ExistingDiscussion))]
public partial class ExistingDiscussionJsonContext : JsonSerializerContext { }
[JsonSerializable(typeof(Discussion.NewDiscussion))]
public partial class NewDiscussionJsonContext : JsonSerializerContext { }

public abstract record Discussion : SimpleTextNode, ResolvedNode
{
    public required SimpleTextNodeDetails SimpleTextNodeDetails { get; init; }

    public required NodeDetails NodeDetails { get; init; }

    public abstract TenantNodeDetails TenantNodeDetails { get; }

    public sealed record ExistingDiscussion : Discussion, ExistingNode
    {
        public override TenantNodeDetails TenantNodeDetails => ExistingTenantNodeDetails;
        public required TenantNodeDetails.ExistingTenantNodeDetails ExistingTenantNodeDetails { get; init; }
        public required NodeIdentification NodeIdentification { get; init; }
    }

    public sealed record NewDiscussion : Discussion, ResolvedNewNode
    {
        public override TenantNodeDetails TenantNodeDetails => NewTenantNodeDetails;
        public required TenantNodeDetails.NewTenantNodeDetails NewTenantNodeDetails { get; init; }

    }

}

