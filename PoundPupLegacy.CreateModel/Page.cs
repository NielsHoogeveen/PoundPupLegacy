namespace PoundPupLegacy.CreateModel;

public abstract record Page : SimpleTextNode
{
    private Page() { }
    public required SimpleTextNodeDetails SimpleTextNodeDetails { get; init; }
    public abstract NodeIdentification NodeIdentification { get; }
    public abstract NodeDetails NodeDetails { get; }
    public abstract T Match<T>(Func<PageToCreate, T> create, Func<PageToUpdate, T> update);
    public abstract void Match(Action<PageToCreate> create, Action<PageToUpdate> update);

    public sealed record PageToCreate : Page, NodeToCreate
    {
        public required NodeIdentification.NodeIdentificationForCreate NodeIdentificationForCreate { get; init; }
        public required NodeDetails.NodeDetailsForCreate NodeDetailsForCreate { get; init; }

        public override NodeIdentification NodeIdentification => NodeIdentificationForCreate;

        public override NodeDetails NodeDetails => NodeDetailsForCreate;
        public override T Match<T>(Func<PageToCreate, T> create, Func<PageToUpdate, T> update)
        {
            return create(this);
        }
        public override void Match(Action<PageToCreate> create, Action<PageToUpdate> update)
        {
            create(this);
        }
    }
    public sealed record PageToUpdate : Page, NodeToUpdate
    {
        public override NodeIdentification NodeIdentification => NodeIdentificationForUpdate;

        public override NodeDetails NodeDetails => NodeDetailsForUpdate;

        public required NodeIdentification.NodeIdentificationForUpdate NodeIdentificationForUpdate { get; init; }
        public required NodeDetails.NodeDetailsForUpdate NodeDetailsForUpdate { get; init; }
        public override T Match<T>(Func<PageToCreate, T> create, Func<PageToUpdate, T> update)
        {
            return update(this);
        }
        public override void Match(Action<PageToCreate> create, Action<PageToUpdate> update)
        {
            update(this);
        }
    }
}
