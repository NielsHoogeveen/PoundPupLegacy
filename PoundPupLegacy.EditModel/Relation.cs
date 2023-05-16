namespace PoundPupLegacy.EditModel;

public interface Relation: Node
{
    bool HasBeenDeleted { get; set; }

    DateTime? DateFrom { get; set; }
    DateTime? DateTo { get; set; }

    DateTimeRange? DateRange { get; set; }
    DocumentListItem? ProofDocument { get; set; }
    string Description { get; set; }

}

public abstract record RelationBase: NodeBase, Relation
{
    public bool HasBeenDeleted { get; set; }

    public required DateTime? DateFrom { get; set; }
    public required DateTime? DateTo { get; set; }

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
    public DocumentListItem? ProofDocument { get; set; }
    public required string Description { get; set; }

}
