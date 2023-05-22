namespace PoundPupLegacy.CreateModel;

public interface ImmediatelyIdentifiablePollQuestion : PollQuestion, ImmediatelyIdentifiableSimpleTextNode
{
}

public interface EventuallyIdentifiablePollQuestion : PollQuestion, EventuallyIdentifiableSimpleTextNode
{
}

public interface PollQuestion : SimpleTextNode
{
    string Question { get; }
    List<PollOption> PollOptions { get; }
    List<PollVote> PollVotes { get; }
}

public abstract record NewPollQuestionBase: NewSimpleTextNodeBase, EventuallyIdentifiablePollQuestion
{
    public required string Question { get; init; }
    public required List<PollOption> PollOptions { get; init; }
    public required List<PollVote> PollVotes { get; init; }
}
public abstract record ExistingPollQuestionBase : ExistingSimpleTextNodeBase, ImmediatelyIdentifiablePollQuestion
{
    public required string Question { get; init; }
    public required List<PollOption> PollOptions { get; init; }
    public required List<PollVote> PollVotes { get; init; }
}