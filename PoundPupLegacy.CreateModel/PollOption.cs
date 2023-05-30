namespace PoundPupLegacy.CreateModel;

public sealed record PollOption : IRequest
{
    public required int? PollQuestionId { get; set; }
    public required int Delta { get; init; }
    public required string Text { get; init; }
    public required int NumberOfVotes { get; init; }
}
