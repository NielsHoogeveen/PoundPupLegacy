namespace PoundPupLegacy.CreateModel;

public abstract record Page : SimpleTextNode
{
    private Page() { }
    public required SimpleTextNodeDetails SimpleTextNodeDetails { get; init; }
    public abstract Identification Identification { get; }
    public abstract NodeDetails NodeDetails { get; }
    public abstract T Match<T>(Func<PageToCreate, T> create, Func<PageToUpdate, T> update);
    public abstract void Match(Action<PageToCreate> create, Action<PageToUpdate> update);

    public sealed record PageToCreate : Page, SimpleTextNodeToCreate
    {
        public required Identification.IdentificationForCreate IdentificationForCreate { get; init; }
        public required NodeDetails.NodeDetailsForCreate NodeDetailsForCreate { get; init; }

        public override Identification Identification => IdentificationForCreate;

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
    public sealed record PageToUpdate : Page, SimpleTextNodeToUpdate
    {
        public override Identification Identification => IdentificationForUpdate;

        public override NodeDetails NodeDetails => NodeDetailsForUpdate;

        public required Identification.IdentificationForUpdate IdentificationForUpdate { get; init; }
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
