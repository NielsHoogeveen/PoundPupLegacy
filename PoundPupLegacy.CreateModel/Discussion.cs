namespace PoundPupLegacy.CreateModel;

public abstract record Discussion : SimpleTextNode
{
    private Discussion() { }
    public required SimpleTextNodeDetails SimpleTextNodeDetails { get; init; }
    public abstract NodeIdentification NodeIdentification { get; }
    public abstract NodeDetails NodeDetails { get; }
    public abstract T Match<T>(Func<DiscussionToCreate, T> create, Func<DiscussionToUpdate, T> update);
    public abstract void Match(Action<DiscussionToCreate> create, Action<DiscussionToUpdate> update);

    public sealed record DiscussionToCreate : Discussion, NodeToCreate
    {
        public required NodeIdentification.NodeIdentificationForCreate NodeIdentificationForCreate { get; init; }
        public required NodeDetails.NodeDetailsForCreate NodeDetailsForCreate { get; init; }

        public override NodeIdentification NodeIdentification => NodeIdentificationForCreate;

        public override NodeDetails NodeDetails => NodeDetailsForCreate;
        public override T Match<T>(Func<DiscussionToCreate, T> create, Func<DiscussionToUpdate, T> update)
        {
            return create(this);
        }
        public override void Match(Action<DiscussionToCreate> create, Action<DiscussionToUpdate> update)
        {
            create(this);
        }
    }
    public sealed record DiscussionToUpdate : Discussion, NodeToUpdate
    {
        public override NodeIdentification NodeIdentification => NodeIdentificationForUpdate;

        public override NodeDetails NodeDetails => NodeDetailsForUpdate;

        public required NodeIdentification.NodeIdentificationForUpdate NodeIdentificationForUpdate { get; init; }
        public required NodeDetails.NodeDetailsForUpdate NodeDetailsForUpdate { get; init; }
        public override T Match<T>(Func<DiscussionToCreate, T> create, Func<DiscussionToUpdate, T> update)
        {
            return update(this);
        }
        public override void Match(Action<DiscussionToCreate> create, Action<DiscussionToUpdate> update)
        {
            update(this);
        }
    }
}
