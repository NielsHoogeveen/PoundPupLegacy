namespace PoundPupLegacy.CreateModel;

public abstract record MultiQuestionPollQuestion : PollQuestion
{
    private MultiQuestionPollQuestion() { }
    public required PollQuestionDetails PollQuestionDetails { get; init; }
    public required SimpleTextNodeDetails SimpleTextNodeDetails { get; init; }
    public abstract Identification Identification { get; }
    public abstract NodeDetails NodeDetails { get; }
    public abstract T Match<T>(Func<MultiQuestionPollQuestionToCreate, T> create, Func<MultiQuestionPollQuestionToUpdate, T> update);
    public abstract void Match(Action<MultiQuestionPollQuestionToCreate> create, Action<MultiQuestionPollQuestionToUpdate> update);

    public sealed record MultiQuestionPollQuestionToCreate : MultiQuestionPollQuestion, PollQuestionToCreate
    {
        public required Identification.IdentificationForCreate IdentificationForCreate { get; init; }
        public required NodeDetails.NodeDetailsForCreate NodeDetailsForCreate { get; init; }

        public override Identification Identification => IdentificationForCreate;

        public override NodeDetails NodeDetails => NodeDetailsForCreate;
        public override T Match<T>(Func<MultiQuestionPollQuestionToCreate, T> create, Func<MultiQuestionPollQuestionToUpdate, T> update)
        {
            return create(this);
        }
        public override void Match(Action<MultiQuestionPollQuestionToCreate> create, Action<MultiQuestionPollQuestionToUpdate> update)
        {
            create(this);
        }
    }
    public sealed record MultiQuestionPollQuestionToUpdate : MultiQuestionPollQuestion, PollQuestionToUpdate
    {
        public override Identification Identification => IdentificationForUpdate;

        public override NodeDetails NodeDetails => NodeDetailsForUpdate;

        public required Identification.IdentificationForUpdate IdentificationForUpdate { get; init; }
        public required NodeDetails.NodeDetailsForUpdate NodeDetailsForUpdate { get; init; }
        public override T Match<T>(Func<MultiQuestionPollQuestionToCreate, T> create, Func<MultiQuestionPollQuestionToUpdate, T> update)
        {
            return update(this);
        }
        public override void Match(Action<MultiQuestionPollQuestionToCreate> create, Action<MultiQuestionPollQuestionToUpdate> update)
        {
            update(this);
        }
    }
}

