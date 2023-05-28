namespace PoundPupLegacy.CreateModel;

public abstract record Profession : Nameable
{
    private Profession() { }
    public required ProfessionDetails ProfessionDetails { get; init; }
    public abstract NodeIdentification NodeIdentification { get; }
    public abstract NodeDetails NodeDetails { get; }
    public abstract NameableDetails NameableDetails { get; }
    public abstract T Match<T>(Func<ProfessionToCreate, T> create, Func<ProfessionToUpdate, T> update);
    public abstract void Match(Action<ProfessionToCreate> create, Action<ProfessionToUpdate> update);

    public sealed record ProfessionToCreate : Profession, NameableToCreate
    {
        public override NodeIdentification NodeIdentification => NodeIdentificationForCreate;
        public override NodeDetails NodeDetails => NodeDetailsForCreate;
        public override NameableDetails NameableDetails => NameableDetailsForCreate;
        public required NodeIdentification.NodeIdentificationForCreate NodeIdentificationForCreate { get; init; }
        public required NodeDetails.NodeDetailsForCreate NodeDetailsForCreate { get; init; }
        public required NameableDetails.NameableDetailsForCreate NameableDetailsForCreate { get; init; }
        public override T Match<T>(Func<ProfessionToCreate, T> create, Func<ProfessionToUpdate, T> update)
        {
            return create(this);
        }
        public override void Match(Action<ProfessionToCreate> create, Action<ProfessionToUpdate> update)
        {
            create(this);
        }
    }
    public sealed record ProfessionToUpdate : Profession, NameableToUpdate
    {
        public required NodeIdentification.NodeIdentificationForUpdate NodeIdentificationForUpdate { get; init; }
        public required NodeDetails.NodeDetailsForUpdate NodeDetailsForUpdate { get; init; }
        public override NameableDetails NameableDetails => NameableDetailsForUpdate;
        public override NodeIdentification NodeIdentification => NodeIdentificationForUpdate;
        public override NodeDetails NodeDetails => NodeDetailsForUpdate;
        public required NameableDetails.NameableDetailsForUpdate NameableDetailsForUpdate { get; init; }
        public override T Match<T>(Func<ProfessionToCreate, T> create, Func<ProfessionToUpdate, T> update)
        {
            return update(this);
        }
        public override void Match(Action<ProfessionToCreate> create, Action<ProfessionToUpdate> update)
        {
            update(this);
        }
    }
}

public sealed record ProfessionDetails
{
    public required bool HasConcreteSubtype { get; init; }
}
