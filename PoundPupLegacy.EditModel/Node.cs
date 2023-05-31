namespace PoundPupLegacy.EditModel;

public interface Node<TExisting, TNew>: Node
    where TExisting: ExistingNode
    where TNew: NewNode
{
    T Match<T>(Func<TExisting, T> existing, Func<TNew, T> resolved);
}
public interface Resolver<TExisting, TNew, TResolveData> : Node
    where TExisting : ExistingNode
    where TNew : ResolvedNewNode
{
    Node<TExisting, TNew> Resolve(TResolveData data);
}

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
    public NodeDetails.ForCreate NodeDetailsForCreate { get; }
}
public interface ExistingNode : ResolvedNode
{
    NodeIdentification NodeIdentification { get; }
    public NodeDetails.ForUpdate NodeDetailsForUpdate { get; }
}


public sealed record NodeIdentification
{
    public required int NodeId { get; init; }
    public required int UrlId { get; init; }
}

public abstract record NodeDetails 
{
    
    public static ForCreate EmptyInstance(int nodeTypeId, string nodeTypeName, int ownerId, int publisherId) => new ForCreate {
        Files = new List<File>(),
        NodeTypeId = nodeTypeId,
        NodeTypeName = nodeTypeName,
        OwnerId = ownerId,
        PublisherId = publisherId,
        TagsToCreate = new List<Tags.ToCreate>(),
        Tenants = new List<Tenant>(),
        Title = "",
        TenantNodeDetailsForCreate = new TenantNodeDetails.ForCreate {
            TenantNodesToAdd = new List<TenantNode.ToCreateForNewNode>(),
        }
    };
    public required string NodeTypeName { get; set; }
    public required int NodeTypeId { get; init; }
    public required int PublisherId { get; set; }
    public required int OwnerId { get; set; }
    public required string Title { get; set; }

    public abstract IEnumerable<Tags> Tags { get; }

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
    public sealed record ForCreate: NodeDetails
    {
        private List<Tags.ToCreate> tags = new();

        public override IEnumerable<Tags> Tags => TagsToCreate;
        public List<Tags.ToCreate> TagsToCreate {
            get => tags;
            init {
                if (value is not null) {
                    tags = value;
                }
            }
        }
        public override TenantNodeDetails TenantNodeDetails => TenantNodeDetailsForCreate;
        public required TenantNodeDetails.ForCreate TenantNodeDetailsForCreate { get; init; }

    }
    public sealed record ForUpdate : NodeDetails
    {
        private List<Tags.ToUpdate> tags = new();
        public override IEnumerable<Tags> Tags => TagsForUpdate;
        public List<Tags.ToUpdate> TagsForUpdate {
            get => tags;
            init {
                if (value is not null) {
                    tags = value;
                }
            }
        }
        public override TenantNodeDetails TenantNodeDetails => TenantNodeDetailsForUpdate;
        public required TenantNodeDetails.ForUpdate TenantNodeDetailsForUpdate { get; init; }
    }
}


