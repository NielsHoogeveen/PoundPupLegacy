namespace PoundPupLegacy.DomainModel;

public abstract record MultiQuestionPoll : SimpleTextNode, Poll
{
    private MultiQuestionPoll() { }
    public required PollDetails PollDetails { get; init; }
    public required SimpleTextNodeDetails SimpleTextNodeDetails { get; init; }
    public sealed record ToCreate : MultiQuestionPoll, SimpleTextNodeToCreate, PollToCreate
    {
        public required MultiQuestionPollDetailsForCreate MultiQuestionPollDetails { get; init; }
        public required Identification.Possible Identification { get; init; }
        public required NodeDetails.ForCreate NodeDetails { get; init; }
    }
    public sealed record MultiQuestionPollToUpdate : MultiQuestionPoll, SimpleTextNodeToUpdate, PollToUpdate
    {
        public required Identification.Certain Identification { get; init; }
        public required NodeDetails.ForUpdate NodeDetails { get; init; }
    }
}

public record MultiQuestionPollDetailsForCreate
{
    public required List<MultiQuestionPollQuestion.ToCreate> PollQuestions { get; init; }
}
