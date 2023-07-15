namespace PoundPupLegacy.DomainModel;

public abstract record SecondLevelGlobalRegion : GlobalRegion
{
    private SecondLevelGlobalRegion() { }
    public required SecondLevelGlobalRegionDetails SecondLevelGlobalRegionDetails { get; init; }
    public required GlobalRegionDetails GlobalRegionDetails { get; init; }
    public sealed record ToCreate : SecondLevelGlobalRegion, GlobalRegionToCreate
    {
        public required Identification.Possible Identification { get; init; }
        public required NodeDetails.ForCreate NodeDetails { get; init; }
        public required NameableDetails.ForCreate NameableDetails { get; init; }
    }
    public sealed record ToUpdate : SecondLevelGlobalRegion, GlobalRegionToUpdate
    {
        public required Identification.Certain Identification { get; init; }
        public required NodeDetails.ForUpdate NodeDetails { get; init; }
        public required NameableDetails.ForUpdate NameableDetails { get; init; }
    }
}

public sealed record SecondLevelGlobalRegionDetails
{
    public required int FirstLevelGlobalRegionId { get; init; }
}
