namespace PoundPupLegacy.CreateModel;

public abstract record BasicNode: Node
{
    private BasicNode() { }
    public abstract Identification Identification { get; }
    public abstract NodeDetails NodeDetails { get; }
    public abstract T Match<T>(Func<BasicNodeToCreate, T> create, Func<BasicNodeToUpdate, T> update);
    public abstract void Match(Action<BasicNodeToCreate> create, Action<BasicNodeToUpdate> update);

    public sealed record BasicNodeToCreate : BasicNode, NodeToCreate
    {
        public required Identification.IdentificationForCreate IdentificationForCreate { get; init; }
        public required NodeDetails.NodeDetailsForCreate NodeDetailsForCreate { get; init; }

        public override Identification Identification => IdentificationForCreate;

        public override NodeDetails NodeDetails => NodeDetailsForCreate;
        public override T Match<T>(Func<BasicNodeToCreate, T> create, Func<BasicNodeToUpdate, T> update)
        {
            return create(this);
        }
        public override void Match(Action<BasicNodeToCreate> create, Action<BasicNodeToUpdate> update)
        {
            create(this);
        }
    }
    public sealed record BasicNodeToUpdate : BasicNode, NodeToUpdate
    {
        public override Identification Identification => IdentificationForUpdate;
        public override NodeDetails NodeDetails => NodeDetailsForUpdate;
        public required Identification.IdentificationForUpdate IdentificationForUpdate { get; init; }
        public required NodeDetails.NodeDetailsForUpdate NodeDetailsForUpdate { get; init; }
        public override T Match<T>(Func<BasicNodeToCreate, T> create, Func<BasicNodeToUpdate, T> update)
        {
            return update(this);
        }
        public override void Match(Action<BasicNodeToCreate> create, Action<BasicNodeToUpdate> update)
        {
            update(this);
        }
    }
}
