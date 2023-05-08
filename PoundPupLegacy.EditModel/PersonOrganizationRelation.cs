namespace PoundPupLegacy.EditModel;

public record PersonOrganizationRelation : Node
{
    public int? NodeId { get; init; }

    public int? UrlId { get; set; }

    public bool HasBeenDeleted { get; set; }

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

    public required DateTime? DateFrom { get; init; }
    public required DateTime? DateTo { get; init; }

    private bool _dateRangeIsSet = false;

    private DateTimeRange? _dateRange;
    public DateTimeRange? DateRange {
        get {
            if (!_dateRangeIsSet) {
                if (DateFrom is not null && DateTo is not null) {
                    _dateRange = new DateTimeRange(DateFrom, DateTo);
                }
                else {
                    _dateRange = null;
                }
                _dateRangeIsSet = true;
            }
            return _dateRange;
        }
        set {
            _dateRange = value;
        }
    }
    public required int PersonOrganizationRelationTypeId { get; set; }
    public required int PersonId { get; set; }
    public required int OrganizationId { get; set; }
    public int? DocumentIdProof { get; set; }

    public required string Description { get; set; }
    public required int? GeographicalEntityId { get; init; }

}
