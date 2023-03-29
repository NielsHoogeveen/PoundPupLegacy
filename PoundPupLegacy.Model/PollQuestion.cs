namespace PoundPupLegacy.CreateModel;

public interface PollQuestion : SimpleTextNode
{
    string Question { get; }
    List<PollOption> PollOptions { get; }

    List<PollVote> PollVotes { get; }
}
