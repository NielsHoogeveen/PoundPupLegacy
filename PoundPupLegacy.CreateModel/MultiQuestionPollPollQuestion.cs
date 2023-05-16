namespace PoundPupLegacy.CreateModel;

public sealed record MultiQuestionPollPollQuestion : IRequest
{
    public required int MultiQuestionPollId { get; init; }
    public required int PollQuestionId { get; init; }
    public required int Delta { get; init; }
}
