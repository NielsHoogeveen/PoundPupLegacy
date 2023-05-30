namespace PoundPupLegacy.CreateModel;

public abstract record SingleQuestionPoll : PollQuestion, Poll
{
    private SingleQuestionPoll() { }
    public required PollDetails PollDetails { get; init; }
    public required PollQuestionDetails PollQuestionDetails { get; init; }
    public required SimpleTextNodeDetails SimpleTextNodeDetails { get; init; }
    public abstract Identification Identification { get; }
    public abstract NodeDetails NodeDetails { get; }
    public sealed record ToCreate : SingleQuestionPoll, PollQuestionToCreate, PollToCreate
    {
        public required Identification.Possible IdentificationForCreate { get; init; }
        public required NodeDetails.NodeDetailsForCreate NodeDetailsForCreate { get; init; }

        public override Identification Identification => IdentificationForCreate;

        public override NodeDetails NodeDetails => NodeDetailsForCreate;
    }
    public sealed record ToUpdate : SingleQuestionPoll, PollQuestionToUpdate, PollToUpdate
    {
        public override Identification Identification => IdentificationCertain;

        public override NodeDetails NodeDetails => NodeDetailsForUpdate;

        public required Identification.Certain IdentificationCertain { get; init; }
        public required NodeDetails.ForUpdate NodeDetailsForUpdate { get; init; }
    }
}

