namespace PoundPupLegacy.CreateModel;

public abstract record SingleQuestionPoll : PollQuestion, Poll
{
    private SingleQuestionPoll() { }
    public required PollDetails PollDetails { get; init; }
    public required PollQuestionDetails PollQuestionDetails { get; init; }
    public required SimpleTextNodeDetails SimpleTextNodeDetails { get; init; }
    public abstract NodeIdentification NodeIdentification { get; }
    public abstract NodeDetails NodeDetails { get; }
    public abstract T Match<T>(Func<SingleQuestionPollToCreate, T> create, Func<SingleQuestionPollToUpdate, T> update);
    public abstract void Match(Action<SingleQuestionPollToCreate> create, Action<SingleQuestionPollToUpdate> update);

    public sealed record SingleQuestionPollToCreate : SingleQuestionPoll, PollQuestionToCreate
    {
        public required NodeIdentification.NodeIdentificationForCreate NodeIdentificationForCreate { get; init; }
        public required NodeDetails.NodeDetailsForCreate NodeDetailsForCreate { get; init; }

        public override NodeIdentification NodeIdentification => NodeIdentificationForCreate;

        public override NodeDetails NodeDetails => NodeDetailsForCreate;
        public override T Match<T>(Func<SingleQuestionPollToCreate, T> create, Func<SingleQuestionPollToUpdate, T> update)
        {
            return create(this);
        }
        public override void Match(Action<SingleQuestionPollToCreate> create, Action<SingleQuestionPollToUpdate> update)
        {
            create(this);
        }
    }
    public sealed record SingleQuestionPollToUpdate : SingleQuestionPoll, PollQuestionToUpdate
    {
        public override NodeIdentification NodeIdentification => NodeIdentificationForUpdate;

        public override NodeDetails NodeDetails => NodeDetailsForUpdate;

        public required NodeIdentification.NodeIdentificationForUpdate NodeIdentificationForUpdate { get; init; }
        public required NodeDetails.NodeDetailsForUpdate NodeDetailsForUpdate { get; init; }
        public override T Match<T>(Func<SingleQuestionPollToCreate, T> create, Func<SingleQuestionPollToUpdate, T> update)
        {
            return update(this);
        }
        public override void Match(Action<SingleQuestionPollToCreate> create, Action<SingleQuestionPollToUpdate> update)
        {
            update(this);
        }
    }
}

