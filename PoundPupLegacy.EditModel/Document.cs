namespace PoundPupLegacy.EditModel;

public record Document : Node
{
    public int? NodeId { get; init; }
    public int? UrlId { get; set; }
    public required string Title { get; set; }
    public required int PublisherId { get; set; }
    public required int OwnerId { get; set; }
    public string? SourceUrl { get; set; }
    public required string Text { get; set; }

    public int? DocumentTypeId { get; set; }

    public DateTime? PublicationDateFrom { get; set; }
    public DateTime? PublicationDateTo { get; set; }


    private bool _publishedSet;
    private FuzzyDate? _published;
    public FuzzyDate? Published {
        get {
            if (!_publishedSet) {
                if (PublicationDateFrom is not null && PublicationDateTo is not null) {
                    var dateTimeRange = new DateTimeRange(PublicationDateFrom, PublicationDateTo);
                    if (FuzzyDate.TryFromDateTimeRange(dateTimeRange, out var result)) {
                        _published = result;
                    }
                }
                else {
                    _published = null;
                }
                _publishedSet = true;
            }
            return _published;
        }
        set {
            _published = value;
        }
    }

    public required DocumentType[] DocumentTypes { get; init; }
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

    public List<File> Files {
        get => files;
        init {
            if (value is not null) {
                files = value;
            }
        }
    }

}
