namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(RelationDetails))]
public partial class RelationDetailsJsonContext : JsonSerializerContext { }

public interface Relation: Node
{
    RelationDetails RelationDetails { get; }
}

public sealed record RelationDetails
{
    public static RelationDetails EmptyInstance => new RelationDetails {
        DateFrom = null,
        DateTo = null,
        Description = "",
        HasBeenDeleted = false,
        ProofDocument = null,
    };
    public bool HasBeenDeleted { get; set; }

    public required DateTime? DateFrom { get; set; }
    public required DateTime? DateTo { get; set; }

    private bool _dateRangeIsSet = false;

    private DateTimeRange? _dateRange;
    public DateTimeRange? DateRange {
        get {
            if (!_dateRangeIsSet) {
                if (DateFrom is not null || DateTo is not null) {
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
    public DocumentListItem? ProofDocument { get; set; }
    public required string Description { get; set; }

}
