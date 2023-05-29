namespace PoundPupLegacy.CreateModel;

public abstract record Review : SimpleTextNode
{
    private Review() { }
    public required SimpleTextNodeDetails SimpleTextNodeDetails { get; init; }
    public abstract Identification Identification { get; }
    public abstract NodeDetails NodeDetails { get; }
    public abstract T Match<T>(Func<ReviewToCreate, T> create, Func<ReviewToUpdate, T> update);
    public abstract void Match(Action<ReviewToCreate> create, Action<ReviewToUpdate> update);

    public sealed record ReviewToCreate : Review, NodeToCreate
    {
        public required Identification.IdentificationForCreate IdentificationForCreate { get; init; }
        public required NodeDetails.NodeDetailsForCreate NodeDetailsForCreate { get; init; }

        public override Identification Identification => IdentificationForCreate;

        public override NodeDetails NodeDetails => NodeDetailsForCreate;
        public override T Match<T>(Func<ReviewToCreate, T> create, Func<ReviewToUpdate, T> update)
        {
            return create(this);
        }
        public override void Match(Action<ReviewToCreate> create, Action<ReviewToUpdate> update)
        {
            create(this);
        }
    }
    public sealed record ReviewToUpdate : Review, NodeToUpdate
    {
        public override Identification Identification => IdentificationForUpdate;

        public override NodeDetails NodeDetails => NodeDetailsForUpdate;

        public required Identification.IdentificationForUpdate IdentificationForUpdate { get; init; }
        public required NodeDetails.NodeDetailsForUpdate NodeDetailsForUpdate { get; init; }
        public override T Match<T>(Func<ReviewToCreate, T> create, Func<ReviewToUpdate, T> update)
        {
            return update(this);
        }
        public override void Match(Action<ReviewToCreate> create, Action<ReviewToUpdate> update)
        {
            update(this);
        }
    }
}
