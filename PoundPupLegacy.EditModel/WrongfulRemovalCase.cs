namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(WrongfulRemovalCase))]
public partial class WrongfulRemovalCaseJsonContext : JsonSerializerContext { }

public record WrongfulRemovalCase : Case
{
    public int? NodeId { get; set; }

    public int? UrlId { get; set; }

    public required string NodeTypeName { get; set; }

    public required string? Description { get; set; }

    public required string Title { get; set; }

    public required int PublisherId { get; set; }

    public required int OwnerId { get; set; }

    public DateTime? DateFrom { get; init; }
    public DateTime? DateTo { get; init; }


    private bool _dateSet;
    private FuzzyDate? _date;

    public FuzzyDate? Date {
        get {
            if (!_dateSet) {
                if (DateFrom is not null && DateTo is not null) {
                    var dateTimeRange = new DateTimeRange(DateFrom, DateTo);
                    if (FuzzyDate.TryFromDateTimeRange(dateTimeRange, out var result)) {
                        _date = result;
                    }
                }
                else {
                    _date = null;
                }
                _dateSet = true;
            }
            return _date;
        }
        set {
            _date = value;
        }
    }

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
    private List<Term> terms = new();

    public List<Term> Terms {
        get => terms;
        init {
            if (value is not null) {
                terms = value;
            }
        }
    }
}
