namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(PollOption))]
public partial class PollOptionJsonContext : JsonSerializerContext { }

public sealed record PollOption
{
    public required string Text { get; init; }
    public required int NumberOfVotes { get; init; }
    public required decimal Percentage { get; init; }
    public required int Delta { get; init; }
}
