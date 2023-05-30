namespace PoundPupLegacy.EditModel;

public interface Node
{
    public NodeDetails NodeDetails { get; }
}

public interface ResolvedNode: Node
{
}
public interface ResolvedNewNode : NewNode, ResolvedNode
{
}
public interface NewNode : Node
{
    public NodeDetails.NodeDetailsForCreate NodeDetailsForCreate { get; }
}
public interface ExistingNode : ResolvedNode
{
    NodeIdentification NodeIdentification { get; }
    public NodeDetails.NodeDetailsForUpdate NodeDetailsForUpdate { get; }
}


public sealed record NodeIdentification
{
    public required int NodeId { get; init; }
    public required int UrlId { get; init; }
}

public abstract record NodeDetails 
{
    
    public static NodeDetailsForCreate EmptyInstance(int nodeTypeId, string nodeTypeName, int ownerId, int publisherId) => new NodeDetailsForCreate {
        Files = new List<File>(),
        NodeTypeId = nodeTypeId,
        NodeTypeName = nodeTypeName,
        OwnerId = ownerId,
        PublisherId = publisherId,
        Tags = new List<Tags>(),
        Tenants = new List<Tenant>(),
        Title = "",
        NewTenantNodeDetails = new TenantNodeDetails.NewTenantNodeDetails {
            TenantNodesToAdd = new List<TenantNode.NewTenantNodeForNewNode>(),
        }
    };
    public required string NodeTypeName { get; set; }
    public required int NodeTypeId { get; init; }
    public required int PublisherId { get; set; }
    public required int OwnerId { get; set; }
    public required string Title { get; set; }
    private List<Tags> tags = new();
    public List<Tags> Tags {
        get => tags;
        init {
            if (value is not null) {
                tags = value;
            }
        }
    }
    private List<Tenant> tenants = new();
    public List<Tenant> Tenants {
        get => tenants;
        init {
            if (value is not null) {
                tenants = value;
            }
        }
    }
    private List<File> files = new();
    public required List<File> Files {
        get => files;
        init {
            if (value is not null) {
                files = value;
            }
        }
    }
    public abstract TenantNodeDetails TenantNodeDetails { get; }
    public sealed record NodeDetailsForCreate: NodeDetails
    {
        public override TenantNodeDetails TenantNodeDetails => NewTenantNodeDetails;
        public required TenantNodeDetails.NewTenantNodeDetails NewTenantNodeDetails { get; init; }

    }
    public sealed record NodeDetailsForUpdate : NodeDetails
    {
        public override TenantNodeDetails TenantNodeDetails => ExistingTenantNodeDetails;
        public required TenantNodeDetails.ExistingTenantNodeDetails ExistingTenantNodeDetails { get; init; }
    }
}


