namespace PoundPupLegacy.CreateModel;

public abstract record Review : SimpleTextNode
{
    private Review() { }
    public required SimpleTextNodeDetails SimpleTextNodeDetails { get; init; }
    public abstract NodeIdentification NodeIdentification { get; }
    public abstract NodeDetails NodeDetails { get; }
    public abstract T Match<T>(Func<ReviewToCreate, T> create, Func<ReviewToUpdate, T> update);
    public abstract void Match(Action<ReviewToCreate> create, Action<ReviewToUpdate> update);

    public sealed record ReviewToCreate : Review, NodeToCreate
    {
        public required NodeIdentification.NodeIdentificationForCreate NodeIdentificationForCreate { get; init; }
        public required NodeDetails.NodeDetailsForCreate NodeDetailsForCreate { get; init; }

        public override NodeIdentification NodeIdentification => NodeIdentificationForCreate;

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
        public override NodeIdentification NodeIdentification => NodeIdentificationForUpdate;

        public override NodeDetails NodeDetails => NodeDetailsForUpdate;

        public required NodeIdentification.NodeIdentificationForUpdate NodeIdentificationForUpdate { get; init; }
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
