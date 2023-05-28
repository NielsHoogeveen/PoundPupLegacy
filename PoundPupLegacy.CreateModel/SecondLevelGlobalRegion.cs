namespace PoundPupLegacy.CreateModel;

public abstract record SecondLevelGlobalRegion : GlobalRegion
{
    private SecondLevelGlobalRegion() { }
    public required SecondLevelGlobalRegionDetails SecondLevelGlobalRegionDetails { get; init; }
    public required GlobalRegionDetails GlobalRegionDetails { get; init; }
    public abstract NodeIdentification NodeIdentification { get; }
    public abstract NodeDetails NodeDetails { get; }
    public abstract NameableDetails NameableDetails { get; }
    public abstract T Match<T>(Func<SecondLevelGlobalRegionToCreate, T> create, Func<SecondLevelGlobalRegionToUpdate, T> update);
    public abstract void Match(Action<SecondLevelGlobalRegionToCreate> create, Action<SecondLevelGlobalRegionToUpdate> update);

    public sealed record SecondLevelGlobalRegionToCreate : SecondLevelGlobalRegion, GlobalRegionToCreate
    {
        public override NodeIdentification NodeIdentification => NodeIdentificationForCreate;
        public override NodeDetails NodeDetails => NodeDetailsForCreate;
        public override NameableDetails NameableDetails => NameableDetailsForCreate;
        public required NodeIdentification.NodeIdentificationForCreate NodeIdentificationForCreate { get; init; }
        public required NodeDetails.NodeDetailsForCreate NodeDetailsForCreate { get; init; }
        public required NameableDetails.NameableDetailsForCreate NameableDetailsForCreate { get; init; }
        public override T Match<T>(Func<SecondLevelGlobalRegionToCreate, T> create, Func<SecondLevelGlobalRegionToUpdate, T> update)
        {
            return create(this);
        }
        public override void Match(Action<SecondLevelGlobalRegionToCreate> create, Action<SecondLevelGlobalRegionToUpdate> update)
        {
            create(this);
        }
    }
    public sealed record SecondLevelGlobalRegionToUpdate : SecondLevelGlobalRegion, GlobalRegionToUpdate
    {
        public required NodeIdentification.NodeIdentificationForUpdate NodeIdentificationForUpdate { get; init; }
        public required NodeDetails.NodeDetailsForUpdate NodeDetailsForUpdate { get; init; }
        public override NameableDetails NameableDetails => NameableDetailsForUpdate;
        public override NodeIdentification NodeIdentification => NodeIdentificationForUpdate;
        public override NodeDetails NodeDetails => NodeDetailsForUpdate;
        public required NameableDetails.NameableDetailsForUpdate NameableDetailsForUpdate { get; init; }
        public override T Match<T>(Func<SecondLevelGlobalRegionToCreate, T> create, Func<SecondLevelGlobalRegionToUpdate, T> update)
        {
            return update(this);
        }
        public override void Match(Action<SecondLevelGlobalRegionToCreate> create, Action<SecondLevelGlobalRegionToUpdate> update)
        {
            update(this);
        }
    }
}

public sealed record SecondLevelGlobalRegionDetails
{
    public required int FirstLevelGlobalRegionId { get; init; }
}
