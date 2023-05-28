namespace PoundPupLegacy.CreateModel;

public abstract record FirstAndBottomLevelSubdivision : ISOCodedFirstLevelSubdivision, BottomLevelSubdivision
{
    private FirstAndBottomLevelSubdivision() { }
    public required ISOCodedSubdivisionDetails ISOCodedSubdivisionDetails { get; init; }
    public required PoliticalEntityDetails PoliticalEntityDetails { get; init; }
    public required SubdivisionDetails SubdivisionDetails { get; init; }
    public abstract NodeIdentification NodeIdentification { get; }
    public abstract NodeDetails NodeDetails { get; }
    public abstract NameableDetails NameableDetails { get; }
    public abstract T Match<T>(Func<FirstAndBottomLevelSubdivisionToCreate, T> create, Func<FirstAndBottomLevelSubdivisionToUpdate, T> update);
    public abstract void Match(Action<FirstAndBottomLevelSubdivisionToCreate> create, Action<FirstAndBottomLevelSubdivisionToUpdate> update);

    public sealed record FirstAndBottomLevelSubdivisionToCreate : FirstAndBottomLevelSubdivision, ISOCodedFirstLevelSubdivisionToCreate, BottomLevelSubdivisionToCreate
    {
        public override NodeIdentification NodeIdentification => NodeIdentificationForCreate;
        public override NodeDetails NodeDetails => NodeDetailsForCreate;
        public override NameableDetails NameableDetails => NameableDetailsForCreate;
        public required NodeIdentification.NodeIdentificationForCreate NodeIdentificationForCreate { get; init; }
        public required NodeDetails.NodeDetailsForCreate NodeDetailsForCreate { get; init; }
        public required NameableDetails.NameableDetailsForCreate NameableDetailsForCreate { get; init; }
        public override T Match<T>(Func<FirstAndBottomLevelSubdivisionToCreate, T> create, Func<FirstAndBottomLevelSubdivisionToUpdate, T> update)
        {
            return create(this);
        }
        public override void Match(Action<FirstAndBottomLevelSubdivisionToCreate> create, Action<FirstAndBottomLevelSubdivisionToUpdate> update)
        {
            create(this);
        }
    }
    public sealed record FirstAndBottomLevelSubdivisionToUpdate : FirstAndBottomLevelSubdivision, ISOCodedFirstLevelSubdivisionToUpdate, BottomLevelSubdivisionToUpdate
    {
        public required NodeIdentification.NodeIdentificationForUpdate NodeIdentificationForUpdate { get; init; }
        public required NodeDetails.NodeDetailsForUpdate NodeDetailsForUpdate { get; init; }
        public override NameableDetails NameableDetails => NameableDetailsForUpdate;
        public override NodeIdentification NodeIdentification => NodeIdentificationForUpdate;
        public override NodeDetails NodeDetails => NodeDetailsForUpdate;
        public required NameableDetails.NameableDetailsForUpdate NameableDetailsForUpdate { get; init; }
        public override T Match<T>(Func<FirstAndBottomLevelSubdivisionToCreate, T> create, Func<FirstAndBottomLevelSubdivisionToUpdate, T> update)
        {
            return update(this);
        }
        public override void Match(Action<FirstAndBottomLevelSubdivisionToCreate> create, Action<FirstAndBottomLevelSubdivisionToUpdate> update)
        {
            update(this);
        }
    }
}

