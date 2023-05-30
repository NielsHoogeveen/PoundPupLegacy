namespace PoundPupLegacy.CreateModel;

public abstract record MultiQuestionPoll : SimpleTextNode, Poll
{
    private MultiQuestionPoll() { }
    public required PollDetails PollDetails { get; init; }
    public required SimpleTextNodeDetails SimpleTextNodeDetails { get; init; }
    public abstract Identification Identification { get; }
    public abstract NodeDetails NodeDetails { get; }
    public sealed record ToCreate : MultiQuestionPoll, SimpleTextNodeToCreate, PollToCreate
    {
        public required MultiQuestionPollDetailsForCreate MultiQuestionPollDetails { get; init; }
        public required Identification.Possible IdentificationForCreate { get; init; }
        public required NodeDetails.NodeDetailsForCreate NodeDetailsForCreate { get; init; }

        public override Identification Identification => IdentificationForCreate;

        public override NodeDetails NodeDetails => NodeDetailsForCreate;
    }
    public sealed record MultiQuestionPollToUpdate : MultiQuestionPoll, SimpleTextNodeToUpdate, PollToUpdate
    {
        public override Identification Identification => IdentificationCertain;

        public override NodeDetails NodeDetails => NodeDetailsForUpdate;

        public required Identification.Certain IdentificationCertain { get; init; }
        public required NodeDetails.ForUpdate NodeDetailsForUpdate { get; init; }
    }
}

public record MultiQuestionPollDetailsForCreate
{
    public required List<MultiQuestionPollQuestion.ToCreate> PollQuestions { get; init; }
}
