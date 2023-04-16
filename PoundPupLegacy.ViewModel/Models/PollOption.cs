namespace PoundPupLegacy.ViewModel.Models;

public record PollOption
{
    public required string Text { get; init; }
    public required int NumberOfVotes { get; init; }
    public required decimal Percentage { get; init; }
    public required int Delta { get; init; }
}
