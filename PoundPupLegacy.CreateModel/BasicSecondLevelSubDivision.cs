namespace PoundPupLegacy.CreateModel;

public abstract record BasicSecondLevelSubdivision : SecondLevelSubdivision
{
    private BasicSecondLevelSubdivision() { }
    public required PoliticalEntityDetails PoliticalEntityDetails { get; init; }
    public required ISOCodedSubdivisionDetails ISOCodedSubdivisionDetails { get; init; }
    public required SubdivisionDetails SubdivisionDetails { get; init; }
    public abstract Identification Identification { get; }
    public abstract NodeDetails NodeDetails { get; }
    public abstract NameableDetails NameableDetails { get; }
    public required int IntermediateLevelSubdivisionId { get; init; }
    public abstract T Match<T>(Func<BasicSecondLevelSubdivisionToCreate, T> create, Func<BasicSecondLevelSubdivisionToUpdate, T> update);
    public abstract void Match(Action<BasicSecondLevelSubdivisionToCreate> create, Action<BasicSecondLevelSubdivisionToUpdate> update);

    public sealed record BasicSecondLevelSubdivisionToCreate : BasicSecondLevelSubdivision, FirstAndSecondLevelSubdivisionToCreate
    {
        public override Identification Identification => IdentificationForCreate;
        public override NodeDetails NodeDetails => NodeDetailsForCreate;
        public override NameableDetails NameableDetails => NameableDetailsForCreate;
        public required Identification.IdentificationForCreate IdentificationForCreate { get; init; }
        public required NodeDetails.NodeDetailsForCreate NodeDetailsForCreate { get; init; }
        public required NameableDetails.NameableDetailsForCreate NameableDetailsForCreate { get; init; }
        public override T Match<T>(Func<BasicSecondLevelSubdivisionToCreate, T> create, Func<BasicSecondLevelSubdivisionToUpdate, T> update)
        {
            return create(this);
        }
        public override void Match(Action<BasicSecondLevelSubdivisionToCreate> create, Action<BasicSecondLevelSubdivisionToUpdate> update)
        {
            create(this);
        }
    }
    public sealed record BasicSecondLevelSubdivisionToUpdate : BasicSecondLevelSubdivision, FirstAndSecondLevelSubdivisionToUpdate
    {
        public required Identification.IdentificationForUpdate IdentificationForUpdate { get; init; }
        public required NodeDetails.NodeDetailsForUpdate NodeDetailsForUpdate { get; init; }
        public override NameableDetails NameableDetails => NameableDetailsForUpdate;
        public override Identification Identification => IdentificationForUpdate;
        public override NodeDetails NodeDetails => NodeDetailsForUpdate;
        public required NameableDetails.NameableDetailsForUpdate NameableDetailsForUpdate { get; init; }
        public override T Match<T>(Func<BasicSecondLevelSubdivisionToCreate, T> create, Func<BasicSecondLevelSubdivisionToUpdate, T> update)
        {
            return update(this);
        }
        public override void Match(Action<BasicSecondLevelSubdivisionToCreate> create, Action<BasicSecondLevelSubdivisionToUpdate> update)
        {
            update(this);
        }
    }
}