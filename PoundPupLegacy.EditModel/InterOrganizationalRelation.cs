namespace PoundPupLegacy.EditModel;

public record InterOrganizationalRelation : Node
{
    public int? NodeId { get; init; } 

    public int? UrlId { get; set; }

    public bool HasBeenDeleted { get; set; }

    public required string NodeTypeName { get; set; }

    public required int PublisherId { get; set; }

    public required int OwnerId { get; set; }

    public required string Title { get; set; }

    public required string Description { get; set; }

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

    public required int InterOrganizationalRelationTypeId { get; set; }

    public required string InterOrganizationalRelationTypeName { get; init; }

    public required int OrganizationIdFrom { get; set; }

    public required string OrganizationNameFrom { get; set; }

    public required int OrganizationIdTo { get; set; }

    public required string OrganizationNameTo { get; set; }

    public required DateTime? DateFrom { get; set; }
    public required DateTime? DateTo { get; set; }

    public DateTimeRange DateRange {
        get => new DateTimeRange(DateFrom, DateTo);
    }
    public int? DocumentIdProof { get; set; }
    public string? DocumentNameProof { get; set; }
    public decimal? MoneyInvolved { get; set; }
    public int? NumberOfChildrenInvolved { get; set; }
    public required int? GeographicalEntityId { get; init; }
    public string? GeographicalEntityName { get; init; }

}
