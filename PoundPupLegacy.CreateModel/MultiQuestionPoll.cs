namespace PoundPupLegacy.CreateModel;

public sealed record NewMultiQuestionPoll : NewSimpleTextNodeBase, EventuallyIdentifiableMultiQuestionPoll
{
    public required int PollStatusId { get; init; }
    public required DateTime DateTimeClosure { get; init; }
    public required List<NewBasicPollQuestion> PollQuestions { get; init; }

}
public sealed record ExistingMultiQuestionPoll : ExistingSimpleTextNodeBase, ImmediatelyIdentifiableMultiQuestionPoll
{
    public required int PollStatusId { get; init; }
    public required DateTime DateTimeClosure { get; init; }
    public required List<NewBasicPollQuestion> PollQuestions { get; init; }

}
public interface ImmediatelyIdentifiableMultiQuestionPoll : MultiQuestionPoll, ImmediatelyIdentifiablePoll, ImmediatelyIdentifiableSimpleTextNode
{
}
public interface EventuallyIdentifiableMultiQuestionPoll : MultiQuestionPoll, EventuallyIdentifiablePoll, EventuallyIdentifiableSimpleTextNode
{
}
public interface MultiQuestionPoll : Poll, SimpleTextNode
{
    List<NewBasicPollQuestion> PollQuestions { get;  }
}
