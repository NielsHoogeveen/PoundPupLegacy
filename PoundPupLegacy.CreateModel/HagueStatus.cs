namespace PoundPupLegacy.CreateModel;

public abstract record HagueStatus : Nameable
{
    private HagueStatus() { }
    public abstract Identification Identification { get; }
    public abstract NodeDetails NodeDetails { get; }
    public abstract NameableDetails NameableDetails { get; }
    public abstract T Match<T>(Func<HagueStatusToCreate, T> create, Func<HagueStatusToUpdate, T> update);
    public abstract void Match(Action<HagueStatusToCreate> create, Action<HagueStatusToUpdate> update);

    public sealed record HagueStatusToCreate : HagueStatus, NameableToCreate
    {
        public override Identification Identification => IdentificationForCreate;
        public override NodeDetails NodeDetails => NodeDetailsForCreate;
        public override NameableDetails NameableDetails => NameableDetailsForCreate;
        public required Identification.IdentificationForCreate IdentificationForCreate { get; init; }
        public required NodeDetails.NodeDetailsForCreate NodeDetailsForCreate { get; init; }
        public required NameableDetails.NameableDetailsForCreate NameableDetailsForCreate { get; init; }
        public override T Match<T>(Func<HagueStatusToCreate, T> create, Func<HagueStatusToUpdate, T> update)
        {
            return create(this);
        }
        public override void Match(Action<HagueStatusToCreate> create, Action<HagueStatusToUpdate> update)
        {
            create(this);
        }
    }
    public sealed record HagueStatusToUpdate : HagueStatus, NameableToUpdate
    {
        public required Identification.IdentificationForUpdate IdentificationForUpdate { get; init; }
        public required NodeDetails.NodeDetailsForUpdate NodeDetailsForUpdate { get; init; }
        public override NameableDetails NameableDetails => NameableDetailsForUpdate;
        public override Identification Identification => IdentificationForUpdate;
        public override NodeDetails NodeDetails => NodeDetailsForUpdate;
        public required NameableDetails.NameableDetailsForUpdate NameableDetailsForUpdate { get; init; }
        public override T Match<T>(Func<HagueStatusToCreate, T> create, Func<HagueStatusToUpdate, T> update)
        {
            return update(this);
        }
        public override void Match(Action<HagueStatusToCreate> create, Action<HagueStatusToUpdate> update)
        {
            update(this);
        }
    }
}
