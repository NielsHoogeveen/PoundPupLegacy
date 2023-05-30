
namespace PoundPupLegacy.CreateModel;

public interface PollQuestionToCreate : PollQuestion, SimpleTextNodeToCreate
{
}
public interface PollQuestionToUpdate : PollQuestion, SimpleTextNodeToUpdate
{
}
public interface PollQuestion : SimpleTextNode
{
    PollQuestionDetails PollQuestionDetails {get;}
}
public sealed record PollQuestionDetails
{
    public required string Question { get; init; }
    public required List<PollOption> PollOptions { get; init; }
    public required List<PollVote> PollVotes { get; init; }
}
