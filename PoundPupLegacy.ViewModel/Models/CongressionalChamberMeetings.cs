namespace PoundPupLegacy.ViewModel.Models;

public record CongressionalChamberMeetings: Link
{
    public required string Title { get; init; }

    public required string Path { get; init; }

    public required DateTime DateFrom { get; init; }

    public required DateTime DateTo { get; init; }
}
