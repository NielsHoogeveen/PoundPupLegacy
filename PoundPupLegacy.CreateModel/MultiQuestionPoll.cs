namespace PoundPupLegacy.CreateModel;

public abstract record MultiQuestionPoll : SimpleTextNode, Poll
{
    private MultiQuestionPoll() { }
    public required MultiQuestionPollDetails MultiQuestionPollDetails { get; init; }
    public required PollDetails PollDetails { get; init; }
    public required SimpleTextNodeDetails SimpleTextNodeDetails { get; init; }
    public abstract NodeIdentification NodeIdentification { get; }
    public abstract NodeDetails NodeDetails { get; }
    public abstract T Match<T>(Func<MultiQuestionPollToCreate, T> create, Func<MultiQuestionPollToUpdate, T> update);
    public abstract void Match(Action<MultiQuestionPollToCreate> create, Action<MultiQuestionPollToUpdate> update);

    public sealed record MultiQuestionPollToCreate : MultiQuestionPoll, NodeToCreate
    {
        public required NodeIdentification.NodeIdentificationForCreate NodeIdentificationForCreate { get; init; }
        public required NodeDetails.NodeDetailsForCreate NodeDetailsForCreate { get; init; }

        public override NodeIdentification NodeIdentification => NodeIdentificationForCreate;

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
    public sealed record MultiQuestionPollToUpdate : MultiQuestionPoll, NodeToUpdate
    {
        public override NodeIdentification NodeIdentification => NodeIdentificationForUpdate;

        public override NodeDetails NodeDetails => NodeDetailsForUpdate;

        public required NodeIdentification.NodeIdentificationForUpdate NodeIdentificationForUpdate { get; init; }
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

public record MultiQuestionPollDetails
{
    public required List<NewBasicPollQuestion> PollQuestions { get; init; }
}
