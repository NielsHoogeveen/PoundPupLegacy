﻿namespace PoundPupLegacy.CreateModel;

public abstract record FormalIntermediateLevelSubdivision : ISOCodedFirstLevelSubdivision, IntermediateLevelSubdivision
{
    private FormalIntermediateLevelSubdivision() { }
    public required PoliticalEntityDetails PoliticalEntityDetails { get; init; }
    public required ISOCodedSubdivisionDetails ISOCodedSubdivisionDetails { get; init; }
    public required SubdivisionDetails SubdivisionDetails { get; init; }
    public abstract NodeIdentification NodeIdentification { get; }
    public abstract NodeDetails NodeDetails { get; }
    public abstract NameableDetails NameableDetails { get; }
    public abstract T Match<T>(Func<FormalIntermediateLevelSubdivisionToCreate, T> create, Func<FormalIntermediateLevelSubdivisionToUpdate, T> update);
    public abstract void Match(Action<FormalIntermediateLevelSubdivisionToCreate> create, Action<FormalIntermediateLevelSubdivisionToUpdate> update);

    public sealed record FormalIntermediateLevelSubdivisionToCreate : FormalIntermediateLevelSubdivision, ISOCodedFirstLevelSubdivisionToCreate, IntermediateLevelSubdivisionToCreate
    {
        public override NodeIdentification NodeIdentification => NodeIdentificationForCreate;
        public override NodeDetails NodeDetails => NodeDetailsForCreate;
        public override NameableDetails NameableDetails => NameableDetailsForCreate;
        public required NodeIdentification.NodeIdentificationForCreate NodeIdentificationForCreate { get; init; }
        public required NodeDetails.NodeDetailsForCreate NodeDetailsForCreate { get; init; }
        public required NameableDetails.NameableDetailsForCreate NameableDetailsForCreate { get; init; }
        public override T Match<T>(Func<FormalIntermediateLevelSubdivisionToCreate, T> create, Func<FormalIntermediateLevelSubdivisionToUpdate, T> update)
        {
            return create(this);
        }
        public override void Match(Action<FormalIntermediateLevelSubdivisionToCreate> create, Action<FormalIntermediateLevelSubdivisionToUpdate> update)
        {
            create(this);
        }
    }
    public sealed record FormalIntermediateLevelSubdivisionToUpdate : FormalIntermediateLevelSubdivision, ISOCodedFirstLevelSubdivisionToUpdate, IntermediateLevelSubdivisionToUpdate
    {
        public required NodeIdentification.NodeIdentificationForUpdate NodeIdentificationForUpdate { get; init; }
        public required NodeDetails.NodeDetailsForUpdate NodeDetailsForUpdate { get; init; }
        public override NameableDetails NameableDetails => NameableDetailsForUpdate;
        public override NodeIdentification NodeIdentification => NodeIdentificationForUpdate;
        public override NodeDetails NodeDetails => NodeDetailsForUpdate;
        public required NameableDetails.NameableDetailsForUpdate NameableDetailsForUpdate { get; init; }
        public override T Match<T>(Func<FormalIntermediateLevelSubdivisionToCreate, T> create, Func<FormalIntermediateLevelSubdivisionToUpdate, T> update)
        {
            return update(this);
        }
        public override void Match(Action<FormalIntermediateLevelSubdivisionToCreate> create, Action<FormalIntermediateLevelSubdivisionToUpdate> update)
        {
            update(this);
        }
    }
}

