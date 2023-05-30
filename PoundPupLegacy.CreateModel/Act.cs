namespace PoundPupLegacy.CreateModel;

public abstract record Act : Nameable, Documentable
{
    private Act() { }
    public required ActDetails ActDetails { get; init; }
    public abstract Identification Identification { get; }
    public abstract NodeDetails NodeDetails { get; }
    public abstract NameableDetails NameableDetails { get; }
    public abstract T Match<T>(Func<ActToCreate, T> create, Func<ActToUpdate, T> update);
    public abstract void Match(Action<ActToCreate> create, Action<ActToUpdate> update);

    public sealed record ActToCreate : Act, NameableToCreate, DocumentableToCreate
    {
        public override Identification Identification => IdentificationForCreate;
        public override NodeDetails NodeDetails => NodeDetailsForCreate;
        public override NameableDetails NameableDetails => NameableDetailsForCreate;
        public required Identification.IdentificationForCreate IdentificationForCreate { get; init; }
        public required NodeDetails.NodeDetailsForCreate NodeDetailsForCreate { get; init; }
        public required NameableDetails.NameableDetailsForCreate NameableDetailsForCreate { get; init; }
        public override T Match<T>(Func<ActToCreate, T> create, Func<ActToUpdate, T> update)
        {
            return create(this);
        }
        public override void Match(Action<ActToCreate> create, Action<ActToUpdate> update)
        {
            create(this);
        }
    }
    public sealed record ActToUpdate : Act, NameableToUpdate, DocumentableToUpdate
    {
        public required Identification.IdentificationForUpdate IdentificationForUpdate { get; init; }
        public required NodeDetails.NodeDetailsForUpdate NodeDetailsForUpdate { get; init; }
        public override NameableDetails NameableDetails => NameableDetailsForUpdate;
        public override Identification Identification => IdentificationForUpdate;
        public override NodeDetails NodeDetails => NodeDetailsForUpdate;
        public required NameableDetails.NameableDetailsForUpdate NameableDetailsForUpdate { get; init; }
        public override T Match<T>(Func<ActToCreate, T> create, Func<ActToUpdate, T> update)
        {
            return update(this);
        }
        public override void Match(Action<ActToCreate> create, Action<ActToUpdate> update)
        {
            update(this);
        }
    }
}


public sealed record ActDetails
{
    public required DateTime? EnactmentDate { get; init; }
}
