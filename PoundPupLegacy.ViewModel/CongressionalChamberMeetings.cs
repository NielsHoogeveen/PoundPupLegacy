namespace PoundPupLegacy.ViewModel;

public record CongressionalChamberMeetings
{
    public required string Name { get; init; }

    public required string Path { get; init; }

    public required DateTime DateFrom { get; init; }

    public required DateTime DateTo { get; init; }
}
