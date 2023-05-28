namespace PoundPupLegacy.CreateModel;

public abstract record HagueStatuse : Nameable
{
    private HagueStatuse() { }
    public abstract NodeIdentification NodeIdentification { get; }
    public abstract NodeDetails NodeDetails { get; }
    public abstract NameableDetails NameableDetails { get; }
    public abstract T Match<T>(Func<HagueStatuseToCreate, T> create, Func<HagueStatuseToUpdate, T> update);
    public abstract void Match(Action<HagueStatuseToCreate> create, Action<HagueStatuseToUpdate> update);

    public sealed record HagueStatuseToCreate : HagueStatuse, NameableToCreate
    {
        public override NodeIdentification NodeIdentification => NodeIdentificationForCreate;
        public override NodeDetails NodeDetails => NodeDetailsForCreate;
        public override NameableDetails NameableDetails => NameableDetailsForCreate;
        public required NodeIdentification.NodeIdentificationForCreate NodeIdentificationForCreate { get; init; }
        public required NodeDetails.NodeDetailsForCreate NodeDetailsForCreate { get; init; }
        public required NameableDetails.NameableDetailsForCreate NameableDetailsForCreate { get; init; }
        public override T Match<T>(Func<HagueStatuseToCreate, T> create, Func<HagueStatuseToUpdate, T> update)
        {
            return create(this);
        }
        public override void Match(Action<HagueStatuseToCreate> create, Action<HagueStatuseToUpdate> update)
        {
            create(this);
        }
    }
    public sealed record HagueStatuseToUpdate : HagueStatuse, NameableToUpdate
    {
        public required NodeIdentification.NodeIdentificationForUpdate NodeIdentificationForUpdate { get; init; }
        public required NodeDetails.NodeDetailsForUpdate NodeDetailsForUpdate { get; init; }
        public override NameableDetails NameableDetails => NameableDetailsForUpdate;
        public override NodeIdentification NodeIdentification => NodeIdentificationForUpdate;
        public override NodeDetails NodeDetails => NodeDetailsForUpdate;
        public required NameableDetails.NameableDetailsForUpdate NameableDetailsForUpdate { get; init; }
        public override T Match<T>(Func<HagueStatuseToCreate, T> create, Func<HagueStatuseToUpdate, T> update)
        {
            return update(this);
        }
        public override void Match(Action<HagueStatuseToCreate> create, Action<HagueStatuseToUpdate> update)
        {
            update(this);
        }
    }
}
