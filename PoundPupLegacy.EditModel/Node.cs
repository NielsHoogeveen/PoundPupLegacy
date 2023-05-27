namespace PoundPupLegacy.EditModel;

public interface Node
{
    string NodeTypeName { get; }

    int PublisherId { get; }

    int OwnerId { get; }

    string Title { get; set; }
    List<Tags> Tags { get; }

    IEnumerable<TenantNode> TenantNodes { get; }

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
    private List<TenantNode.NewTenantNodeForNewNode> tenantNodesToAdd = new();

    public List<TenantNode.NewTenantNodeForNewNode> TenantNodesToAdd {
        get => tenantNodesToAdd;
        init {
            if (value is not null) {
                tenantNodesToAdd = value;
            }
        }
    }

    public override IEnumerable<TenantNode> TenantNodes => TenantNodesToAdd;

}
public abstract record ExistingNodeBase : NodeBase, ExistingNode
{
    public required int NodeId { get; init; }
    public required int UrlId { get; init; }

    private List<TenantNode.NewTenantNodeForExistingNode> tenantNodesToAdd = new();

    public List<TenantNode.NewTenantNodeForExistingNode> TenantNodesToAdd {
        get => tenantNodesToAdd;
        init {
            if (value is not null) {
                tenantNodesToAdd = value;
            }
        }
    }
    private List<TenantNode.ExistingTenantNode> tenantNodesToUpdate = new();

    public List<TenantNode.ExistingTenantNode> TenantNodesToUpdate {
        get => tenantNodesToUpdate;
        init {
            if (value is not null) {
                tenantNodesToUpdate = value;
            }
        }
    }

    public override IEnumerable<TenantNode> TenantNodes => GetTenantNodes();

    private IEnumerable<TenantNode> GetTenantNodes()
    {
        foreach(var elem in tenantNodesToUpdate) {
            yield return elem;
        }
        foreach (var elem in tenantNodesToAdd) {
            yield return elem;
        }
    }
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
    public abstract IEnumerable<TenantNode> TenantNodes { get; }
}
