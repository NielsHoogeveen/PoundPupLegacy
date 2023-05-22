namespace PoundPupLegacy.CreateModel;

public sealed record NewSingleQuestionPoll : NewPollQuestionBase, EventuallyIdentifiableSingleQuestionPoll
{
    public required int PollStatusId { get; init; }
    public required DateTime DateTimeClosure { get; init; }
}
public sealed record ExistingSingleQuestionPoll : ExistingPollQuestionBase, ImmediatelyIdentifiableSingleQuestionPoll
{
    public required int PollStatusId { get; init; }
    public required DateTime DateTimeClosure { get; init; }
}
public interface ImmediatelyIdentifiableSingleQuestionPoll : SingleQuestionPoll, ImmediatelyIdentifiablePoll, ImmediatelyIdentifiablePollQuestion
{
}
public interface EventuallyIdentifiableSingleQuestionPoll : SingleQuestionPoll, EventuallyIdentifiablePoll, EventuallyIdentifiablePollQuestion
{
}
public interface SingleQuestionPoll : Poll, PollQuestion
{
}
