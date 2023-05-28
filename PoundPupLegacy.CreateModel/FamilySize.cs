﻿namespace PoundPupLegacy.CreateModel;

public abstract record FamilySize : Nameable
{
    private FamilySize() { }
    public abstract NodeIdentification NodeIdentification { get; }
    public abstract NodeDetails NodeDetails { get; }
    public abstract NameableDetails NameableDetails { get; }
    public abstract T Match<T>(Func<FamilySizeToCreate, T> create, Func<FamilySizeToUpdate, T> update);
    public abstract void Match(Action<FamilySizeToCreate> create, Action<FamilySizeToUpdate> update);

    public sealed record FamilySizeToCreate : FamilySize, NameableToCreate
    {
        public override NodeIdentification NodeIdentification => NodeIdentificationForCreate;
        public override NodeDetails NodeDetails => NodeDetailsForCreate;
        public override NameableDetails NameableDetails => NameableDetailsForCreate;
        public required NodeIdentification.NodeIdentificationForCreate NodeIdentificationForCreate { get; init; }
        public required NodeDetails.NodeDetailsForCreate NodeDetailsForCreate { get; init; }
        public required NameableDetails.NameableDetailsForCreate NameableDetailsForCreate { get; init; }
        public override T Match<T>(Func<FamilySizeToCreate, T> create, Func<FamilySizeToUpdate, T> update)
        {
            return create(this);
        }
        public override void Match(Action<FamilySizeToCreate> create, Action<FamilySizeToUpdate> update)
        {
            create(this);
        }
    }
    public sealed record FamilySizeToUpdate : FamilySize, NameableToUpdate
    {
        public required NodeIdentification.NodeIdentificationForUpdate NodeIdentificationForUpdate { get; init; }
        public required NodeDetails.NodeDetailsForUpdate NodeDetailsForUpdate { get; init; }
        public override NameableDetails NameableDetails => NameableDetailsForUpdate;
        public override NodeIdentification NodeIdentification => NodeIdentificationForUpdate;
        public override NodeDetails NodeDetails => NodeDetailsForUpdate;
        public required NameableDetails.NameableDetailsForUpdate NameableDetailsForUpdate { get; init; }
        public override T Match<T>(Func<FamilySizeToCreate, T> create, Func<FamilySizeToUpdate, T> update)
        {
            return update(this);
        }
        public override void Match(Action<FamilySizeToCreate> create, Action<FamilySizeToUpdate> update)
        {
            update(this);
        }
    }
}
