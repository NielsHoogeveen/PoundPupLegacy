namespace PoundPupLegacy.CreateModel;

public record MultiQuestionPollPollQuestion: IRequest
{
    public required int MultiQuestionPollId { get; init; }
    public required int PollQuestionId { get; init; }
    public required int Delta { get; init; }
}
