namespace PoundPupLegacy.DomainModel;

public abstract record FirstLevelGlobalRegion : GlobalRegion
{
    private FirstLevelGlobalRegion() { }
    public required GlobalRegionDetails GlobalRegionDetails { get; init; }

    public sealed record ToCreate : FirstLevelGlobalRegion, GlobalRegionToCreate
    {
        public required Identification.Possible Identification { get; init; }
        public required NodeDetails.ForCreate NodeDetails { get; init; }
        public required NameableDetails.ForCreate NameableDetails { get; init; }
    }
    public sealed record ToUpdate : FirstLevelGlobalRegion, GlobalRegionToUpdate
    {
        public required Identification.Certain Identification { get; init; }
        public required NodeDetails.ForUpdate NodeDetails { get; init; }
        public required NameableDetails.ForUpdate NameableDetails { get; init; }
    }
}
