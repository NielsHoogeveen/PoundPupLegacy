namespace PoundPupLegacy.CreateModel;

public abstract record InformalIntermediateLevelSubdivision : IntermediateLevelSubdivision
{
    private InformalIntermediateLevelSubdivision() { }
    public required SubdivisionDetails SubdivisionDetails { get; init; }
    public abstract Identification Identification { get; }
    public abstract NodeDetails NodeDetails { get; }
    public abstract NameableDetails NameableDetails { get; }

    public abstract T Match<T>(Func<InformalIntermediateLevelSubdivisionToCreate, T> create, Func<InformalIntermediateLevelSubdivisionToUpdate, T> update);
    public abstract void Match(Action<InformalIntermediateLevelSubdivisionToCreate> create, Action<InformalIntermediateLevelSubdivisionToUpdate> update);

    public sealed record InformalIntermediateLevelSubdivisionToCreate : InformalIntermediateLevelSubdivision, IntermediateLevelSubdivisionToCreate
    {
        public override Identification Identification => IdentificationForCreate;
        public override NodeDetails NodeDetails => NodeDetailsForCreate;
        public override NameableDetails NameableDetails => NameableDetailsForCreate;
        public required Identification.IdentificationForCreate IdentificationForCreate { get; init; }
        public required NodeDetails.NodeDetailsForCreate NodeDetailsForCreate { get; init; }
        public required NameableDetails.NameableDetailsForCreate NameableDetailsForCreate { get; init; }
        public override T Match<T>(Func<InformalIntermediateLevelSubdivisionToCreate, T> create, Func<InformalIntermediateLevelSubdivisionToUpdate, T> update)
        {
            return create(this);
        }
        public override void Match(Action<InformalIntermediateLevelSubdivisionToCreate> create, Action<InformalIntermediateLevelSubdivisionToUpdate> update)
        {
            create(this);
        }
    }
    public sealed record InformalIntermediateLevelSubdivisionToUpdate : InformalIntermediateLevelSubdivision, IntermediateLevelSubdivisionToUpdate
    {
        public required Identification.IdentificationForUpdate IdentificationForUpdate { get; init; }
        public required NodeDetails.NodeDetailsForUpdate NodeDetailsForUpdate { get; init; }
        public override NameableDetails NameableDetails => NameableDetailsForUpdate;
        public override Identification Identification => IdentificationForUpdate;
        public override NodeDetails NodeDetails => NodeDetailsForUpdate;
        public required NameableDetails.NameableDetailsForUpdate NameableDetailsForUpdate { get; init; }
        public override T Match<T>(Func<InformalIntermediateLevelSubdivisionToCreate, T> create, Func<InformalIntermediateLevelSubdivisionToUpdate, T> update)
        {
            return update(this);
        }
        public override void Match(Action<InformalIntermediateLevelSubdivisionToCreate> create, Action<InformalIntermediateLevelSubdivisionToUpdate> update)
        {
            update(this);
        }
    }
}
