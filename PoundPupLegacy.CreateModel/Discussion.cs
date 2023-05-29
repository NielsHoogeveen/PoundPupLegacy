namespace PoundPupLegacy.CreateModel;

public abstract record Discussion : SimpleTextNode
{
    private Discussion() { }
    public required SimpleTextNodeDetails SimpleTextNodeDetails { get; init; }
    public abstract Identification Identification { get; }
    public abstract NodeDetails NodeDetails { get; }
    public abstract T Match<T>(Func<DiscussionToCreate, T> create, Func<DiscussionToUpdate, T> update);
    public abstract void Match(Action<DiscussionToCreate> create, Action<DiscussionToUpdate> update);

    public sealed record DiscussionToCreate : Discussion, SimpleTextNodeToCreate
    {
        public required Identification.IdentificationForCreate IdentificationForCreate { get; init; }
        public required NodeDetails.NodeDetailsForCreate NodeDetailsForCreate { get; init; }

        public override Identification Identification => IdentificationForCreate;

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
    public sealed record DiscussionToUpdate : Discussion, SimpleTextNodeToUpdate
    {
        public override Identification Identification => IdentificationForUpdate;

        public override NodeDetails NodeDetails => NodeDetailsForUpdate;

        public required Identification.IdentificationForUpdate IdentificationForUpdate { get; init; }
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
