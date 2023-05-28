namespace PoundPupLegacy.EditModel;

public interface Node
{

    public NodeDetails NodeDetails { get; }

    public TenantNodeDetails TenantNodeDetails { get; }

}

public interface ResolvedNode: Node
{
}
public interface ResolvedNewNode : NewNode, ResolvedNode
{

}
public interface NewNode : Node
{
    public TenantNodeDetails.NewTenantNodeDetails NewTenantNodeDetails { get;  }
}
public interface ExistingNode : ResolvedNode
{
    NodeIdentification NodeIdentification { get; }
    public TenantNodeDetails.ExistingTenantNodeDetails ExistingTenantNodeDetails { get; }
}


public sealed record NodeIdentification
{
    public required int NodeId { get; init; }
    public required int UrlId { get; init; }
}

public sealed record NodeDetails 
{
    public static NodeDetails EmptyInstance(string nodeTypeName, int ownerId, int publisherId) => new NodeDetails {
        Files = new List<File>(),
        NodeTypeName = nodeTypeName,
        OwnerId = ownerId,
        PublisherId = publisherId,
        Tags = new List<Tags>(),
        Tenants = new List<Tenant>(),
        Title = ""
    };

    public required string NodeTypeName { get; set; }

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
}


