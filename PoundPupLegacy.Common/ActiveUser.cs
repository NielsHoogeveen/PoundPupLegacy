namespace PoundPupLegacy.Common;

public record ActiveUser
{
    public required User User { get; init; }

    public required DateTime DateTime { get; init; }
    public required int Count { get; set; }
}
