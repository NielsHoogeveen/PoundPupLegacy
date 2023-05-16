namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(DateTimeInterval))]
public partial class DateTimeIntervalJsonContext : JsonSerializerContext { }

public sealed record DateTimeInterval
{
    public required DateTime? From { get; init; }
    public required DateTime? To { get; init; }

    public FuzzyDate? ToFuzzyDate()
    {
        if (From is not null && To is not null) {
            var dateTimeRange = new DateTimeRange(From, To);
            if (FuzzyDate.TryFromDateTimeRange(dateTimeRange, out var result)) {
                return result;
            }
        }
        return null;
    }
}
