namespace PoundPupLegacy.CreateModel;

public abstract record SingleQuestionPoll : PollQuestion, Poll
{
    private SingleQuestionPoll() { }
    public required PollDetails PollDetails { get; init; }
    public required PollQuestionDetails PollQuestionDetails { get; init; }
    public required SimpleTextNodeDetails SimpleTextNodeDetails { get; init; }
    public abstract Identification Identification { get; }
    public abstract NodeDetails NodeDetails { get; }
    public abstract T Match<T>(Func<SingleQuestionPollToCreate, T> create, Func<SingleQuestionPollToUpdate, T> update);
    public abstract void Match(Action<SingleQuestionPollToCreate> create, Action<SingleQuestionPollToUpdate> update);

    public sealed record SingleQuestionPollToCreate : SingleQuestionPoll, PollQuestionToCreate, PollToCreate
    {
        public required Identification.IdentificationForCreate IdentificationForCreate { get; init; }
        public required NodeDetails.NodeDetailsForCreate NodeDetailsForCreate { get; init; }

        public override Identification Identification => IdentificationForCreate;

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
    public sealed record SingleQuestionPollToUpdate : SingleQuestionPoll, PollQuestionToUpdate, PollToUpdate
    {
        public override Identification Identification => IdentificationForUpdate;

        public override NodeDetails NodeDetails => NodeDetailsForUpdate;

        public required Identification.IdentificationForUpdate IdentificationForUpdate { get; init; }
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

