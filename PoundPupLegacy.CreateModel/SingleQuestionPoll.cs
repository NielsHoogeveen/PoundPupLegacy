namespace PoundPupLegacy.CreateModel;

public abstract record SingleQuestionPoll : PollQuestion, Poll
{
    private SingleQuestionPoll() { }
    public required PollDetails PollDetails { get; init; }
    public required PollQuestionDetails PollQuestionDetails { get; init; }
    public required SimpleTextNodeDetails SimpleTextNodeDetails { get; init; }
    public sealed record ToCreate : SingleQuestionPoll, PollQuestionToCreate, PollToCreate
    {
        public required Identification.Possible Identification { get; init; }
        public required NodeDetails.ForCreate NodeDetails { get; init; }

    }
    public sealed record ToUpdate : SingleQuestionPoll, PollQuestionToUpdate, PollToUpdate
    {
        public required Identification.Certain Identification { get; init; }
        public required NodeDetails.ForUpdate NodeDetails { get; init; }
    }
}

