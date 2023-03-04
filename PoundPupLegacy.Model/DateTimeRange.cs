namespace PoundPupLegacy.Model;

public sealed record DateTimeRange
{
    public DateTimeRange(DateTime? start, DateTime? end)
    {
        if (start > end) {
            throw new ArgumentException($"the start value {start} is larger than the end value {end}");
        }
        Start = start;
        End = end;
    }

    public DateTime? Start { get; }
    public DateTime? End { get; }
}
