namespace PoundPupLegacy.EditModel;

public abstract record Tenant
{

    private Tenant()
    {

    }
    private Subgroup[] subgroups = Array.Empty<Subgroup>();
    public required Subgroup[] Subgroups {
        get => subgroups;
        init {
            if (value is not null) {
                subgroups = value;
            }
        }
    }

    public required int Id { get; init; }
    public required string DomainName { get; init; }

    public abstract TenantNode? TenantNode { get; set; }

    public bool HasTenantNode => TenantNode != null;

    public required PublicationStatusListItem[] PublicationStatuses { get; init; }

    public record ToCreate: Tenant
    {
        public required TenantNode.ToCreateForNewNode? TenantNodeToCreate { get; init; }

        private TenantNode? tenantNode = null;
        public override TenantNode? TenantNode {
            get { 
                if(tenantNode is null) {
                    tenantNode = TenantNodeToCreate;
                }
                return tenantNode;
            }
            set {
                switch (value) {
                    case null:
                        tenantNode = null;
                        break;
                    case TenantNode.ToCreateForNewNode tn:
                        tenantNode = tn;
                        break;
                    default:
                        throw new Exception("Incompatable tenant node type");
                }
            }
        }
    }
    public record ToUpdate : Tenant
    {
        public required TenantNode.ToUpdate? TenantNodeToUpdate { get; init; }
        private TenantNode? tenantNode = null;
        public override TenantNode? TenantNode {
            get {
                if (tenantNode is null) {
                    tenantNode = TenantNodeToUpdate;
                }
                return tenantNode;
            }
            set {
                switch (value) {
                    case null:
                        tenantNode = null;
                        break;
                    case TenantNode.ToCreateForExistingNode tn:
                        tenantNode = tn;
                        break;
                    case TenantNode.ToUpdate tn:
                        tenantNode = tn;
                        break;
                    default:
                        throw new Exception("Incompatable tenant node type");
                }
            }
        }
    }
}
