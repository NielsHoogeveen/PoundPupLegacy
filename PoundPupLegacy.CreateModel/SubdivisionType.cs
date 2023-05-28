namespace PoundPupLegacy.CreateModel;

public abstract record SubdivisionType : Nameable
{
    private SubdivisionType() { }
    public abstract NodeIdentification NodeIdentification { get; }
    public abstract NodeDetails NodeDetails { get; }
    public abstract NameableDetails NameableDetails { get; }
    public abstract T Match<T>(Func<SubdivisionTypeToCreate, T> create, Func<SubdivisionTypeToUpdate, T> update);
    public abstract void Match(Action<SubdivisionTypeToCreate> create, Action<SubdivisionTypeToUpdate> update);

    public sealed record SubdivisionTypeToCreate : SubdivisionType, NameableToCreate
    {
        public override NodeIdentification NodeIdentification => NodeIdentificationForCreate;
        public override NodeDetails NodeDetails => NodeDetailsForCreate;
        public override NameableDetails NameableDetails => NameableDetailsForCreate;
        public required NodeIdentification.NodeIdentificationForCreate NodeIdentificationForCreate { get; init; }
        public required NodeDetails.NodeDetailsForCreate NodeDetailsForCreate { get; init; }
        public required NameableDetails.NameableDetailsForCreate NameableDetailsForCreate { get; init; }
        public override T Match<T>(Func<SubdivisionTypeToCreate, T> create, Func<SubdivisionTypeToUpdate, T> update)
        {
            return create(this);
        }
        public override void Match(Action<SubdivisionTypeToCreate> create, Action<SubdivisionTypeToUpdate> update)
        {
            create(this);
        }
    }
    public sealed record SubdivisionTypeToUpdate : SubdivisionType, NameableToUpdate
    {
        public required NodeIdentification.NodeIdentificationForUpdate NodeIdentificationForUpdate { get; init; }
        public required NodeDetails.NodeDetailsForUpdate NodeDetailsForUpdate { get; init; }
        public override NameableDetails NameableDetails => NameableDetailsForUpdate;
        public override NodeIdentification NodeIdentification => NodeIdentificationForUpdate;
        public override NodeDetails NodeDetails => NodeDetailsForUpdate;
        public required NameableDetails.NameableDetailsForUpdate NameableDetailsForUpdate { get; init; }
        public override T Match<T>(Func<SubdivisionTypeToCreate, T> create, Func<SubdivisionTypeToUpdate, T> update)
        {
            return update(this);
        }
        public override void Match(Action<SubdivisionTypeToCreate> create, Action<SubdivisionTypeToUpdate> update)
        {
            update(this);
        }
    }
}
