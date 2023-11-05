using System.Net;

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
}

public abstract record NodeDetails 
{
    
    public static ForCreate EmptyInstance(int nodeTypeId, string nodeTypeName, string nodeTypeViewerPath, int ownerId, int publisherId, List<Tenant> tenants) => new ForCreate {
        Files = new List<File>(),
        NodeTypeId = nodeTypeId,
        NodeTypeName = nodeTypeName,
        NodeTypeViewerPath = nodeTypeViewerPath,
        OwnerId = ownerId,
        PublisherId = publisherId,
        TagsToCreate = new List<Tags.ToCreate>(),
        TenantsToCreate = tenants.Select(x => new Tenant.ToCreate { 
            DomainName = x.DomainName, 
            Id = x.Id, 
            Subgroups = x.Subgroups, 
            TenantNodeToCreate = null,
            PublicationStatuses = x.PublicationStatuses,
        }).ToList(),
        Title = "",
    };
    public required string NodeTypeName { get; set; }
    public required string NodeTypeViewerPath { get; set; }
    public required int NodeTypeId { get; init; }
    public required int PublisherId { get; set; }
    public required int OwnerId { get; set; }
    public required string Title { get; set; }

    public abstract IEnumerable<Tags> Tags { get; }
    public abstract List<Tenant> Tenants { get; set; }

    private List<File> files = new();
    public required List<File> Files {
        get => files;
        init {
            if (value is not null) {
                files = value;
            }
        }
    }
    public sealed record ForCreate: NodeDetails
    {
        private List<Tags.ToCreate> tags = new();

        public override IEnumerable<Tags> Tags => TagsToCreate;

        public required List<Tenant.ToCreate> TenantsToCreate { get; init; }

        private List<Tenant>? tenants = null;
        public override List<Tenant> Tenants {
            get { 
                if(tenants is null) {
                    tenants = TenantsToCreate.Select(x => (Tenant)x).ToList();
                }
                return tenants;
            }
            set {
                if(value is not null) 
                { 
                    tenants = value;
                }
            }
        }

        public List<Tags.ToCreate> TagsToCreate {
            get => tags;
            init {
                if (value is not null) {
                    tags = value;
                }
            }
        }
    }
    public sealed record ForUpdate : NodeDetails
    {
        public required int Id { get; init; }

        public required string PublisherName { get; set; }

        public required List<Tenant.ToUpdate> TenantsToUpdate { get; init; }

        private List<Tenant>? tenants = null;

        public override List<Tenant> Tenants {
            get {
                if (tenants is null) {
                    foreach(var tenant in TenantsToUpdate.Where(x => x.TenantNode is not null && x.TenantNode.SubgroupId is not null)) {
                        var subgroup = tenant.Subgroups.FirstOrDefault(x => x.Id == tenant.TenantNode!.SubgroupId!.Value);
                        if (subgroup is not null) {
                            subgroup.IsSelected = true;
                        }
                    }
                    tenants = TenantsToUpdate.Select(x => (Tenant)x).ToList();
                }
                return tenants;
            }
            set {
                if (value is not null) {
                    tenants = value;
                }
            }
        }

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
    }
}


