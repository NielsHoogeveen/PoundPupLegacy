namespace PoundPupLegacy.CreateModel;

public abstract record MultiQuestionPollQuestion : PollQuestion
{
    private MultiQuestionPollQuestion() { }
    public required PollQuestionDetails PollQuestionDetails { get; init; }
    public required SimpleTextNodeDetails SimpleTextNodeDetails { get; init; }
    public sealed record ToCreate : MultiQuestionPollQuestion, PollQuestionToCreate
    {
        public required Identification.Possible Identification { get; init; }
        public required NodeDetails.ForCreate NodeDetails { get; init; }

    }
    public sealed record ToUpdate : MultiQuestionPollQuestion, PollQuestionToUpdate
    {
        public required Identification.Certain Identification { get; init; }
        public required NodeDetails.ForUpdate NodeDetails { get; init; }
    }
}

