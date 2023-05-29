namespace PoundPupLegacy.CreateModel;

public abstract record MultiQuestionPoll : SimpleTextNode, Poll
{
    private MultiQuestionPoll() { }
    
    public required PollDetails PollDetails { get; init; }
    public required SimpleTextNodeDetails SimpleTextNodeDetails { get; init; }
    public abstract Identification Identification { get; }
    public abstract NodeDetails NodeDetails { get; }
    public abstract T Match<T>(Func<MultiQuestionPollToCreate, T> create, Func<MultiQuestionPollToUpdate, T> update);
    public abstract void Match(Action<MultiQuestionPollToCreate> create, Action<MultiQuestionPollToUpdate> update);

    public sealed record MultiQuestionPollToCreate : MultiQuestionPoll, SimpleTextNodeToCreate, PollToCreate
    {
        public required MultiQuestionPollDetailsForCreate MultiQuestionPollDetails { get; init; }
        public required Identification.IdentificationForCreate IdentificationForCreate { get; init; }
        public required NodeDetails.NodeDetailsForCreate NodeDetailsForCreate { get; init; }

        public override Identification Identification => IdentificationForCreate;

        public override NodeDetails NodeDetails => NodeDetailsForCreate;
        public override T Match<T>(Func<MultiQuestionPollToCreate, T> create, Func<MultiQuestionPollToUpdate, T> update)
        {
            return create(this);
        }
        public override void Match(Action<MultiQuestionPollToCreate> create, Action<MultiQuestionPollToUpdate> update)
        {
            create(this);
        }
    }
    public sealed record MultiQuestionPollToUpdate : MultiQuestionPoll, SimpleTextNodeToUpdate, PollToUpdate
    {
        public override Identification Identification => IdentificationForUpdate;

        public override NodeDetails NodeDetails => NodeDetailsForUpdate;

        public required Identification.IdentificationForUpdate IdentificationForUpdate { get; init; }
        public required NodeDetails.NodeDetailsForUpdate NodeDetailsForUpdate { get; init; }
        public override T Match<T>(Func<MultiQuestionPollToCreate, T> create, Func<MultiQuestionPollToUpdate, T> update)
        {
            return update(this);
        }
        public override void Match(Action<MultiQuestionPollToCreate> create, Action<MultiQuestionPollToUpdate> update)
        {
            update(this);
        }
    }
}

public record MultiQuestionPollDetailsForCreate
{
    public required List<MultiQuestionPollQuestion.MultiQuestionPollQuestionToCreate> PollQuestions { get; init; }
}
