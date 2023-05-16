namespace PoundPupLegacy.EditModel;

public abstract record CaseBase: NameableBase, Case {
    public DateTime? DateFrom { get; set; }
    public DateTime? DateTo { get; set; }

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
}

public interface Case: Nameable
{
    DateTime? DateFrom { get; set; }
    DateTime? DateTo { get; set; }
    FuzzyDate? Date { get; set; }
}
