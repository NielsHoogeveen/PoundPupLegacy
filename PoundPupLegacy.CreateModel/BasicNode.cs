namespace PoundPupLegacy.CreateModel;

public abstract record BasicNode: Node
{
    private BasicNode() { }
    public abstract NodeIdentification NodeIdentification { get; }
    public abstract NodeDetails NodeDetails { get; }
    public abstract T Match<T>(Func<BasicNodeToCreate, T> create, Func<BasicNodeToUpdate, T> update);
    public abstract void Match(Action<BasicNodeToCreate> create, Action<BasicNodeToUpdate> update);

    public sealed record BasicNodeToCreate : BasicNode, NodeToCreate
    {
        public required NodeIdentification.NodeIdentificationForCreate NodeIdentificationForCreate { get; init; }
        public required NodeDetails.NodeDetailsForCreate NodeDetailsForCreate { get; init; }

        public override NodeIdentification NodeIdentification => NodeIdentificationForCreate;

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
        public override NodeIdentification NodeIdentification => NodeIdentificationForUpdate;
        public override NodeDetails NodeDetails => NodeDetailsForUpdate;
        public required NodeIdentification.NodeIdentificationForUpdate NodeIdentificationForUpdate { get; init; }
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
