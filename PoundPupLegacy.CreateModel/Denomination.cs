namespace PoundPupLegacy.CreateModel;

public abstract record Denomination : Nameable
{
    private Denomination() { }
    public abstract Identification Identification { get; }
    public abstract NodeDetails NodeDetails { get; }
    public abstract NameableDetails NameableDetails { get; }
    public abstract T Match<T>(Func<DenominationToCreate, T> create, Func<DenominationToUpdate, T> update);
    public abstract void Match(Action<DenominationToCreate> create, Action<DenominationToUpdate> update);

    public sealed record DenominationToCreate : Denomination, NameableToCreate
    {
        public override Identification Identification => IdentificationForCreate;
        public override NodeDetails NodeDetails => NodeDetailsForCreate;
        public override NameableDetails NameableDetails => NameableDetailsForCreate;
        public required Identification.IdentificationForCreate IdentificationForCreate { get; init; }
        public required NodeDetails.NodeDetailsForCreate NodeDetailsForCreate { get; init; }
        public required NameableDetails.NameableDetailsForCreate NameableDetailsForCreate { get; init; }
        public override T Match<T>(Func<DenominationToCreate, T> create, Func<DenominationToUpdate, T> update)
        {
            return create(this);
        }
        public override void Match(Action<DenominationToCreate> create, Action<DenominationToUpdate> update)
        {
            create(this);
        }
    }
    public sealed record DenominationToUpdate : Denomination, NameableToUpdate
    {
        public required Identification.IdentificationForUpdate IdentificationForUpdate { get; init; }
        public required NodeDetails.NodeDetailsForUpdate NodeDetailsForUpdate { get; init; }
        public override NameableDetails NameableDetails => NameableDetailsForUpdate;
        public override Identification Identification => IdentificationForUpdate;
        public override NodeDetails NodeDetails => NodeDetailsForUpdate;
        public required NameableDetails.NameableDetailsForUpdate NameableDetailsForUpdate { get; init; }
        public override T Match<T>(Func<DenominationToCreate, T> create, Func<DenominationToUpdate, T> update)
        {
            return update(this);
        }
        public override void Match(Action<DenominationToCreate> create, Action<DenominationToUpdate> update)
        {
            update(this);
        }
    }
}
