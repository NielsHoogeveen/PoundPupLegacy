namespace PoundPupLegacy.EditModel;

public interface Node
{
    string NodeTypeName { get; }

    int PublisherId { get; }

    int OwnerId { get; }

    string Title { get; set; }
    List<Tags> Tags { get; }

    List<TenantNode> TenantNodes { get; }

    List<Tenant> Tenants { get; }

    List<File> Files { get; }

}

public interface ResolvedNode: Node
{
}
public interface ResolvedNewNode : NewNode, ResolvedNode
{

}
public interface NewNode : Node
{
}
public interface ExistingNode : ResolvedNode
{
    int NodeId { get; }
    int UrlId { get; }

}

public abstract record NewNodeBase : NodeBase, NewNode
{
}
public abstract record ExistingNodeBase : NodeBase, ExistingNode
{
    public required int NodeId { get; init; }
    public required int UrlId { get; init; }
}
public abstract record NodeBase : Node
{
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
    private List<TenantNode> tenantNodes = new();

    public List<TenantNode> TenantNodes {
        get => tenantNodes;
        init {
            if (value is not null) {
                tenantNodes = value;
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
