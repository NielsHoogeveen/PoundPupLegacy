namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(ExistingInterOrganizationalFromRelation))]
public partial class ExistingInterOrganizationalFromRelationJsonContext : JsonSerializerContext { }

public interface _Node
{
    string NodeTypeName { get; }

    int PublisherId { get; }

    int OwnerId { get; }

    string Title { get; }
    List<Tags> Tags { get; }

    List<TenantNode> TenantNodes { get; }

    List<Tenant> Tenants { get; }

    List<File> Files { get; }

}

public interface NewNode: _Node
{

}
public interface ExistingNode : _Node
{
    int NodeId { get; }
    int UrlId { get; }
    bool HasBeenDeleted { get; set; }

}

public interface _InterOrganizationalRelation: _Node
{

    string Description { get; }

    
    InterOrganizationalRelationTypeListItem InterOrganizationalRelationType { get; }

    DateTime? DateFrom { get; }
    DateTime? DateTo { get; }

    DateTimeRange DateRange { get; }
    DocumentListItem? ProofDocument { get; }
    decimal? MoneyInvolved { get; }
    int? NumberOfChildrenInvolved { get;  }
    GeographicalEntityListItem? GeographicalEntity { get;  }

}
public interface ExistingInterOrganizationalRelation: _InterOrganizationalRelation, ExistingNode
{

}
public interface NewInterOrganizationalRelation: _InterOrganizationalRelation, NewNode
{
}

public record NodeBase: _Node
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
public record InterOrganizationalRelationBase : NodeBase, _InterOrganizationalRelation
{
    public required string Description { get; set; }

    public required InterOrganizationalRelationTypeListItem InterOrganizationalRelationType { get; set; }

    public required OrganizationListItem? OrganizationFrom { get; set; }

    public required OrganizationListItem? OrganizationTo { get; set; }

    public required DateTime? DateFrom { get; set; }
    public required DateTime? DateTo { get; set; }

    public DateTimeRange DateRange {
        get => new DateTimeRange(DateFrom, DateTo);
    }
    public DocumentListItem? ProofDocument { get; set; }
    public decimal? MoneyInvolved { get; set; }
    public int? NumberOfChildrenInvolved { get; set; }
    public required GeographicalEntityListItem? GeographicalEntity { get; set; }


}
public record ExistingInterOrganizationalFromRelation : InterOrganizationalRelationBase, ExistingInterOrganizationalRelation
{
    public int NodeId { get; init; } 

    public int UrlId { get; set; }

    public bool HasBeenDeleted { get; set; }

}
